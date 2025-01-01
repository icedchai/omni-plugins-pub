using System;
using Exiled.API.Features;
using Exiled.CustomRoles;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Features.Roles;
using PlayerRoles;
using CustomRole = Exiled.CustomRoles.API.Features.CustomRole;
using OmniCommonLibrary;
using System.Linq;
using UncomplicatedCustomRoles.API.Features;
using Mirror;
using Customs;
using Exiled.Loader;
using System.Reflection;

namespace OmniCommonLibrary
{
    public static class PlayerExtensions
    {
        public static Assembly Assembly => Loader.Plugins.FirstOrDefault(p => p.Name is "UncomplicatedCustomRoles")?.Assembly;
        public static Type PlayerExtension => Assembly.GetType("UncomplicatedCustomRoles.Extensions.PlayerExtension");
        public static Type SummonedCustomRole => Assembly.GetType("UncomplicatedCustomRoles.API.Features.SummonedCustomRole");
        public static string ProcessNickname(string nickname, Player player)
        {
            Config config = OmniCommonLibrary.pluginInstance.Config;
            Random rng;
            if (player.SessionVariables.TryGetValue("omni_seed", out Object o))
            {
                if (o is int seed)
                {
                    rng = new Random(seed);
                }
                else
                {
                    //Adds session variable omni_seed to make RNG consistent per-life. omni_seed is removed on death.
                    player.SessionVariables.Remove("omni_seed");
                    seed = DateTime.Now.Millisecond + DateTime.Now.Second * 20;
                    player.SessionVariables.Add("omni_seed", seed);
                    rng = new Random(seed);
                }
            }
            else
            {
                int seed = DateTime.Now.Millisecond + DateTime.Now.Second * 20;
                player.SessionVariables.Add("omni_seed", seed);
                rng = new Random(seed);
            }

            nickname = nickname.Replace("%nick%", player.Nickname)
                .Replace("%nickfirst%", $"{player.Nickname[0]}".ToUpper())
                .Replace("%division%", player.UnitName);
            nickname = nickname.Replace("%4digit%", $"{rng.Next(1000, 9999)}")
                .Replace("%1digit%", $"{rng.Next(0, 9)}");
            string rank = null;

            //If he has a rank, use it. If not, initialize one for him, and then use that, and store it for him.
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
                //Using the consistentReplacements, find out which of the consistentReplacements a player is eligible for by checking if they have a key in their name.
                //then, choose a random item and set that as their rank. People generally shouldn't be using more than one rank in a name anyways.
                foreach (RankGroup rankGroup in OmniCommonLibrary.consistentReplacements)
                {
                    if(nickname.Contains($"%{rankGroup.Name.ToLower()}%")) rank = rankGroup.PossibleReplacements[rng.Next(rankGroup.PossibleReplacements.Count)];
                }
                if (rank is not null) player.SessionVariables.Add("omni_rank", rank);
            }
            //Applies the rank to the player's name when processing it.
            foreach(RankGroup rankGroup in OmniCommonLibrary.consistentReplacements)
            {
                if (nickname.Contains($"%{rankGroup.Name.ToLower()}%")) nickname = nickname.Replace($"%{rankGroup.Name}%", rank);
            }
            //Inconsistent replacements are the same per-life using the rng session variable.
            foreach(RankGroup rankGroup in OmniCommonLibrary.inconsistentReplacements)
            {
                if (nickname.Contains($"%{rankGroup.Name.ToLower()}%")) nickname = nickname.Replace($"%{rankGroup.Name}%", rankGroup.PossibleReplacements[rng.Next(rankGroup.PossibleReplacements.Count)]);
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
            //Hides the entire InfoArea except the CustomInfo and Badge. InfoArea is the text that displays
            //when you hover over another player.
            player.InfoArea = PlayerInfoArea.CustomInfo | PlayerInfoArea.Badge | PlayerInfoArea.UnitName;
            player.ReferenceHub.nicknameSync.ShownPlayerInfo = PlayerInfoArea.CustomInfo | PlayerInfoArea.Badge | PlayerInfoArea.UnitName;
            if (role.Contains("</"))
                Log.Error($"Failed to apply CustomInfo with Role name at PlayerExtension::ApplyCustomInfoAndRoleName(%Player, string, string): role name can't contains any end tag like </color>, </b>, </size> etc...!\nCustomInfo won't be applied to player {player.Nickname} ({player.Id}) -- Found: {role}");

            if (customInfo.StartsWith("<"))
                Log.Error($"Failed to apply CustomInfo with Role name at PlayerExtension::ApplyCustomInfoAndRoleName(%Player, string, string): role custom_info can't contains any tag like </olor>, <b>, <size> etc...!\nCustomInfo won't be applied to player {player.Nickname} ({player.Id}) -- Found: {customInfo}");
            //player.ReferenceHub.nicknameSync.Network_customPlayerInfoString = $"{player.CustomName}\n{ProcessCustomInfo(customInfo)}\n{role}";

            //CustomInfo supports line breaks, so I have the "customInfo", then the CustomName, then the rolename
            //to simulate how the InfoArea is organized in vanilla
            //(e.g:
            //Custom info
            //Jonny
            //Tutorial)
            string info = $"{ProcessCustomInfo(customInfo)}\n{player.CustomName}\n{role}";
            //Replaces %division% with the player's UnitName, if applicable. This allows for custom roles to have the name 
            //of their MTF unit in their role name (like XRAY-12)
            //eg Hammer-Down Captain (%division%)
            //turns into
            //Hammer-Down Captain (GOLF-09)
            info = ProcessNickname(info, player);
            player.ReferenceHub.nicknameSync.Network_customPlayerInfoString = info;
        }
        public static string GetCustomInfo(this Player player)
        {
            string first;
            using (var reader = new StringReader(player.ReferenceHub.nicknameSync.Network_customPlayerInfoString))
            {
                first = reader.ReadLine();
            }
            return first;
        }
        public static string GetNickname(this Player player)
        {

            string second;

            using (var reader = new StringReader(player.ReferenceHub.nicknameSync.Network_customPlayerInfoString))
            {
                reader.ReadLine();
                second = reader.ReadLine();
            }
            return second;
        }
        public static string GetRoleName(this Player player)
        {
            string third;
            using (var reader = new StringReader(player.ReferenceHub.nicknameSync.Network_customPlayerInfoString))
            {
                reader.ReadLine();
                reader.ReadLine();
                third = reader.ReadLine();
            }
            if (third is null)
            {
                if (player.GetCustomRoles().Count == 0) return player.Role.Name;
                else
                {
                    return player.GetCustomRoles().First().Name;
                }
            }
            return third;
        }
        public static OverallRoleType GetOverallRole(this Player player)
        {

            if (SummonedCustomRole is not null)
            {
                MethodInfo SummonedCustomRoleGet = SummonedCustomRole.GetMethod("Get", new Type[] { typeof(Player) });
                object ucrSumRole = SummonedCustomRoleGet.Invoke(null, new object[] { player });
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

                    if (SummonedCustomRole is null) return false;
                    MethodInfo SummonedCustomRoleGet = SummonedCustomRole.GetMethod("Get", new Type[] { typeof(Player) });
                    Log.Debug($"SummonedCustomRoleGet is null {SummonedCustomRoleGet is null}");
                    object ucrSumRole = SummonedCustomRoleGet.Invoke(null, new object[] { player });
                    Log.Debug($"ucrSumRole is null {ucrSumRole is null}");
                    PropertyInfo ucrRoleProp = ucrSumRole.GetType().GetProperty("Role");
                    Log.Debug($"ucrRoleProp is null {ucrRoleProp is null}");
                    object ucrSumRoleRole = ucrRoleProp.GetValue(ucrSumRole);
                    Log.Debug($"ucrSumRoleRole is null {ucrSumRoleRole is null}");
                    PropertyInfo ucrRoleIdProp = ucrSumRoleRole.GetType().GetProperty("Id");
                    Log.Debug($"ucrRoleIdProp is null {ucrRoleIdProp is null}");
                    object ucrRoleId = ucrRoleIdProp.GetValue(ucrSumRoleRole);
                    Log.Debug($"ucrRoleId is null {ucrRoleId is null}");

                    if (ucrSumRole is null) return false;
                    return ((int)ucrRoleId == roleType.RoleId);
                case RoleVersion.CrRole:
                    if (player.GetCustomRoles().IsEmpty()) return false;
                    CustomRole.TryGet((uint)roleType.RoleId, out CustomRole cr);
                    return cr is not null && (player.GetCustomRoles().Contains(cr));
                case RoleVersion.BaseGameRole:
                    MethodInfo HasCustomRole = PlayerExtension.GetMethod("HasCustomRole");
                    bool hasUcrRole = (bool)HasCustomRole.Invoke(null, new object[] { player });
                    if (!Enum.TryParse($"{roleType.RoleId}", out RoleTypeId roleid)|| hasUcrRole ||!player.GetCustomRoles().IsEmpty()) return false;
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
