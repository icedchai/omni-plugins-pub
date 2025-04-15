﻿using ColdWaterLibrary.Types;
using Exiled.API.Features;
using Exiled.Loader;
using Omni_Utils.Customs;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Omni_Utils.Extensions
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
                foreach (RankGroup rankGroup in OmniUtilsPlugin.consistentReplacements)
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
            foreach (RankGroup rankGroup in OmniUtilsPlugin.consistentReplacements)
            {
                if (nickname.Contains($"%{rankGroup.Name.ToLower()}%"))
                {
                    nickname = nickname.Replace($"%{rankGroup.Name}%", rank);
                }
            }

            // Inconsistent replacements are the same per-life using the rng session variable.
            foreach (RankGroup rankGroup in OmniUtilsPlugin.inconsistentReplacements)
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

        public static void OSetPlayerCustomInfoAndRoleName(this Player player, string customInfo, string role)
        {
            player.ReferenceHub.nicknameSync.Network_playerInfoToShow |= PlayerInfoArea.CustomInfo;
            player.ReferenceHub.nicknameSync.Network_playerInfoToShow &= ~(PlayerInfoArea.Role | PlayerInfoArea.Nickname);

            // CustomInfo supports line breaks, so I have the "customInfo", then the CustomName, then the rolename
            // to simulate how the InfoArea is organized in vanilla
            // (e.g:
            // Custom info
            // Jonny
            // Tutorial)
            string info = $"{(string.IsNullOrWhiteSpace(customInfo) ? string.Empty : $"{customInfo}\n")}{(player.HasCustomName ? $"<color=#944710></color>{player.CustomName}<color=#944710>*</color>" : player.Nickname)}\n{role}";

            info = ProcessNickname(info, player);
            player.ReferenceHub.nicknameSync.Network_customPlayerInfoString = info;
        }

        public static string GetCustomInfo(this Player player)
        {
            string cinfo = player.ReferenceHub.nicknameSync.Network_customPlayerInfoString ?? string.Empty;
            return cinfo.Length - cinfo.Replace(Environment.NewLine, string.Empty).Length < 3 ? string.Empty : cinfo.Split('\n')[0];
        }
        public static string GetNickname(this Player player)
        {
            string cinfo = player.ReferenceHub.nicknameSync.Network_customPlayerInfoString ?? string.Empty;

            return cinfo.Split('\n')[1] ?? player.DisplayNickname;
        }

        public static string GetRoleName(this Player player)
        {
            string cinfo = player.ReferenceHub.nicknameSync.Network_customPlayerInfoString ?? string.Empty;

            return cinfo.Split('\n').Last() ?? player.Role.Name;
        }

    }
}
