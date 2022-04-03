﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.SMZ3.Msu
{
    public class Smz3MusicPack : MusicPack
    {
        public Smz3MusicPack(string? title, string? author, IDictionary<int, PcmTrackSet> tracks)
            : base(title, author, tracks)
        {
        }

        public virtual PcmTrackSet? ComboCredits => base[99];

        public virtual PcmTrackSet? this[SuperMetroidSoundtrack track]
            => base[(int)track];

        public virtual PcmTrackSet? this[ALttPSoundtrack track]
            => base[(int)track + 100];

        public virtual PcmTrackSet? this[ALttpExtendedSoundtrack track]
            => base[(int)track + 100];

        public static bool IsValidTrackNumber(int trackNumber)
            => trackNumber == 99
                || Enum.IsDefined(typeof(SuperMetroidSoundtrack), trackNumber)
                || (trackNumber > 100 && Enum.IsDefined(typeof(ALttPSoundtrack), trackNumber - 100))
                || (trackNumber > 100 && Enum.IsDefined(typeof(ALttpExtendedSoundtrack), trackNumber - 100));
        public override string ToString() => $"{base.ToString()} (SMZ3)";
    }
}
