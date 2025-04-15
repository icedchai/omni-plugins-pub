namespace Omni_Utils.Patches
{
    using System;
    using System.Collections.Generic;
    using CommandSystem;
    using CommandSystem.Commands.RemoteAdmin;
    using Exiled.API.Features;
    using HarmonyLib;
    using Omni_Utils.Extensions;
    using Utils;

    /// <summary>
    /// Patches the name change.
    /// </summary>
    [HarmonyPatch(typeof(NicknameSync))]
    public class NicknamePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("DisplayName", MethodType.Setter)]
        [HarmonyPatch("Network_displayName", MethodType.Setter)]
        public static bool NicknamePrefix(NicknameSync __instance, string value)
        {
            Player player = Player.Get(ReferenceHub.GetHub(__instance.gameObject));
            if (value is not null)
            {
                value = PlayerExtensions.ProcessNickname(value, player);
            }
            if (OmniUtilsPlugin.PluginInstance.Config.RolenameConfig.IsEnabled)
            {
                player.OSetPlayerCustomInfoAndRoleName(player.GetCustomInfo(), player.GetRoleName());
            }

            return true;
        }
    }
}
