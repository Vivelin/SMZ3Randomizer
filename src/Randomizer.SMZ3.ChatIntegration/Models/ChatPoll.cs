﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.SMZ3.ChatIntegration.Models
{
    public class ChatPoll
    {
        public bool IsComplete { get; set; }
        public bool IsSuccessful { get; set; }
        public string? WinningChoice { get; set; }
    }
}
