namespace OmniCommonLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Customs;
    using Exiled.API.Features;
    using Exiled.CreditTags.Features;
    using MEC;

    public class OmniCommonLibrary
    {

        // I call these 'ranks' because they stay the same per-life, and all use the same data slot on a player.
        public static List<RankGroup> consistentReplacements = new List<RankGroup>();

        // These can be randomized per-life.
        public static List<RankGroup> inconsistentReplacements = new List<RankGroup>();
    }
}