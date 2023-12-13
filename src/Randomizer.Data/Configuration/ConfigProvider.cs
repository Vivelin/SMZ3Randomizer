﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Randomizer.Data.Configuration.ConfigFiles;
using Randomizer.Data.Configuration.ConfigTypes;
using Randomizer.Shared;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Randomizer.Data.Configuration
{
    /// <summary>
    /// Provides tracker configuration data.
    /// </summary>
    public partial class ConfigProvider
    {
        private static readonly JsonSerializerOptions s_options = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        private static readonly IDeserializer s_deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        private readonly string _basePath;
        private readonly ILogger<ConfigProvider>? _logger;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ConfigProvider"/> class.
        /// </summary>
        public ConfigProvider(ILogger<ConfigProvider>? logger)
        {
#if DEBUG
            _basePath = Path.Combine(GetSourceDirectory(), "Randomizer.Data", "Configuration", "Yaml");
#else
            _basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMZ3CasRandomizer", "Configs");
#endif
            _logger = logger;
        }

        /// <summary>
        /// Loads the maps configuration.
        /// </summary>
        /// <returns>A new <see cref="TrackerMapConfig"/> object.</returns>
        public virtual TrackerMapConfig GetMapConfig()
            => LoadConfig<TrackerMapConfig>("maps.json");

        /// <summary>
        /// Loads the specified configuration instance from the specified
        /// filename, falling back to the embedded resource with the same name.
        /// </summary>
        /// <typeparam name="T">The type of configuration to load.</typeparam>
        /// <param name="fileName">The file name to load from.</param>
        /// <returns>The configuration.</returns>
        /// <exception cref="InvalidOperationException">
        /// An unknown error occurred while deserializing the JSON file.
        /// </exception>
        protected virtual T LoadConfig<T>(string fileName)
        {
#if DEBUG
            return GetBuiltInConfig<T>(fileName);
#else
            var jsonPath = Path.Combine(_basePath, fileName);
            if (!File.Exists(jsonPath))
            {
                _logger?.LogWarning("Could not find configuration file '{path}'. Falling back to embedded resource.", jsonPath);
                return GetBuiltInConfig<T>(fileName);
            }

            var json = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<T>(json, s_options)
                ?? throw new InvalidOperationException($"The '{jsonPath}' configuration could not be loaded.");
#endif
        }

        /// <summary>
        /// Returns the configs with additional information for bosses
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual BossConfig GetBossConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<BossConfig, BossInfo>("bosses.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with additional information for dungeons
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual DungeonConfig GetDungeonConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<DungeonConfig, DungeonInfo>("dungeons.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with additional information for items
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual ItemConfig GetItemConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<ItemConfig, ItemData>("items.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with additional information for locations
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual LocationConfig GetLocationConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<LocationConfig, LocationInfo>("locations.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with additional information for regions
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual RegionConfig GetRegionConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<RegionConfig, RegionInfo>("regions.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with additional requests
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual RequestConfig GetRequestConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<RequestConfig, BasicVoiceRequest>("requests.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with tracker responses
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual ResponseConfig GetResponseConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<ResponseConfig, ResponseConfig>("responses.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with additional information for rooms
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual RoomConfig GetRoomConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<RoomConfig, RoomInfo>("rooms.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with additional information for rewards
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual RewardConfig GetRewardConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<RewardConfig, RewardInfo>("rewards.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with UI layouts
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual UIConfig GetUIConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<UIConfig, UILayout>("ui.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with in game text
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual GameLinesConfig GetGameConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<GameLinesConfig, GameLinesConfig>("game.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with msus
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual MsuConfig GetMsuConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<MsuConfig, MsuConfig>("msu.yml", profiles, mood);

        /// <summary>
        /// Returns the configs with hint tiles
        /// </summary>
        /// <param name="profiles">The selected tracker profile(s) to load</param>
        /// <returns></returns>
        public virtual HintTileConfig GetHintTileConfig(IReadOnlyCollection<string> profiles, string? mood) =>
            LoadYamlConfigs<HintTileConfig, HintTileConfig>("hint_tiles.yml", profiles, mood);

        /// <summary>
        /// Returns a collection of all possible config profiles to
        /// select from
        /// </summary>
        /// <returns></returns>
        public virtual ICollection<string> GetAvailableProfiles()
        {
            return Directory
                .GetDirectories(_basePath)
                .Select(x => new DirectoryInfo(x).Name)
                .Where(x => x != "Templates")
                .ToList();
        }

        /// <summary>
        /// Returns a list of the moods available in the selected profiles.
        /// </summary>
        /// <param name="profiles">The names of profiles to query.</param>
        /// <returns>A list of names of moods that are available in the selected profiles.</returns>
        public virtual IReadOnlyList<string> GetAvailableMoods(IReadOnlyCollection<string> profiles)
        {
            return GetBuiltInMoods(profiles)
                .Concat(GetFileSystemMoods(profiles))
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToImmutableList();
        }

        /// <summary>
        /// Returns the path of the config files
        /// </summary>
        public virtual string ConfigDirectory => _basePath;

        protected virtual IReadOnlyList<string> GetBuiltInMoods(IReadOnlyCollection<string> profiles)
        {
            return Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .Select(x =>
                {
                    foreach (var profile in profiles)
                    {
                        var expectedPrefix = $"Randomizer.Data.Configuration.Yaml.{profile}";
                        if (x.StartsWith(expectedPrefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var embeddedFileName = x.Substring(expectedPrefix.Length);
                            var match = ProfileFileNameWithMoodRegex().Match(embeddedFileName);
                            if (match.Success)
                            {
                                return match.Groups["mood"].Value;
                            }
                        }
                    }

                    return null;
                })
                .NonNull()
                .Distinct()
                .ToImmutableList();
        }

        protected virtual IReadOnlyList<string> GetFileSystemMoods(IReadOnlyCollection<string> profiles)
        {
            return Directory.EnumerateFiles(_basePath, "*.yml", SearchOption.AllDirectories)
                .Select(path =>
                {
                    foreach (var profile in profiles)
                    {
                        var expectedPrefix = Path.Combine(_basePath, profile);
                        if (path.StartsWith(expectedPrefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var embeddedFileName = Path.GetFileName(path);
                            var match = ProfileFileNameWithMoodRegex().Match(embeddedFileName);
                            if (match.Success)
                            {
                                return match.Groups["mood"].Value;
                            }
                        }
                    }

                    return null;
                })
                .NonNull()
                .Distinct()
                .ToImmutableList();
        }

        private T LoadYamlConfigs<T, T2>(string fileName, IReadOnlyCollection<string>? profiles = null, string? mood = null) where T : new()
        {
            var defaultMethod = typeof(T).GetMethod("Default")
                ?? throw new InvalidOperationException($"The class '{typeof(T).Name}' does not have a Default method.");
            var config = (T)(defaultMethod.Invoke(null, null)
                ?? new T()) ?? throw new InvalidOperationException($"The class '{typeof(T).Name}' does not implement IConfigFile.");
            var mergeableConfig = (IMergeable<T2>)config
                ?? throw new InvalidOperationException($"The class '{typeof(T).Name}' does not implement IMergeable.");

            if (profiles == null || profiles.Count == 0) return config;

            foreach (var profile in profiles)
            {
                if (!string.IsNullOrEmpty(profile))
                {
                    TryMergeYamlFile(mergeableConfig, fileName, profile);

                    var moodFileName = GetMoodFileName(fileName, mood);
                    if (moodFileName != null)
                    {
                        TryMergeYamlFile(mergeableConfig, moodFileName, profile);
                    }
                }
            }

            return config;

            void TryMergeYamlFile(IMergeable<T2> mergeableConfig, string fileName, string profile)
            {
                var otherConfig = LoadYamlFile<T>(fileName, profile);
                if (otherConfig == null)
                    return;

                var otherMergeableConfig = (IMergeable<T2>)otherConfig;
                if (otherConfig == null)
                    throw new InvalidOperationException($"The class '{typeof(T).Name}' does not implement IMergeable.");

                mergeableConfig.MergeFrom(otherMergeableConfig);
            }
        }

        private string? GetMoodFileName(string fileName, string? mood)
        {
            if (string.IsNullOrEmpty(mood))
                return null;

            var extension = Path.GetExtension(fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            return $"{fileNameWithoutExtension}.{mood}{extension}";
        }

        private T? LoadYamlFile<T>(string fileName, string profile)
        {
            var path = Path.Combine(_basePath, profile, fileName);
            var yml = "";
            if (!File.Exists(path))
                yml = LoadBuiltInYamlFile(fileName, profile);
            else
            {
                yml = File.ReadAllText(path);
            }
            T? obj = default;
            try
            {
                obj = s_deserializer.Deserialize<T>(yml);
            }
            catch (Exception ex) when (ex is YamlDotNet.Core.SemanticErrorException or YamlDotNet.Core.YamlException)
            {
                _logger?.LogError(ex, "Unable to load config file {Path}", path);
                throw new YamlDotNet.Core.SemanticErrorException("Unable to load config file " + path, ex);
            }
            return obj;
        }

        private static string LoadBuiltInYamlFile(string fileName, string profile)
        {
            var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"Randomizer.Data.Configuration.Yaml.{profile}.{fileName}");
            if (stream == null)
                return "";
            using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);
            return reader.ReadToEnd();
        }

        private static T GetBuiltInConfig<T>(string fileName)
        {
            var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Randomizer.Data." + fileName);
            if (stream == null)
                throw new FileNotFoundException($"Could not find embedded stream in Randomizer.SMZ3.Tracking for '{fileName}'.");

            using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);
            return JsonSerializer.Deserialize<T>(reader.ReadToEnd(), s_options)
                ?? throw new InvalidOperationException("The embedded tracker configuration could not be loaded.");
        }

        private string GetSourceDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directory = Directory.GetParent(currentDirectory);
            while (directory != null && directory.Name != "src")
            {
                directory = Directory.GetParent(directory.FullName);
            }
            return directory?.FullName ?? "";
        }

        [GeneratedRegex("[^\\.]+\\.(?<mood>.+)\\.yml")]
        private static partial Regex ProfileFileNameWithMoodRegex();
    }
}
