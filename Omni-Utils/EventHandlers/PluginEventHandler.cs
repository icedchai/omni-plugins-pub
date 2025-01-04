
using System;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using MEC;
using OmniCommonLibrary;
using PlayerRoles;
using System.Collections.Generic;
using RueI.Displays;
using RueI.Displays.Scheduling;
using RueI.Elements;
using UnityEngine;
using Display = RueI.Displays.Display;
using Random = UnityEngine.Random;
using Config = Omni_Utils.Configs.Config;
using Omni_Utils.API;

namespace Omni_Utils.EventHandlers
{
    //PluginEventHandler.cs by icedchqi
    //Documented November 7th 2024

    //Notes:
    //Might be a bit messy. Sorry.
    public class PluginEventHandler
    {


        public static Config config => OmniUtilsPlugin.pluginInstance.Config;


        public static Dictionary<Player, DynamicElement> playerIntroElements = new Dictionary<Player, DynamicElement>();
        public static JobToken IntroToken { get; } = new();
        public static string IntroGetter(DisplayCore hub)
        {
            Player player = Player.Get(hub.Hub);

            string output = $"Your name is {player.CustomName}. You are {player.GetRoleName()}.";
            if (config.UseRoleplayHeight)
            {
                output += $"\nYour height is {Math.Round(1.6f * player.Scale.y,1)} meters.";
            }
            return output;
        }
        public void OnChangingNickname(ChangingNicknameEventArgs e)
        {
            if(e.NewName is not null) e.NewName = PlayerExtensions.ProcessNickname(e.NewName, e.Player);
            if (config.RolenameConfig.IsEnabled) Timing.CallDelayed(0.1f, () => e.Player.SetPlayerCustomInfoAndRoleName(e.Player.GetCustomInfo(), e.Player.GetRoleName()));
        }
        public void OnChangingRole(ChangingRoleEventArgs e)
        {


            Player player = e.Player;
            player.InfoArea = PlayerInfoArea.Nickname | PlayerInfoArea.CustomInfo | PlayerInfoArea.Badge | PlayerInfoArea.UnitName;
            //Clearing relevant session variables on role change.
            if (player.SessionVariables.ContainsKey("omni_seed"))
            {
                player.SessionVariables.Remove("omni_seed");
            }
            if (player.SessionVariables.ContainsKey("omni_rank"))
            {
                player.SessionVariables.Remove("omni_rank");
            }
            if (config.NicknameConfig.IsEnabled)
            {
                if (config.NicknameConfig.RoleNicknames.TryGetValue(e.NewRole, out string nickname))
                {
                    player.CustomName = PlayerExtensions.ProcessNickname(nickname, player);
                }
                else
                {
                    player.CustomName = null;
                }

            }
            if (config.RolenameConfig.IsEnabled)
            {
                if (config.RolenameConfig.RoleRoleNames.TryGetValue(e.NewRole, out string roleName))
                {
                    player.SetPlayerCustomInfoAndRoleName("", roleName);
                }
                else
                {
                    player.SetPlayerCustomInfoAndRoleName("", player.GetRoleName());
                    player.InfoArea = PlayerInfoArea.CustomInfo | PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.UnitName;
                }
            }
            //Sets random height if the height is appropriate
            if (player.Scale.y < 1.11 && player.Scale.y > 0.74
                && config.UseRoleplayHeight)
            {
                player.Scale = Vector3.one * Random.Range( config.HeightMin, config.HeightMax);
            }

            if (!e.NewRole.IsHuman()) return;
            if (!config.NicknameConfig.ShowIntroText) return;
            ShowIntro(player);
        }
        public void ShowIntro(Player player)
        {
            Timing.CallDelayed(0.2f, () =>
            {
                if (player.IsDead || player.Role.Type == RoleTypeId.Tutorial) return;
                //Show hint
                DisplayCore core = DisplayCore.Get(player.ReferenceHub);
                Display display = new(core);
                DynamicElement element;
                if (playerIntroElements.TryGetValue(player, out element))
                {
                    playerIntroElements.Remove(player);
                    display.Elements.Remove(element);
                    core.Update();
                }

                element = new(IntroGetter, 325f);
                display.Elements.Add(element);
                playerIntroElements.Add(player, element);

                core.Scheduler.KillJob(IntroToken);
                core.Scheduler.Schedule(TimeSpan.FromSeconds(30f), () =>
                {
                    playerIntroElements.Remove(player);
                    display.Elements.Remove(element);
                });
                core.Update();
            });
        }
        public void OnPlayerJump(JumpingEventArgs e)
        {
            //Omni-2 has had a function to steal stamina when a player jumps since late-2022 to early-2023. 
            //The plugin broke and nobody bothered to write the literal nine-line solution. For years.

            if (e.Player.IsHuman & e.Player.IsUsingStamina)
            {
                if (!( e.Player.IsEffectActive<Invigorated>() || e.Player.IsEffectActive<Scp207>() ))
                {
                    e.Player.Stamina -= (OmniUtilsPlugin.pluginInstance.Config.StaminaUseOnJump * 0.01f);
                }
            }
        }
        
        public void OnAnnouncingScpTermination(AnnouncingScpTerminationEventArgs e)
        {
            if (!config.CustomTerminationAnnouncementConfig.IsEnabled) return;
            //Disables SCP termination announcements
            e.IsAllowed = false;
        }
        //This makes a CASSIE announcement for any death you configure it to. This is so that
        //important subjects other than SCPs get announcements, for instance if an O5 level personnel
        //dies.
        public void AnnounceSubjectDeath(Player attacker, Player victim)
        {
            CustomAnnouncement subjectName = null;
            if (attacker is null)
            {
                foreach (OverallRoleType newType in OmniUtilsPlugin.pluginInstance.Config.CustomTerminationAnnouncementConfig.ScpCassieString.Keys)
                {
                    if (victim.HasOverallRole(newType))
                    {
                        OmniUtilsPlugin.pluginInstance.Config.CustomTerminationAnnouncementConfig.ScpCassieString.TryGetValue(newType, out subjectName);
                    }
                }
                CustomAnnouncement fallback = config.CustomTerminationAnnouncementConfig.FallbackTerminationAnnouncement;
                Cassie.MessageTranslated(fallback.Words.Replace("%subject%", subjectName.Words), fallback.Translation.Replace("%subject%", subjectName.Translation));
                return;
            }

            string announcementName = null;
            foreach (OverallRoleType newType in OmniUtilsPlugin.pluginInstance.Config.CustomTerminationAnnouncementConfig.ScpTerminationAnnouncementIndex.Keys)
            {
                if (attacker.HasOverallRole(newType))
                {
                    if (!OmniUtilsPlugin.pluginInstance.Config.CustomTerminationAnnouncementConfig.ScpTerminationAnnouncementIndex.TryGetValue(newType, out announcementName))
                    {
                        return;
                    }
                }
            }

            string cassie;
            string subs;
            CustomAnnouncement announcement;
            OmniUtilsPlugin.pluginInstance.Config.CustomTerminationAnnouncementConfig.ScpTerminationCassieAnnouncements.TryGetValue(
                announcementName, out announcement);
            cassie = announcement.Words;
            subs = announcement.Translation;


            foreach (OverallRoleType newType in OmniUtilsPlugin.pluginInstance.Config.CustomTerminationAnnouncementConfig.ScpCassieString.Keys)
            {
                if (victim.HasOverallRole(newType))
                {
                    OmniUtilsPlugin.pluginInstance.Config.CustomTerminationAnnouncementConfig.ScpCassieString.TryGetValue(newType, out subjectName);
                }
            }

            if (subjectName is null)
            {
                return;
            }
            cassie = cassie.Replace("%subject%", subjectName.Words);
            subs = subs.Replace("%subject%", subjectName.Translation);
            //make sure unit name is not empty
            if (attacker.UnitName is not null)
            {
                cassie = cassie.Replace("%division%", OmniUtilsAPI.MakeUnitNameReadable(attacker.UnitName));
                subs = subs.Replace("%division%", attacker.UnitName);
            }
            else
            {
                cassie = cassie.Replace("%division%", "unknown");
                subs = subs.Replace("%division%", "UNKNOWN");
            }
            Cassie.MessageTranslated(cassie, subs);
        }
        public void OnPlayerDeath(DyingEventArgs e)
        {
            if (e.Player is null)
            {
                return;
            }
            if(config.NicknameConfig.ResetNamesOnMortality) 
                e.Player.CustomName = null;
            if(config.CustomTerminationAnnouncementConfig.IsEnabled)
                AnnounceSubjectDeath(e.Attacker, e.Player);
        }


       


        }
    }


