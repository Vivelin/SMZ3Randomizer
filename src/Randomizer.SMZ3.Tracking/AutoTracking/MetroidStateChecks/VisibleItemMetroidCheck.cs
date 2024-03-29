﻿using System.Collections.Generic;
using System.Linq;
using Randomizer.Abstractions;
using Randomizer.Data.Tracking;
using Randomizer.Data.WorldData;
using Randomizer.Shared;
using Randomizer.Shared.Enums;
using Randomizer.SMZ3.Tracking.Services;

namespace Randomizer.SMZ3.Tracking.AutoTracking.MetroidStateChecks;

public class VisibleItemMetroidCheck : IMetroidStateCheck
{
    private readonly Dictionary<int, VisibleItemMetroid> _items;
    private readonly HashSet<VisibleItemArea> _trackedAreas = new();
    private readonly IWorldService _worldService;

    public VisibleItemMetroidCheck(IWorldService worldService)
    {
        _worldService = worldService;
        var visibleItems = VisibleItems.GetVisibleItems().MetroidItems;
        _items = visibleItems
            .ToDictionary(s => s.Room, s => s);
    }

    public bool ExecuteCheck(TrackerBase tracker, AutoTrackerMetroidState currentState,
        AutoTrackerMetroidState prevState)
    {
        if (_items.TryGetValue(currentState.CurrentRoom, out var visibleItems))
        {
            CheckItems(visibleItems, currentState, tracker);
            return true;
        }
        return false;
    }

    private bool CheckItems(VisibleItemMetroid details, AutoTrackerMetroidState currentState, TrackerBase tracker)
    {
        var areas = details.Areas
            .Where(x => !_trackedAreas.Contains(x) && currentState.IsSamusInArea(x.TopLeftX, x.BottomRightX, x.TopLeftY, x.BottomRightY))
            .ToList();

        if (!areas.Any())
        {
            return false;
        }

        tracker.AutoTracker!.SetLatestViewAction($"VisibleItemMetroidCheck_{areas.First().Locations.First()}", () =>
        {
            var toClearLocations = new List<Location>();

            foreach (var area in areas)
            {
                var locations = area.Locations.Select(x => tracker.World.FindLocation(x)).ToList();

                if (locations.All(x => x.State.Cleared || x.State.Autotracked || x.State.MarkedItem == x.State.Item))
                {
                    _trackedAreas.Add(area);
                    continue;
                }

                toClearLocations.AddRange(locations.Where(x => x.State is { Cleared: false, Autotracked: false } &&
                                                     !x.State.Item.IsEquivalentTo(x.State.MarkedItem)));
            }

            foreach (var location in toClearLocations)
            {
                tracker.MarkLocation(location, location.Item.Type.GetGenericType());
            }

            if (toClearLocations.Any())
            {
                tracker.UpdateLastMarkedLocations(toClearLocations);
            }

            // Remove room if all items have been collected or marked
            if (_items[currentState.CurrentRoom].Areas
                .SelectMany(x => x.Locations.Select(l => tracker.World.FindLocation(l)))
                .All(x => x.State.Cleared || x.State.Autotracked ||
                          x.State.Item.IsEquivalentTo(x.State.MarkedItem)))
            {
                _items.Remove(currentState.CurrentRoom);
            }
        });

        return true;
    }
}
