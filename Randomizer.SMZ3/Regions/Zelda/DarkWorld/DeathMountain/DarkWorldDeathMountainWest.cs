﻿using System.Collections.Generic;

namespace Randomizer.SMZ3.Regions.Zelda.DarkWorld.DeathMountain
{
    public class DarkWorldDeathMountainWest : Z3Region
    {
        public DarkWorldDeathMountainWest(World world, Config config)
            : base(world, config)
        {
            SpikeCave = new(this);
        }

        public override string Name => "Dark World Death Mountain West";

        public override string Area => "Dark World";

        public SpikeCaveRoom SpikeCave { get; }

        public class SpikeCaveRoom : Room
        {
            public SpikeCaveRoom(Region region)
                : base(region, "Spike Cave")
            {
                Chest = new Location(this, 256 + 64, 0x1EA8B, LocationType.Regular,
                    "Spike Cave",
                    items => items.MoonPearl && items.Hammer && items.CanLiftLight() &&
                        ((items.CanExtendMagic() && items.Cape) || items.Byrna) &&
                        World.LightWorldDeathMountainWest.CanEnter(items));
            }

            public Location Chest { get; }
        }
    }
}