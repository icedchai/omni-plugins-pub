namespace Omni_Utils.Patches
{
    using Exiled.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs.Player;
    using HarmonyLib;
    using Omni_Utils.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [HarmonyPatch(typeof(CustomRole))]
    public static class CustomRolePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnInternalChangingNickname")]
        public static bool OnInternalChangingNicknamePrefix(CustomRole __instance, ChangingNicknameEventArgs ev)
        {
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch("AddRole")]
        public static void AddRolePostfix(CustomRole __instance, Player player)
        {
            if (!__instance.Check(player))
            {
                return;
            }

            player.OSetPlayerCustomInfoAndRoleName(string.Empty, __instance.CustomInfo);
        }
    }
}
