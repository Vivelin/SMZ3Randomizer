﻿using System.Collections.Generic;
using Randomizer.Shared;

namespace Randomizer.SMZ3.Regions.Zelda.LightWorld
{
    public class LightWorldSouth : Z3Region
    {
        private const int SphereOne = -10;

        public LightWorldSouth(World world, Config config) : base(world, config)
        {
            MazeRace = new Location(this, 256 + 45, 0x308142, LocationType.Regular,
                name: "Maze Race",
                vanillaItem: ItemType.HeartPiece)
                .Weighted(SphereOne);

            Library = new Location(this, 256 + 240, 0x308012, LocationType.Regular,
                name: "Library",
                vanillaItem: ItemType.Book,
                access: items => items.Boots);

            ForestClearingDigSpot = new Location(this, 256 + 241, 0x30814A, LocationType.Regular,
                name: "Flute Spot",
                alsoKnownAs: "Forest Clearing - Digging Spot",
                vanillaItem: ItemType.Flute,
                access: items => items.Shovel);

            Cave45 = new Location(this, 256 + 242, 0x308003, LocationType.Regular,
                name: "South of Grove",
                alsoKnownAs: "Cave #45",
                vanillaItem: ItemType.HeartPiece,
                access: items => items.Mirror && World.DarkWorldSouth.CanEnter(items));

            LinksHouse = new Location(this, 256 + 243, 0x1E9BC, LocationType.Regular,
                name: "Link's House",
                vanillaItem: ItemType.Lamp)
                .Weighted(SphereOne);

            Aginah = new Location(this, 256 + 244, 0x1E9F2, LocationType.Regular,
                name: "Aginah's Cave",
                alsoKnownAs: "Aggina's Cave",
                vanillaItem: ItemType.HeartPiece)
                .Weighted(SphereOne);

            DesertLedge = new Location(this, 256 + 252, 0x308143, LocationType.Regular,
                name: "Desert Ledge",
                vanillaItem: ItemType.HeartPiece,
                access: items => World.DesertPalace.CanEnter(items));

            CheckerboardCave = new Location(this, 256 + 253, 0x308005, LocationType.Regular,
                name: "Checkerboard Cave",
                vanillaItem: ItemType.HeartPiece,
                access: items => items.Mirror && (
                    (items.Flute && Logic.CanLiftHeavy(items)) ||
                    Logic.CanAccessMiseryMirePortal(items)
                ) && Logic.CanLiftLight(items));

            BombosTablet = new Location(this, 256 + 58, 0x308017, LocationType.Bombos,
                name: "Bombos Tablet",
                vanillaItem: ItemType.Bombos,
                access: items => items.Book && items.MasterSword && items.Mirror && World.DarkWorldSouth.CanEnter(items));

            LakeHyliaIsland = new Location(this, 256 + 61, 0x308144, LocationType.Regular,
                name: "Lake Hylia Island",
                vanillaItem: ItemType.HeartPiece,
                access: items => items.Flippers && items.MoonPearl && items.Mirror && (
                    World.DarkWorldSouth.CanEnter(items) ||
                    World.DarkWorldNorthEast.CanEnter(items)));

            UnderTheBridge = new Location(this, 256 + 62, 0x6BE7D, LocationType.Regular,
                name: "Hobo",
                alsoKnownAs: "Under the bridge",
                vanillaItem: ItemType.Bottle,
                access: items => items.Flippers || Logic.CanHyruleSouthFakeFlippers(items, false));

            IceCave = new Location(this, 256 + 63, 0x1EB4E, LocationType.Regular,
                name: "Ice Rod Cave",
                alsoKnownAs: "Ice Cave",
                vanillaItem: ItemType.Icerod)
                .Weighted(SphereOne);

            MiniMoldormCave = new(this);

            SwampRuins = new(this);
        }

        public override string Name => "Light World South";
        public override string Area => "Light World";

        public Location MazeRace { get; }

        public Location Library { get; }

        public Location ForestClearingDigSpot { get; }

        public Location Cave45 { get; }

        public Location LinksHouse { get; }

        public Location Aginah { get; }

        public Location DesertLedge { get; }

        public Location CheckerboardCave { get; }

        public Location BombosTablet { get; }

        public Location LakeHyliaIsland { get; }

        public Location UnderTheBridge { get; }

        public Location IceCave { get; }

        public MiniMoldormCaveRoom MiniMoldormCave { get; }

        public SwampRuinsRoom SwampRuins { get; }

        public class MiniMoldormCaveRoom : Room
        {
            public MiniMoldormCaveRoom(Region region)
                : base(region, "Mini Moldorm Cave")
            {
                FarLeftChest = new Location(this, 256 + 51, 0x1EB42, LocationType.Regular,
                    name: "Far Left",
                    vanillaItem: ItemType.ThreeBombs)
                    .Weighted(SphereOne);

                LeftChest = new Location(this, 256 + 52, 0x1EB45, LocationType.Regular,
                    name: "Left",
                    vanillaItem: ItemType.TwentyRupees)
                    .Weighted(SphereOne);

                Npc = new Location(this, 256 + 53, 0x308010, LocationType.Regular,
                    name: "NPC",
                    vanillaItem: ItemType.ThreeHundredRupees)
                    .Weighted(SphereOne);

                RightChest = new Location(this, 256 + 54, 0x1EB48, LocationType.Regular,
                    name: "Right",
                    vanillaItem: ItemType.TwentyRupees)
                    .Weighted(SphereOne);

                FarRightChest = new Location(this, 256 + 251, 0x1EB4B, LocationType.Regular,
                    name: "Far Right",
                    vanillaItem: ItemType.TenArrows)
                    .Weighted(SphereOne);
            }

            public Location FarLeftChest { get; }

            public Location LeftChest { get; }

            public Location RightChest { get; }

            public Location FarRightChest { get; }

            public Location Npc { get; }
        }

        public class SwampRuinsRoom : Room
        {
            public SwampRuinsRoom(Region region)
                : base(region, "Swamp Ruins")
            {
                FloodgateChest = new Location(this, 256 + 59, 0x1E98C, LocationType.Regular,
                   name: "Floodgate Chest",
                   vanillaItem: ItemType.ThreeBombs)
                   .Weighted(SphereOne);

                SunkedTreasure = new Location(this, 256 + 60, 0x308145, LocationType.Regular,
                    name: "Sunken Treasure",
                    vanillaItem: ItemType.HeartPiece)
                    .Weighted(SphereOne);
            }

            public Location FloodgateChest { get; }

            public Location SunkedTreasure { get; }

        }
    }

}
