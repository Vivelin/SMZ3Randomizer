﻿using System.Collections.Generic;
using Randomizer.Data.Configuration.ConfigTypes;
using Randomizer.Shared;
using Randomizer.Data.Options;
using Randomizer.Data.Services;
using Randomizer.Shared.Models;

namespace Randomizer.Data.WorldData.Regions.SuperMetroid.Brinstar
{
    public class KraidsLair : SMRegion, IHasBoss
    {
        public KraidsLair(World world, Config config, IMetadataService? metadata, TrackerState? trackerState) : base(world, config, metadata, trackerState)
        {
            WarehouseEnergyTank = new WarehouseEnergyTankRoom(this, metadata, trackerState);
            WarehouseKihunter = new WarehouseKihunterRoom(this, metadata, trackerState);
            VariaSuit = new VariaSuitRoom(this, metadata, trackerState);
            MemoryRegionId = 1;
            Boss = new Boss(Shared.Enums.BossType.Kraid, World, this, metadata, trackerState);
            Metadata = metadata?.Region(GetType()) ?? new RegionInfo("Kraid's Lair");
        }

        public override string Name => "Kraid's Lair";

        public override string Area => "Brinstar";

        public override List<string> AlsoKnownAs { get; } = new List<string>()
        {
            "Warehouse"
        };

        public Boss Boss{ get; set; }

        public WarehouseEnergyTankRoom WarehouseEnergyTank { get; }

        public WarehouseKihunterRoom WarehouseKihunter { get; }

        public VariaSuitRoom VariaSuit { get; }

        public override bool CanEnter(Progression items, bool requireRewards)
        {
            return (Logic.CanDestroyBombWalls(items) || items.SpeedBooster || Logic.CanAccessNorfairUpperPortal(items))
                && items.Super && Logic.CanPassBombPassages(items)
                && (items.HiJump || Logic.CanWallJump(WallJumpDifficulty.Medium) || Logic.CanFly(items));
        }

        public bool CanBeatBoss(Progression items)
        {
            return CanEnter(items, true) && items.CardBrinstarBoss;
        }

        public class WarehouseEnergyTankRoom : Room
        {
            public WarehouseEnergyTankRoom(KraidsLair region, IMetadataService? metadata, TrackerState? trackerState)
                : base(region, "Warehouse Energy Tank Room", metadata)
            {
                    ETank = new Location(this, LocationId.WarehouseEnergyTank, 0x8F899C, LocationType.Hidden,
                        name: "Energy Tank, Kraid",
                        vanillaItem: ItemType.ETank,
                        access: items => items.Kraid,
                        relevanceRequirement: items => region.CanBeatBoss(items),
                        memoryAddress: 0x5,
                        memoryFlag: 0x8,
                        metadata: metadata,
                        trackerState: trackerState);
            }

            public Location ETank { get; }
        }

        public class WarehouseKihunterRoom : Room
        {
            public WarehouseKihunterRoom(KraidsLair region, IMetadataService? metadata, TrackerState? trackerState)
                : base(region, "Warehouse Kihunter Room", metadata)
            {
                MissileBeforeKraid = new Location(this, LocationId.WarehouseKihunter, 0x8F89EC, LocationType.Hidden,
                    name: "Missile (Kraid)",
                    vanillaItem: ItemType.Missile,
                    access: items => Logic.CanUsePowerBombs(items),
                    memoryAddress: 0x5,
                    memoryFlag: 0x10,
                    metadata: metadata,
                    trackerState: trackerState);
            }

            public Location MissileBeforeKraid { get; }
        }

        public class VariaSuitRoom : Room
        {
            public VariaSuitRoom(KraidsLair region, IMetadataService? metadata, TrackerState? trackerState)
                : base(region, "Varia Suit Room", metadata)
            {
                KraidsItem = new Location(this, LocationId.VariaSuit, 0x8F8ACA, LocationType.Chozo,
                    name: "Varia Suit",
                    vanillaItem: ItemType.Varia,
                    access: items => items.Kraid,
                    relevanceRequirement: items => region.CanBeatBoss(items),
                    memoryAddress: 0x6,
                    memoryFlag: 0x1,
                    metadata: metadata,
                    trackerState: trackerState);
            }

            public Location KraidsItem { get; }
        }
    }
}
