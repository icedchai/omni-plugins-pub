namespace Omni_Utils.Patches
{
    using Exiled.API.Features;
    using HarmonyLib;
    using MEC;
    using Omni_Utils.Extensions;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Patches the name change.
    /// </summary>
    [HarmonyPatch(typeof(NicknameSync))]
    public static class NicknameSyncPatch
    {
        private static Dictionary<Player, string> _rolenames = new ();

        [HarmonyPrefix]
        [HarmonyPatch("CustomPlayerInfo", MethodType.Setter)]
        [HarmonyPatch("Network_customPlayerInfoString", MethodType.Setter)]
        public static bool CustomPlayerInfoPrefix(NicknameSync __instance, string value)
        {
            if (!OmniUtilsPlugin.PluginInstance.Config.RolenameConfig.IsEnabled)
            {
                return true;
            }

            if (value.Contains("<color=#944710></color>"))
            {
                return true;
            }

            Player player = Player.Get(ReferenceHub.GetHub(__instance.gameObject));
            _rolenames.Add(player, player.GetRoleName());
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch("CustomPlayerInfo", MethodType.Setter)]
        [HarmonyPatch("Network_customPlayerInfoString", MethodType.Setter)]
        public static void CustomPlayerInfoPostfix(NicknameSync __instance, string value)
        {
            if (!OmniUtilsPlugin.PluginInstance.Config.RolenameConfig.IsEnabled)
            {
                return;
            }

            if (value.Contains("<color=#944710></color>"))
            {
                return;
            }

            Player player = Player.Get(ReferenceHub.GetHub(__instance.gameObject));

            player.OSetPlayerCustomInfoAndRoleName(value, _rolenames[player]);
            _rolenames.Remove(player);
        }
    }
}
