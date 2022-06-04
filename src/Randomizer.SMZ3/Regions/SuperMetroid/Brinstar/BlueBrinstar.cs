﻿using Randomizer.Shared;

namespace Randomizer.SMZ3.Regions.SuperMetroid.Brinstar
{
    public class BlueBrinstar : SMRegion
    {
        public BlueBrinstar(World world, Config config) : base(world, config)
        {
            MorphBall = new(this, 26, 0x8F86EC, LocationType.Visible,
                name: "Morphing Ball",
                alsoKnownAs: "Morph Ball (Corridor No. 1)",
                vanillaItem: ItemType.Morph,
                memoryAddress: 0x3,
                memoryFlag: 0x4);

            PowerBomb = new(this, 27, 0x8F874C, LocationType.Visible,
                name: "Power Bomb (blue Brinstar)",
                alsoKnownAs: "Power Bomb wall (Corridor No. 1)",
                vanillaItem: ItemType.PowerBomb,
                access: items => Logic.CanUsePowerBombs(items),
                memoryAddress: 0x3,
                memoryFlag: 0x8);

            MiddleMissile = new(this, 28, 0x8F8798, LocationType.Visible,
                name: "Missile (blue Brinstar middle)",
                vanillaItem: ItemType.Missile,
                access: items => items.CardBrinstarL1 && items.Morph,
                memoryAddress: 0x3,
                memoryFlag: 0x10);

            Ceiling = new(this, 29, 0x8F879E, LocationType.Hidden,
                name: "Energy Tank, Brinstar Ceiling",
                vanillaItem: ItemType.ETank,
                access: items => items.CardBrinstarL1 && (Logic.CanFly(items) || items.HiJump || items.SpeedBooster || items.Ice),
                memoryAddress: 0x3,
                memoryFlag: 0x20);

            BottomMissile = new(this, 34, 0x8F8802, LocationType.Chozo,
                name: "Missile (blue Brinstar bottom)",
                vanillaItem: ItemType.Missile,
                access: items => items.Morph,
                memoryAddress: 0x4,
                memoryFlag: 0x4);

            BlueBrinstarTop = new(this);

            MemoryRegionId = 1;
        }

        public override string Name => "Blue Brinstar";

        public override string Area => "Brinstar";

        public Location MorphBall { get; }

        public Location PowerBomb { get; }

        public Location MiddleMissile { get; }

        public Location Ceiling { get; }

        public Location BottomMissile { get; }

        public BlueBrinstarTopRoom BlueBrinstarTop { get; }

        public class BlueBrinstarTopRoom : Room
        {
            public BlueBrinstarTopRoom(BlueBrinstar region)
                : base(region, "Blue Brinstar Top")
            {
                MainItem = new(this, 36, 0x8F8836, LocationType.Visible,
                    name: "Main Item",
                    alsoKnownAs: new[] { "Missile (blue Brinstar top)", "Billy Mays Room" },
                    vanillaItem: ItemType.Missile,
                    access: items => items.CardBrinstarL1 && Logic.CanUsePowerBombs(items),
                    memoryAddress: 0x4,
                    memoryFlag: 0x10);

                HiddenItem = new(this, 37, 0x8F883C, LocationType.Hidden,
                    name: "Hidden Item",
                    alsoKnownAs: new[] { "Missile (blue Brinstar behind missile)", "Billy Mays Room - Hidden item" },
                    vanillaItem: ItemType.Missile,
                    access: items => items.CardBrinstarL1 && Logic.CanUsePowerBombs(items),
                    memoryAddress: 0x4,
                    memoryFlag: 0x20);
            }

            public Location MainItem { get; }

            public Location HiddenItem { get; }

        }
    }
}
