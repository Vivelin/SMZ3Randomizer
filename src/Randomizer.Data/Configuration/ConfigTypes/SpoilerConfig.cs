﻿using System;
using Randomizer.Data.Configuration.ConfigTypes;

namespace Randomizer.Data.Configuration
{
    /// <summary>
    /// Provides the phrases for spoilers.
    /// </summary>
    public class SpoilerConfig : IMergeable<SpoilerConfig>
    {
        /// <summary>
        /// Gets the phrases to respond with when spoilers are turned on.
        /// </summary>
        public SchrodingersString EnabledSpoilers { get; init; }
            = new("Toggled spoilers on.");

        /// <summary>
        /// Gets the phrases to respond with when spoilers are turned off.
        /// </summary>
        public SchrodingersString DisabledSpoilers { get; init; }
            = new("Toggled spoilers off.");

        /// <summary>
        /// Gets the phrases to respond with when asked about an item and
        /// spoilers are disabled.
        /// </summary>
        public SchrodingersString PromptEnableItemSpoilers { get; init; }
            = new("If you want me to spoil it, say 'Hey tracker, enable spoilers'.");

        /// <summary>
        /// Gets the phrases to respond with when asked about a location and
        /// spoilers are disabled.
        /// </summary>
        public SchrodingersString PromptEnableLocationSpoilers { get; init; }
            = new("Why don't you go find out? Or just say 'Hey tracker, enable spoilers' and I might tell you.");

        /// <summary>
        /// Gets the phrases to respond with when asking about an item that is
        /// already at the highest stage.
        /// <c>{0}</c> is a placeholder for the name of the item.
        /// </summary>
        public SchrodingersString TrackedAllItemsAlready { get; init; }
            = new("You already have every {0}.");

        /// <summary>
        /// Gets the phrases to respond with when asking about an item that has
        /// already been tracked.
        /// <c>{0}</c> is a placeholder for the name of the item, including "a",
        /// "an" or "the".
        /// </summary>
        public SchrodingersString TrackedItemAlready { get; init; }
            = new("You already have {0}.");

        /// <summary>
        /// Gets the phrases to respond with when asking about an item that has
        /// been marked.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString MarkedItem { get; init; }
            = new("You've marked {0} at {1} <break strength='weak'/> in {2}");

        /// <summary>
        /// Gets the phrases to respond with when asking about a location whose
        /// item has already been marked.
        /// <c>{0}</c> is a placeholder for the name of the location. <c>{1}</c>
        /// is a placeholder for the name of the marked item, with "a", "an" or
        /// "the".
        /// </summary>
        public SchrodingersString MarkedLocation { get; init; }
             = new("You've marked {1} at {0}.");

        /// <summary>
        /// Gets the phrases to respond with when the location that was asked
        /// about did not have an item.
        /// <c>{0}</c> is a placeholder for the name of the location.
        /// </summary>
        public SchrodingersString EmptyLocation { get; init; }
            = new("{0} does not have an item. Did you forget to generate a seed first?");

        /// <summary>
        /// Gets the phrases to respond with when the item could not be found.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the".
        /// </summary>
        public SchrodingersString ItemNotFound { get; init; }
            = new("I cannot find {0}.");

        /// <summary>
        /// Gets the phrases to respond with when no instances of an item could
        /// be found anymore.
        /// <c>{0}</c> is a placeholder for the plural name of the item.
        /// </summary>
        public SchrodingersString ItemsNotFound { get; init; }
            = new("I cannot find any more {0}.");

        /// <summary>
        /// Gets the phrases to respond with when all locations that have the item are cleared.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the".
        /// </summary>
        public SchrodingersString LocationsCleared { get; init; }
            = new("You already cleared every location that has {0}.");

        /// <summary>
        /// Gets the phrases that spoil the item that is at the requested
        /// location.
        /// <c>{0}</c> is a placeholder for the name of the location. <c>{1}</c>
        /// is a placeholder for the name of the item, with "a", "an" or "the".
        /// </summary>
        public SchrodingersString LocationHasItem { get; init; }
            = new("{0} has {1}");

        /// <summary>
        /// Gets the phrases that spoil the item that is at the requested
        /// when in a multiworld game and the item belongs to another player
        /// location.
        /// <c>{0}</c> is a placeholder for the name of the location. <c>{1}</c>
        /// is a placeholder for the name of the item, with "a", "an" or "the".
        /// <c>2 is a placeholder for the name of the player the item belongs to</c>
        /// </summary>
        public SchrodingersString LocationHasItemOtherWorld { get; init; }
            = new("{0} has {1} belonging to {2}");

        /// <summary>
        /// Gets the phrases that spoil the item that is at the requested
        /// when in a multiworld game and the item belongs to the local player
        /// location.
        /// <c>{0}</c> is a placeholder for the name of the location. <c>{1}</c>
        /// is a placeholder for the name of the item, with "a", "an" or "the".
        /// </summary>
        public SchrodingersString LocationHasItemOwnWorld { get; init; }
            = new("{0} has {1} belonging to you");

        /// <summary>
        /// Gets the phrases that spoil the item that is at the requested
        /// location, when the item does not exist in the item data.
        /// <c>{0}</c> is a placeholder for the name of the location. <c>{1}</c>
        /// is a placeholder for the name of the item, with "a", "an" or "the".
        /// </summary>
        public SchrodingersString LocationHasUnknownItem { get; init; }
            = new("{0} has {1}");

        /// <summary>
        /// Gets the phrases that spoil the location that has the requested
        /// item.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemIsAtLocation { get; init; }
            = new("{0} is at {1} <break strength='weak'/> in {2}.");

        /// <summary>
        /// Gets the phrases that spoil the location that has the requested
        /// item and that location is in another world.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region. <c>3</c> is
        /// a placeholder for the name of the player for that location's world.
        /// </summary>
        public SchrodingersString ItemIsAtLocationOtherWorld { get; init; }
            = new("{0} is at {1} <break strength='weak'/> in {2} in {3}'s world.");

        /// <summary>
        /// Gets the phrases that spoil the location that has the requested
        /// item and that location is in the local world.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemIsAtLocationOwnWorld { get; init; }
            = new("{0} is at {1} <break strength='weak'/> in {2} in your world.");

        /// <summary>
        /// Gets the phrases that spoil one of the locations that has the
        /// requested item.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemsAreAtLocation { get; init; }
            = new("There is {0} at {1} <break strength='weak'/> in {2}");

        /// <summary>
        /// Gets the phrases that spoil one of the locations that has the
        /// requested item and that location is in another world.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region. <c>3</c> is
        /// a placeholder for the name of the player for that location's world.
        /// </summary>
        public SchrodingersString ItemsAreAtLocationOtherWorld { get; init; }
            = new("There is {0} at {1} <break strength='weak'/> in {2} in {3}'s world.");

        /// <summary>
        /// Gets the phrases that spoil one of the locations that has the
        /// requested item and that location is in the local world.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemsAreAtLocationOwnWorld { get; init; }
            = new("There is {0} at {1} <break strength='weak'/> in {2} in your world.");

        /// <summary>
        /// Gets the phrases that spoil the location that has the requested
        /// item, but the location is out of logic.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemIsAtOutOfLogicLocation { get; init; }
            = new("{0} is at {1} <break strength='weak'/> in {2}, but it is out of logic.");

        /// <summary>
        /// Gets the phrases that spoil the location that has the requested
        /// item, but the location is out of logic and in another player's world.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemIsAtOutOfLogicLocationOtherWorld { get; init; }
            = new("{0} is at {1} <break strength='weak'/> in {2} in {3}'s world, but it is out of logic.");

        /// <summary>
        /// Gets the phrases that spoil the location that has the requested
        /// item, but the location is out of logic and is in the local world.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemIsAtOutOfLogicLocationOwnWorld { get; init; }
            = new("{0} is at {1} <break strength='weak'/> in {2} in your world, but it is out of logic.");

        /// <summary>
        /// Gets the phrases that spoil one of the locations that has the
        /// requested item, but the location is out of logic.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemsAreAtOutOfLogicLocation { get; init; }
            = new("There is {0} at {1} <break strength='weak'/> in {2}, but you cannot get it yet.");


        /// <summary>
        /// Gets the phrases that spoil one of the locations that has the
        /// requested item, but the location is out of logic and in another
        /// player's world.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemsAreAtOutOfLogicLocationOtherWorld { get; init; }
            = new("There is {0} at {1} <break strength='weak'/> in {2} in {3}'s, but you cannot get it yet.");

        /// <summary>
        /// Gets the phrases that spoil one of the locations that has the
        /// requested item, but the location is out of logic and is in the
        /// local world.
        /// <c>{0}</c> is a placeholder for the name of the item, with "a", "an"
        /// or "the". <c>{1}</c> is a placeholder for the name of the location.
        /// <c>{2}</c> is a placeholder for the name of the region.
        /// </summary>
        public SchrodingersString ItemsAreAtOutOfLogicLocationOwnWorld { get; init; }
            = new("There is {0} at {1} <break strength='weak'/> in {2} in your world, but you cannot get it yet.");

        /// <summary>
        /// Gets the phrases that mention all the items in an area.
        /// <c>{0}</c> is a placeholder for the name of the room or region.
        /// <c>{1}</c> is a placeholder for the names of the items left.
        /// </summary>
        public SchrodingersString ItemsInArea { get; init; }
            = new("{0} has {1}");

    }
}
