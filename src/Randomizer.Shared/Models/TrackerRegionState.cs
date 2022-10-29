﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Randomizer.Shared.Models {

    public class TrackerRegionState
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public TrackerState TrackerState { get; set; } = new();
        public string TypeName { get; set; } = string.Empty;
        public RewardType? Reward { get; set; }
        public ItemType? Medallion { get; set; }
    }

}
