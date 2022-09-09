﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Randomizer.Data.WorldData.Regions;
using Randomizer.Data.WorldData;
using Randomizer.SMZ3.Tracking.Configuration.ConfigTypes;

namespace Randomizer.SMZ3.Tracking.Services
{
    /// <summary>
    /// Service for retrieving information about the current state of
    /// the world
    /// </summary>
    public interface IWorldService
    {
        /// <summary>
        /// Collection of all additional region information
        /// </summary>
        public IReadOnlyCollection<RegionInfo> Regions { get; }

        /// <summary>
        /// Collection of all additional dungeon information
        /// </summary>
        public IReadOnlyCollection<DungeonInfo> Dungeons { get; }

        /// <summary>
        /// Collection of all additional room information
        /// </summary>
        public IReadOnlyCollection<RoomInfo> Rooms { get; }

        /// <summary>
        /// Collection of all additional location information
        /// </summary>
        public IReadOnlyCollection<LocationInfo> Locations { get; }

        /// <summary>
        /// Collection of all additional boss information
        /// </summary>
        public IReadOnlyCollection<BossInfo> Bosses { get; }

        /// <summary>
        /// Returns extra information for the specified region.
        /// </summary>
        /// <param name="name">
        /// The name or fully qualified type name of the region.
        /// </param>
        /// <returns>
        /// A new <see cref="RegionInfo"/> for the specified region.
        /// </returns>
        public RegionInfo Region(string name);

        /// <summary>
        /// Returns extra information for the specified region.
        /// </summary>
        /// <param name="type">
        /// The type of the region.
        /// </param>
        /// <returns>
        /// A new <see cref="RegionInfo"/> for the specified region.
        /// </returns>
        public RegionInfo Region(Type type);

        /// <summary>
        /// Returns extra information for the specified region.
        /// </summary>
        /// <param name="region">The region to get extra information for.</param>
        /// <returns>
        /// A new <see cref="RegionInfo"/> for the specified region.
        /// </returns>
        public RegionInfo Region(Region region);

        /// <summary>
        /// Returns extra information for the specified region.
        /// </summary>
        /// <typeparam name="TRegion">
        /// The type of region to get extra information for.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="RegionInfo"/> for the specified region.
        /// </returns>
        public RegionInfo Region<TRegion>() where TRegion : Region;

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <param name="name">
        /// The name or fully qualified type name of the dungeon region.
        /// </param>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region, or
        /// <c>null</c> if <paramref name="typeName"/> is not a valid dungeon.
        /// </returns>
        public DungeonInfo? Dungeon(string name);

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <param name="type">
        /// The type of dungeon to be looked up
        /// </param>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region, or
        /// <c>null</c> if <paramref name="type"/> is not a valid dungeon.
        /// </returns>
        public DungeonInfo Dungeon(Type type);

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <param name="region">
        /// The dungeon region to get extra information for.
        /// </param>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region.
        /// </returns>
        public DungeonInfo Dungeon(Region region);

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <typeparam name="TRegion">
        /// The type of region that represents the dungeon to get extra
        /// information for.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region.
        /// </returns>
        public DungeonInfo Dungeon<TRegion>() where TRegion : Region;

        /// <summary>
        /// Returns extra information for the specified room.
        /// </summary>
        /// <param name="name">
        /// The name or fully qualified type name of the room.
        /// </param>
        /// <returns>
        /// A new <see cref="RoomInfo"/> for the specified room.
        /// </returns>
        public RoomInfo Room(string name);

        /// <summary>
        /// Returns extra information for the specified room.
        /// </summary>
        /// <param name="type">
        /// The type of the room.
        /// </param>
        /// <returns>
        /// A new <see cref="RoomInfo"/> for the specified room.
        /// </returns>
        public RoomInfo Room(Type type);

        /// <summary>
        /// Returns extra information for the specified room.
        /// </summary>
        /// <param name="room">The room to get extra information for.</param>
        /// <returns>
        /// A new <see cref="RoomInfo"/> for the specified room.
        /// </returns>
        public RoomInfo Room(Room room);

        /// <summary>
        /// Returns extra information for the specified room.
        /// </summary>
        /// <typeparam name="TRoom">
        /// The type of room to get extra information for.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="RoomInfo"/> for the specified room.
        /// </returns>
        public RoomInfo Room<TRoom>() where TRoom : Room;

        /// <summary>
        /// Returns extra information for the specified location.
        /// </summary>
        /// <param name="id">The numeric ID of the location.</param>
        /// <returns>
        /// A new <see cref="LocationInfo"/> for the specified room.
        /// </returns>
        public LocationInfo Location(int id);

        /// <summary>
        /// Returns extra information for the specified location.
        /// </summary>
        /// <param name="location">
        /// The location to get extra information for.
        /// </param>
        /// <returns>
        /// A new <see cref="LocationInfo"/> for the specified room.
        /// </returns>
        public LocationInfo Location(Location location);

        /// <summary>
        /// Returns information about a specified boss
        /// </summary>
        /// <param name="name">The name of the boss</param>
        /// <returns>The <see cref="BossInfo"/> for the specified boss.</returns>
        public BossInfo Boss(string name);
    }
}
