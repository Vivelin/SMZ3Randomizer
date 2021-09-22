﻿using System.Collections.Generic;

using static Randomizer.SMZ3.ItemType;

namespace Randomizer.SMZ3.Regions.Zelda
{
    public class TurtleRock : Z3Region, IHasReward, INeedsMedallion
    {

        public override string Name => "Turtle Rock";

        public Reward Reward { get; set; } = Reward.None;
        public ItemType Medallion { get; set; }

        public TurtleRock(World world, Config config) : base(world, config)
        {
            RegionItems = new[] { KeyTR, BigKeyTR, MapTR, CompassTR };

            Locations = new List<Location> {
                new Location(this, 256+177, 0x1EA22, LocationType.Regular, "Turtle Rock - Compass Chest"),
                new Location(this, 256+178, 0x1EA1C, LocationType.Regular, "Turtle Rock - Roller Room - Left",
                    items => items.Firerod),
                new Location(this, 256+179, 0x1EA1F, LocationType.Regular, "Turtle Rock - Roller Room - Right",
                    items => items.Firerod),
                new Location(this, 256+180, 0x1EA16, LocationType.Regular, "Turtle Rock - Chain Chomps",
                    items => items.KeyTR >= 1),
                new Location(this, 256+181, 0x1EA25, LocationType.Regular, "Turtle Rock - Big Key Chest",
                    items => items.KeyTR >=
                        (!Config.Keysanity || GetLocation("Turtle Rock - Big Key Chest").ItemIs(BigKeyTR, World) ? 2 :
                            GetLocation("Turtle Rock - Big Key Chest").ItemIs(KeyTR, World) ? 3 : 4))
                    .AlwaysAllow((item, items) => item.Is(KeyTR, World) && items.KeyTR >= 3),
                new Location(this, 256+182, 0x1EA19, LocationType.Regular, "Turtle Rock - Big Chest",
                    items => items.BigKeyTR && items.KeyTR >= 2)
                    .Allow((item, items) => item.IsNot(BigKeyTR, World)),
                new Location(this, 256+183, 0x1EA34, LocationType.Regular, "Turtle Rock - Crystaroller Room",
                    items => items.BigKeyTR && items.KeyTR >= 2),
                new Location(this, 256+184, 0x1EA28, LocationType.Regular, "Turtle Rock - Eye Bridge - Top Right", LaserBridge),
                new Location(this, 256+185, 0x1EA2B, LocationType.Regular, "Turtle Rock - Eye Bridge - Top Left", LaserBridge),
                new Location(this, 256+186, 0x1EA2E, LocationType.Regular, "Turtle Rock - Eye Bridge - Bottom Right", LaserBridge),
                new Location(this, 256+187, 0x1EA31, LocationType.Regular, "Turtle Rock - Eye Bridge - Bottom Left", LaserBridge),
                new Location(this, 256+188, 0x308159, LocationType.Regular, "Turtle Rock - Trinexx",
                    items => items.BigKeyTR && items.KeyTR >= 4 && items.Lamp && CanBeatBoss(items)),
            };
        }

        private bool LaserBridge(Progression items)
        {
            return items.BigKeyTR && items.KeyTR >= 3 && items.Lamp && (items.Cape || items.Byrna || items.CanBlockLasers);
        }

        private bool CanBeatBoss(Progression items)
        {
            return items.Firerod && items.Icerod;
        }

        public override bool CanEnter(Progression items)
        {
            return Medallion switch
            {
                Bombos => items.Bombos,
                Ether => items.Ether,
                _ => items.Quake
            } && items.Sword &&
                items.MoonPearl && items.CanLiftHeavy() && items.Hammer && items.Somaria &&
                World.LightWorldDeathMountainEast.CanEnter(items);
        }

        public bool CanComplete(Progression items)
        {
            return GetLocation("Turtle Rock - Trinexx").IsAvailable(items);
        }

    }

}
