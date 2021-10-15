﻿using System;
using System.Linq;
using System.Speech.Recognition;

namespace Randomizer.SMZ3.Tracking.VoiceCommands
{
    public class ZeldaDungeonTrackingModule : TrackerModule
    {
        private const string DungeonKey = "DungeonName";
        private const string RewardKey = "RewardName";

        public ZeldaDungeonTrackingModule(Tracker tracker) : base(tracker)
        {
            AddCommand("MarkDungeonRewardRule", GetMarkDungeonRewardRule(), (tracker, result) =>
            {
                var dungeon = GetDungeonFromResult(tracker, result);
                var reward = (RewardItem)result.Semantics[RewardKey].Value;

                tracker.SetDungeonReward(dungeon, reward, result.Confidence);
            });

            AddCommand("ClearDungeonRule", GetClearDungeonRule(), (tracker, result) =>
            {
                var dungeon = GetDungeonFromResult(tracker, result);

                tracker.ClearDungeon(dungeon, result.Confidence);
            });
        }

        private static ZeldaDungeon GetDungeonFromResult(Tracker tracker, RecognitionResult result)
        {
            var dungeonName = (string)result.Semantics[DungeonKey].Value;
            var dungeon = tracker.Dungeons.SingleOrDefault(x => x.Name.Contains(dungeonName, StringComparison.OrdinalIgnoreCase));
            return dungeon ?? throw new Exception($"Could not find recognized dungeon '{dungeonName}'.");
        }

        private GrammarBuilder GetMarkDungeonRewardRule()
        {
            var dungeonNames = GetDungeonNames();
            var rewardNames = new Choices();
            foreach (var reward in Enum.GetValues<RewardItem>())
            {
                rewardNames.Add(new SemanticResultValue(reward.GetName(), (int)reward));
            }

            return new GrammarBuilder()
                .Append("Hey tracker")
                .OneOf("mark", "set")
                .Append(DungeonKey, dungeonNames)
                .Append("as")
                .Append(RewardKey, rewardNames);
        }

        private GrammarBuilder GetClearDungeonRule()
        {
            var dungeonNames = GetDungeonNames();
            return new GrammarBuilder()
                .Append("Hey tracker")
                .OneOf("clear")
                .Append(DungeonKey, dungeonNames);
        }

        private Choices GetDungeonNames()
        {
            var dungeonNames = new Choices();
            foreach (var dungeon in Tracker.Dungeons)
            {
                foreach (var name in dungeon.Name)
                    dungeonNames.Add(new SemanticResultValue(name.Text, name.Text));
                dungeonNames.Add(new SemanticResultValue(dungeon.Abbreviation, dungeon.Abbreviation));
            }

            return dungeonNames;
        }
    }
}
