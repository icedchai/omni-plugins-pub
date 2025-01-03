using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Customs;
using Exiled.API.Features;
using Exiled.CreditTags.Features;
using Exiled.Loader;
using MEC;
using OmniCommonLibrary;

namespace OmniCommonLibrary
{
    public static class UcrIntegration
    {
        public static void Reflect()
        {
            Assembly = Loader.Plugins.FirstOrDefault(p => p.Name is "UncomplicatedCustomRoles")?.Assembly;
            PlayerExtension = Assembly.GetType("UncomplicatedCustomRoles.Extensions.PlayerExtension");
            SummonedCustomRole = Assembly.GetType("UncomplicatedCustomRoles.API.Features.SummonedCustomRole");
            SummonedCustomRoleGet = SummonedCustomRole.GetMethod("Get", new Type[] { typeof(Player) });
            ucrRoleProp = SummonedCustomRole.GetProperty("Role", BindingFlags.IgnoreCase | BindingFlags.Public);
            UcrCustomRole = Assembly.GetType("UncomplicatedCustomRoles.API.Features.CustomRole");
            ucrRoleIdProp = UcrCustomRole.GetProperty("Id");
        }
        public static Assembly Assembly;
        public static Type PlayerExtension;


        public static Type SummonedCustomRole;
        public static MethodInfo SummonedCustomRoleGet;
        public static PropertyInfo ucrRoleProp;
        public static Type UcrCustomRole;
        public static PropertyInfo ucrRoleIdProp;
    }
    public class OmniCommonLibrary : Plugin<Config>
    {
        public override string Author => "icedchqi";
        public override string Name => "Omni Library";
        public override string Prefix => "omni-commonlibrary";
        public override Version Version => new Version(1, 0, 0);
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