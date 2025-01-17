﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Customs;
using Exiled.API.Features;
using Exiled.CreditTags.Features;
using MEC;
using OmniCommonLibrary;

namespace OmniCommonLibrary
{
    public class OmniCommonLibrary : Plugin<Config>
    {
        public override string Author => "icedchqi";
        public override string Name => "Omni-2 Library";
        public override string Prefix => "omni-commonlibrary";
        public override Version Version => new Version(1, 0, 1);
        public static OmniCommonLibrary pluginInstance = null;
        //I call these 'ranks' because they stay the same per-life, and all use the same data slot on a player.
        public static List<RankGroup> consistentReplacements = new List<RankGroup>();
        //These can be randomized per-life.
        public static List<RankGroup> inconsistentReplacements = new List<RankGroup>();
        public override void OnEnabled()
        {
            base.OnEnabled();
            pluginInstance = this;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            consistentReplacements = new List<RankGroup>();
            inconsistentReplacements = new List<RankGroup>();
            pluginInstance = null;
        }
    }
}