﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Randomizer.App.Patches;
using Randomizer.Data;
using Randomizer.Data.Options;
using Randomizer.SMZ3.FileData;

namespace Randomizer.App;

/// <summary>
/// Service for collecting and applying player and ship sprites
/// </summary>
public class SpriteService
{
    private readonly ILogger<SpriteService> _logger;

    public SpriteService(ILogger<SpriteService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<Sprite> Sprites { get; set; } = new List<Sprite>();
    public IEnumerable<Sprite> LinkSprites => Sprites.Where(x => x.SpriteType == SpriteType.Link);
    public IEnumerable<Sprite> SamusSprites => Sprites.Where(x => x.SpriteType == SpriteType.Samus);
    public IEnumerable<Sprite> ShipSprites => Sprites.Where(x => x.SpriteType == SpriteType.Ship);

    /// <summary>
    /// Loads all player and ship sprites
    /// </summary>
    public void LoadSprites()
    {
        if (Sprites.Any()) return;

        Task.Run(() =>
        {
            var defaults = new List<Sprite>() { Sprite.DefaultSamus, Sprite.DefaultLink, Sprite.DefaultShip };

            var playerSprites = Directory.EnumerateFiles(SpritePath, "*.rdc", SearchOption.AllDirectories)
                .Select(LoadRdcSprite);

            var shipSprites = Directory.EnumerateFiles(Path.Combine(SpritePath, "Ships"), "*.ips", SearchOption.AllDirectories)
                .Select(LoadIpsSprite);

            var sprites = playerSprites.Concat(shipSprites).Concat(defaults).OrderBy(x => x.Name).ToList();

            var extraSpriteDirectory = Environment.ExpandEnvironmentVariables("%LocalAppData%\\SMZ3CasRandomizer\\Sprites");

            if (Directory.Exists(extraSpriteDirectory))
            {
                sprites.AddRange(Directory.EnumerateFiles(extraSpriteDirectory, "*.rdc", SearchOption.AllDirectories)
                    .Select(LoadRdcSprite));
                sprites.AddRange(Directory.EnumerateFiles(extraSpriteDirectory, "*.ips", SearchOption.AllDirectories)
                    .Select(LoadIpsSprite));
            }

            Sprites = sprites;

            _logger.LogInformation("{LinkSprites} Link Sprites | {SamusSprites} Samus Sprites | {ShipCount} Ship Sprites", LinkSprites.Count(), SamusSprites.Count(), ShipSprites.Count());
        });
    }

    /// <summary>
    /// Retrieves the random sprite image for the given sprite type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetRandomPreviewImage(SpriteType type)
    {
        var spriteFolder = type == SpriteType.Ship ? "Ships" : type.ToString();
        return Path.Combine(SpritePath, spriteFolder, "random.png");
    }

    /// <summary>
    /// Loads a Link or Samus rdc sprite
    /// </summary>
    /// <param name="path">The path to the rdc file</param>
    /// <returns>The Sprite object</returns>
    private Sprite LoadRdcSprite(string path)
    {
        using var stream = File.OpenRead(path);
        var rdc = Rdc.Parse(stream);

        var spriteType = SpriteType.Unknown;
        if (rdc.Contains<LinkSprite>())
            spriteType = SpriteType.Link;
        else if (rdc.Contains<SamusSprite>())
            spriteType = SpriteType.Samus;

        var name = Path.GetFileName(path);
        var author = rdc.Author;
        if (rdc.TryParse<MetaDataBlock>(stream, out var block))
        {
            var title = block?.Content?.Value<string>("title");
            if (!string.IsNullOrEmpty(title))
                name = title;

            var author2 = block?.Content?.Value<string>("author");
            if (string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(author2))
                author = author2;
        }

        var file = new FileInfo(path);
        var previewPath = file.FullName.Replace(file.Extension, ".png");
        if (!File.Exists(previewPath))
        {
            previewPath = GetRandomPreviewImage(spriteType);
        }

        return new Sprite(name, author, path, spriteType, previewPath);
    }

    /// <summary>
    /// Loads a ship ips sprite
    /// </summary>
    /// <param name="path">The path to the ips file</param>
    /// <returns>The sprite object</returns>
    private Sprite LoadIpsSprite(string path)
    {
        var file = new FileInfo(path);
        var parts = Path.GetFileNameWithoutExtension(path).Split(" by ");
        var name = parts[0];
        var author = parts[1];
        var previewPath = file.FullName.Replace(file.Extension, ".png");
        if (!File.Exists(previewPath))
        {
            previewPath = GetRandomPreviewImage(SpriteType.Ship);
        }
        return new Sprite(name, author, path, SpriteType.Ship, previewPath);
    }

    /// <summary>
    /// Applys a sprite to the given rom bytes
    /// </summary>
    /// <param name="sprite">The sprite to apply</param>
    /// <param name="bytes">The rom bytes to apply the sprite to</param>
    public void ApplySpriteTo(Sprite sprite, byte[] bytes)
    {
        if (sprite.IsDefault) return;

        if (sprite.SpriteType == SpriteType.Ship)
        {
            ApplyShipSpriteTo(sprite, bytes);
        }
        else if (sprite.SpriteType is SpriteType.Link or SpriteType.Samus)
        {
            ApplyRdcSpriteTo(sprite, bytes);
        }
    }

    private void ApplyShipSpriteTo(Sprite sprite, byte[] bytes)
    {
        var shipPatchFileName = sprite.FilePath;
        if (File.Exists(shipPatchFileName))
        {
            using var customShipBasePatch = IpsPatch.CustomShip();
            Rom.ApplySuperMetroidIps(bytes, customShipBasePatch);

            using var shipPatch = File.OpenRead(shipPatchFileName);
            Rom.ApplySuperMetroidIps(bytes, shipPatch);
        }
    }

    private void ApplyRdcSpriteTo(Sprite sprite, byte[] bytes)
    {
        using var stream = File.OpenRead(sprite.FilePath);
        var rdc = Rdc.Parse(stream);

        if (rdc.TryParse<LinkSprite>(stream, out var linkSprite))
            linkSprite?.Apply(bytes);

        if (rdc.TryParse<SamusSprite>(stream, out var samusSprite))
            samusSprite?.Apply(bytes);
    }

    private string SpritePath => Path.Combine(
        Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName ?? "")!,
        "Sprites");
}