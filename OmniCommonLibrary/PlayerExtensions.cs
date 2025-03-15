using System;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using System.Collections.Generic;
using System.IO;
using PlayerRoles;
using CustomRole = Exiled.CustomRoles.API.Features.CustomRole;
using System.Linq;
using Customs;
using Exiled.Loader;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace OmniCommonLibrary
{
    public static class PlayerExtensions
    {
        public static Assembly Assembly => Loader.Plugins.FirstOrDefault(p => p.Name is "UncomplicatedCustomRoles")?.Assembly;
        public static Type PlayerExtension => Assembly.GetType("UncomplicatedCustomRoles.Extensions.PlayerExtension");

        public static Type SummonedCustomRole => Assembly.GetType("UncomplicatedCustomRoles.API.Features.SummonedCustomRole");
        public static string ProcessNickname(string nickname, Player player)
        {
            Random rng;
            if (player.SessionVariables.TryGetValue("omni_seed", out object o))
            {
                if (o is int seed)
                {
                    rng = new Random(seed);
                }
                else
                {
                    // Adds session variable omni_seed to make RNG consistent per-life. omni_seed is removed on death.
                    player.SessionVariables.Remove("omni_seed");
                    seed = DateTime.Now.Millisecond + (DateTime.Now.Second * 20);
                    player.SessionVariables.Add("omni_seed", seed);
                    rng = new Random(seed);
                }
            }
            else
            {
                int seed = DateTime.Now.Millisecond + (DateTime.Now.Second * 20);
                player.SessionVariables.Add("omni_seed", seed);
                rng = new Random(seed);
            }


            nickname = nickname.Replace("%nick%", player.Nickname)
                .Replace("%nickfirst%", $"{player.Nickname[0]}".ToUpper())
                .Replace("%division%", player.UnitName);
            nickname = nickname.Replace("%4digit%", $"{rng.Next(1000, 9999)}")
                .Replace("%1digit%", $"{rng.Next(0, 9)}");
            string rank = null;

            // If he has a rank, use it. If not, initialize one for him, and then use that, and store it for him.
            if (player.SessionVariables.TryGetValue("omni_rank", out o) && o is string)
            {
                rank = o as string;
            }
            else
            {
                if (player.SessionVariables.ContainsKey("omni_rank"))
                {
                    player.SessionVariables.Remove("omni_rank");
                }

                // Using the consistentReplacements, find out which of the consistentReplacements a player is eligible for by checking if they have a key in their name.
                // then, choose a random item and set that as their rank. People generally shouldn't be using more than one rank in a name anyways.
                foreach (RankGroup rankGroup in OmniCommonLibrary.consistentReplacements)
                {
                    if (nickname.Contains($"%{rankGroup.Name.ToLower()}%"))
                    {
                        rank = rankGroup.PossibleReplacements[rng.Next(rankGroup.PossibleReplacements.Count)];
                    }
                }

                if (rank is not null)
                {
                    player.SessionVariables.Add("omni_rank", rank);
                }
            }

            // Applies the rank to the player's name when processing it.
            foreach (RankGroup rankGroup in OmniCommonLibrary.consistentReplacements)
            {
                if (nickname.Contains($"%{rankGroup.Name.ToLower()}%"))
                {
                    nickname = nickname.Replace($"%{rankGroup.Name}%", rank);
                }
            }

            // Inconsistent replacements are the same per-life using the rng session variable.
            foreach (RankGroup rankGroup in OmniCommonLibrary.inconsistentReplacements)
            {
                if (nickname.Contains($"%{rankGroup.Name.ToLower()}%"))
                {
                    nickname = nickname.Replace($"%{rankGroup.Name}%", rankGroup.PossibleReplacements[rng.Next(rankGroup.PossibleReplacements.Count)]);
                }
            }

            return nickname;
        }

        public static string ProcessCustomInfo(string customInfo)
        {
            if (customInfo.Contains("none"))
            {
                return "";
            }
            return customInfo.Replace("[br]", "");
        }

        public static void SetPlayerCustomInfoAndRoleName(this Player player, string customInfo, string role)
        {
            // Hides the entire InfoArea except the CustomInfo and Badge. InfoArea is the text that displays
            // when you hover over another player.
            player.ReferenceHub.nicknameSync.ShownPlayerInfo -= 9;

            // CustomInfo supports line breaks, so I have the "customInfo", then the CustomName, then the rolename
            // to simulate how the InfoArea is organized in vanilla
            // (e.g:
            // Custom info
            // Jonny
            // Tutorial)
            string info = $"{ProcessCustomInfo(customInfo)}\n{player.CustomName}\n{role}";

            info = ProcessNickname(info, player);
            player.ReferenceHub.nicknameSync.Network_customPlayerInfoString = info;
        }

        public static string GetCustomInfo(this Player player)
        {
            string cinfo = player.ReferenceHub.nicknameSync.Network_customPlayerInfoString ?? string.Empty;
            return cinfo.Split('\n')[0] ?? "";
        }
        public static string GetNickname(this Player player)
        {
            string cinfo = player.ReferenceHub.nicknameSync.Network_customPlayerInfoString ?? string.Empty;

            return cinfo.Split('\n')[1] ?? player.DisplayNickname;
        }
        public static string GetRoleName(this Player player)
        {
            string cinfo = player.ReferenceHub.nicknameSync.Network_customPlayerInfoString ?? string.Empty;
            if (cinfo.Split('\n').Length < 3)
            {
                return player.Role.Name;
            }

            return cinfo?.Split('\n')?[2];
        }
        public static OverallRoleType GetOverallRole(this Player player)
        {

            if (SummonedCustomRole is not null)
            {
                MethodInfo summonedCustomRoleGet = SummonedCustomRole.GetMethod("Get", new Type[] { typeof(Player) });
                object ucrSumRole = summonedCustomRoleGet.Invoke(null, new object[] { player });
                PropertyInfo ucrRoleProp = ucrSumRole.GetType().GetProperty("Role", BindingFlags.IgnoreCase | BindingFlags.Public);
                object ucrSumRoleRole = ucrRoleProp.GetValue(ucrSumRole);
                PropertyInfo ucrRoleIdProp = ucrSumRoleRole.GetType().GetProperty("Id");
                object ucrRoleId = ucrRoleIdProp.GetValue(ucrSumRoleRole);
                if (ucrSumRole is not null)
                {
                    return new OverallRoleType { RoleType = RoleVersion.UcrRole, RoleId = (int)ucrRoleId };
                }
            }
            //Put Exiled CustomRoles here
            if (!player.GetCustomRoles().IsEmpty())
            {
                return new OverallRoleType { RoleType = RoleVersion.CrRole, RoleId = (int)player.GetCustomRoles()[0].Id };
            }
            else
            {
                return new OverallRoleType { RoleType = RoleVersion.BaseGameRole, RoleId = (sbyte)player.Role.Type };
            }
        }
        public static bool HasOverallRole(this Player player, OverallRoleType roleType)
        {

            switch (roleType.RoleType)
            {
                case RoleVersion.UcrRole:

                    if (SummonedCustomRole is null)
                    {
                        Log.Info("At PlayerExtensions.HasOverallRole, UCR is NOT installed and the code for checking UCR roles will therefore not run.");
                        return false;
                    } 
                    MethodInfo SummonedCustomRoleGet = SummonedCustomRole.GetMethod("Get", new Type[] { typeof(Player) });
                    object ucrSumRole = SummonedCustomRoleGet.Invoke(null, new object[] { player });
                    if (ucrSumRole is null) return false;
                    PropertyInfo ucrRoleProp = ucrSumRole.GetType().GetProperty("Role");
                    object ucrSumRoleRole = ucrRoleProp.GetValue(ucrSumRole);
                    PropertyInfo ucrRoleIdProp = ucrSumRoleRole.GetType().GetProperty("Id");
                    object ucrRoleId = ucrRoleIdProp.GetValue(ucrSumRoleRole);

                    return (int)ucrRoleId == roleType.RoleId;
                case RoleVersion.CrRole:
                    if (player.GetCustomRoles().IsEmpty()) return false;
                    CustomRole.TryGet((uint)roleType.RoleId, out CustomRole cr);
                    return cr is not null && (player.GetCustomRoles().Contains(cr));
                case RoleVersion.BaseGameRole:
                    if (!Enum.TryParse($"{roleType.RoleId}", out RoleTypeId roleid) || !player.GetCustomRoles().IsEmpty()) return false;
                    return (player.Role.Type == roleid);
                default: return false;
            }
        }
        public static bool HasOverallRole(this Player player, List<OverallRoleType> roleType)
        {

            foreach (OverallRoleType role in roleType)
            {
                if (player.HasOverallRole(role))
                {
                    return true;
                }
            }
            return false;
        }

        public static void SetOverallRole(this Player player, OverallRoleType roleType)
        {
            switch (roleType.RoleType)
            {
                case RoleVersion.BaseGameRole:
                    if (!Enum.TryParse(roleType.RoleId.ToString(), out RoleTypeId baserole))
                    {
                        Log.Error($"At PlayerExtensions.SetOverallRole: BaseGame Role of ID {roleType.RoleId} does not exist!");
                        return;
                    }
                    player.Role.Set(baserole);
                    break;
                case RoleVersion.UcrRole:
                    if (PlayerExtension is null) return;
                    MethodInfo SetCustomRole = PlayerExtension.GetMethod("SetCustomRole", new Type[]
                    {
                        typeof(Player),typeof(int)
                    });
                    SetCustomRole.Invoke(null, new object[] { player, roleType.RoleId });
                    //player.SetCustomRole(roleType.RoleId);
                    break;
                case RoleVersion.CrRole:
                    CustomRole crRole;
                    CustomRole.TryGet((uint)roleType.RoleId, out crRole);
                    if (crRole is null)
                    {
                        Log.Error($"At PlayerExtensions.SetOverallRole: Custom Role of ID {roleType.RoleId} does not exist!");
                        return;
                    }
                    crRole.AddRole(player);
                    break;
            }

        }

    }
}
