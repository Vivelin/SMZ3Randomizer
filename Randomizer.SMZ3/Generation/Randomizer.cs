﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using Randomizer.Shared.Contracts;
using Randomizer.SMZ3.FileData;

namespace Randomizer.SMZ3.Generation
{
    public class Randomizer : IRandomizer
    {
        public static readonly Version version = new Version(11, 2);

        public string Id => "smz3";
        public string Name => "Super Metroid & A Link to the Past Combo Randomizer";
        public string Version => version.ToString();
        public List<IRandomizerOption> Options => RandomizerOptions.List;

        private static readonly Regex legalCharacters = new Regex(@"[A-Z0-9]", RegexOptions.IgnoreCase);
        private static readonly Regex illegalCharacters = new Regex(@"[^A-Z0-9]", RegexOptions.IgnoreCase);
        private static readonly Regex continousSpace = new Regex(@" +");

        public ISeedData GenerateSeed(IDictionary<string, string> options, string seed, CancellationToken cancellationToken)
        {
            int randoSeed;
            if (string.IsNullOrEmpty(seed))
            {
                randoSeed = System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, int.MaxValue);
                seed = randoSeed.ToString();
            }
            else
            {
                randoSeed = int.Parse(seed);
                /* The Random ctor takes the absolute value of a negative seed.
                 * This is an non-obvious behavior so we treat a negative value
                 * as out of range. */
                if (randoSeed < 0)
                    throw new ArgumentOutOfRangeException("Expected the seed option value to be an integer value in the range [0, 2147483647]");
            }

            var randoRnd = new Random(randoSeed);
            var config = RandomizerOptions.Parse(options);

            /* FIXME: Just here to semi-obfuscate race seeds until a better solution is in place */
            if (config.Race)
                randoRnd = new Random(randoRnd.Next());

            var worlds = new List<World>();
            if (config.SingleWorld)
                worlds.Add(new World(config, "Player", 0, Guid.NewGuid().ToString("N")));
            else
            {
                var players = options.ContainsKey("players") ? int.Parse(options["players"]) : 1;
                for (var p = 0; p < players; p++)
                {
                    var found = options.TryGetValue($"player-{p}", out var player);
                    if (!found)
                        throw new ArgumentException($"No name provided for player {p + 1}");
                    if (!legalCharacters.IsMatch(player))
                        throw new ArgumentException($"No alphanumeric characters found in name for player {p + 1}");
                    player = CleanPlayerName(player);
                    worlds.Add(new World(config, player, p, Guid.NewGuid().ToString("N")));
                }
            }

            var filler = new Filler(worlds, config, randoRnd, cancellationToken);
            filler.Fill();

            var playthrough = new Playthrough(worlds, config);
            var spheres = playthrough.Generate();

            var seedData = new SeedData
            {
                Guid = Guid.NewGuid().ToString("N"),
                Seed = seed,
                Game = Name,
                Mode = config.GameMode.ToLowerString(),
                Logic = $"{config.SMLogic.ToLowerString()}+{config.Z3Logic.ToLowerString()}",
                Playthrough = config.Race ? new List<Dictionary<string, string>>() : spheres,
                Worlds = new List<IWorldData>(),
            };

            /* Make sure RNG is the same when applying patches to the ROM to have consistent RNG for seed identifer etc */
            var patchSeed = randoRnd.Next();
            foreach (var world in worlds)
            {
                var patchRnd = new Random(patchSeed);
                var patch = new Patch(world, worlds, seedData.Guid, config.Race ? 0 : randoSeed, patchRnd);
                var worldData = new WorldData
                {
                    Id = world.Id,
                    Guid = world.Guid,
                    Player = world.Player,
                    Patches = patch.Create(config),
                    Locations = world.Locations
                        .Select(l => new LocationData() { LocationId = l.Id, ItemId = (int)l.Item.Type, ItemWorldId = l.Item.World.Id })
                        .ToList<ILocationData>(),
                };

                seedData.Worlds.Add(worldData);
            }

            return seedData;
        }

        public Dictionary<int, ILocationTypeData> GetLocations() =>
            new World(new Config(), "", 0, "")
                .Locations.Select(location => new LocationTypeData
                {
                    Id = location.Id,
                    Name = location.Name,
                    Type = location.Type.ToString(),
                    Region = location.Region.Name,
                    Area = location.Region.Area
                }).Cast<ILocationTypeData>().ToDictionary(locationData => locationData.Id);

        public Dictionary<int, IItemTypeData> GetItems() =>
            Enum.GetValues(typeof(ItemType)).Cast<ItemType>().Select(i => new ItemTypeData
            {
                Id = (int)i,
                Name = i.GetDescription()
            }).Cast<IItemTypeData>().ToDictionary(itemTypeData => itemTypeData.Id);

        private static string CleanPlayerName(string name)
        {
            name = illegalCharacters.Replace(name, " ");
            name = continousSpace.Replace(name, " ");
            return name.Trim();
        }

    }

}