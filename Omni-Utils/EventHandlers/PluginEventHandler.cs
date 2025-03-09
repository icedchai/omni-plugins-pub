namespace Omni_Utils.EventHandlers
{
    using System;
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Map;
    using Exiled.Events.EventArgs.Player;
    using MEC;
    using Omni_Utils.API;
    using OmniCommonLibrary;
    using PlayerRoles;
    using RueI.Displays;
    using RueI.Displays.Scheduling;
    using RueI.Elements;
    using UnityEngine;
    using Config = Omni_Utils.Configs.Config;
    using Display = RueI.Displays.Display;
    using Random = UnityEngine.Random;

    // PluginEventHandler.cs by icedchqi
    // Documented November 7th 2024

    // Notes:
    // Might be a bit messy. Sorry.

    /// <summary>
    /// Event handler for most things.
    /// </summary>
    public class PluginEventHandler
    {
        private static Dictionary<Player, DynamicElement> playerIntroElements = new Dictionary<Player, DynamicElement>();

        /// <summary>
        /// Gets the plugin's config.
        /// </summary>
        public static Config Config => OmniUtilsPlugin.PluginInstance.Config;

        /// <summary>
        /// Gets the intro token for RUEI.
        /// </summary>
        public static JobToken IntroToken { get; } = new ();

        /// <summary>
        /// Gets the intro text for a player.
        /// </summary>
        /// <param name="hub">The display core to show to.</param>
        /// <returns>The intro text to display to the player.</returns>
        public static string IntroGetter(DisplayCore hub)
        {
            Player player = Player.Get(hub.Hub);

            string output = $"Your name is {player.CustomName}. You are {player.GetRoleName()}.";
            if (Config.UseRoleplayHeight)
            {
                output += $"\nYour height is {Math.Round(1.6f * player.Scale.y, 1)} meters.";
            }

            return output;
        }

        /// <summary>
        /// Event handler for ChangingNicknameEvent.
        /// </summary>
        /// <param name="e">The event args.</param>
        public void OnChangingNickname(ChangingNicknameEventArgs e)
        {
            if (e.NewName is not null)
            {
                e.NewName = PlayerExtensions.ProcessNickname(e.NewName, e.Player);
            }

            if (Config.RolenameConfig.IsEnabled)
            {
                Timing.CallDelayed(0.1f, () => e.Player.SetPlayerCustomInfoAndRoleName(e.Player.GetCustomInfo(), e.Player.GetRoleName()));
            }
        }

        /// <summary>
        /// Event handler for ChangingRoleEvent.
        /// </summary>
        /// <param name="e">The event args.</param>
        public void OnChangingRole(ChangingRoleEventArgs e)
        {
            Player player = e.Player;

            // Clearing relevant session variables on role change.
            if (player.SessionVariables.ContainsKey("omni_seed"))
            {
                player.SessionVariables.Remove("omni_seed");
            }

            if (player.SessionVariables.ContainsKey("omni_rank"))
            {
                player.SessionVariables.Remove("omni_rank");
            }

            if (Config.NicknameConfig.IsEnabled)
            {
                if (Config.NicknameConfig.RoleNicknames.TryGetValue(e.NewRole, out string nickname))
                {
                    player.CustomName = PlayerExtensions.ProcessNickname(nickname, player);
                }
                else
                {
                    player.CustomName = null;
                }
            }

            if (Config.RolenameConfig.IsEnabled)
            {
                player.InfoArea = PlayerInfoArea.Nickname | PlayerInfoArea.CustomInfo | PlayerInfoArea.Badge | PlayerInfoArea.UnitName;
                if (Config.RolenameConfig.RoleRoleNames.TryGetValue(e.NewRole, out string roleName))
                {
                    player.SetPlayerCustomInfoAndRoleName(string.Empty, roleName);
                }
                else
                {
                    player.SetPlayerCustomInfoAndRoleName(string.Empty, player.GetRoleName());
                    player.InfoArea = PlayerInfoArea.CustomInfo | PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.UnitName;
                }
            }

            // Sets random height if the height is appropriate
            if (player.Scale.y < 1.11 && player.Scale.y > 0.74
                && Config.UseRoleplayHeight)
            {
                player.Scale = Vector3.one * Random.Range(Config.HeightMin, Config.HeightMax);
            }

            if (!e.NewRole.IsHuman())
            {
                return;
            }

            if (!Config.NicknameConfig.ShowIntroText)
            {
                return;
            }

            ShowIntro(player);
        }

        /// <summary>
        /// Shows intro to a player.
        /// </summary>
        /// <param name="player">The player to show the intro to.</param>
        public void ShowIntro(Player player)
        {
            Timing.CallDelayed(0.2f, () =>
            {
                if (player.IsDead || player.Role.Type == RoleTypeId.Tutorial)
                {
                    return;
                }

                // Show hint
                DisplayCore core = DisplayCore.Get(player.ReferenceHub);
                Display display = new (core);
                DynamicElement element;
                if (playerIntroElements.TryGetValue(player, out element))
                {
                    playerIntroElements.Remove(player);
                    display.Elements.Remove(element);
                    core.Update();
                }

                element = new (IntroGetter, 325f);
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

        /// <summary>
        /// Event handler for JumpingEvent.
        /// </summary>
        /// <param name="e">The event args.</param>
        public void OnPlayerJump(JumpingEventArgs e)
        {
            // Omni-2 has had a function to steal stamina when a player jumps since late-2022 to early-2023.
            // The plugin broke and nobody bothered to write the literal nine-line solution. For years.
            if (e.Player.IsHuman & e.Player.IsUsingStamina)
            {
                if (!(e.Player.IsEffectActive<Invigorated>() || e.Player.IsEffectActive<Scp207>()))
                {
                    e.Player.Stamina -= OmniUtilsPlugin.PluginInstance.Config.StaminaUseOnJump * 0.01f;
                }
            }
        }

        /// <summary>
        /// Event handler for AnnouncingScpTerminationEvent.
        /// </summary>
        /// <param name="e">The event args.</param>
        public void OnAnnouncingScpTermination(AnnouncingScpTerminationEventArgs e)
        {
            if (!Config.CustomTerminationAnnouncementConfig.IsEnabled)
            {
                return;
            }

            // Disables SCP termination announcements
            e.IsAllowed = false;
        }

        /// <summary>
        /// Announces a subject's death.
        /// </summary>
        /// <param name="attacker">The player who killed the victim.</param>
        /// <param name="victim">The player whose death is being announced.</param>
        public void AnnounceSubjectDeath(Player attacker, Player victim)
        {
            CustomAnnouncement subjectName = null;
            if (attacker is null)
            {
                foreach (OverallRoleType newType in OmniUtilsPlugin.PluginInstance.Config.CustomTerminationAnnouncementConfig.ScpCassieString.Keys)
                {
                    if (victim.HasOverallRole(newType))
                    {
                        OmniUtilsPlugin.PluginInstance.Config.CustomTerminationAnnouncementConfig.ScpCassieString.TryGetValue(newType, out subjectName);
                    }
                }

                if (subjectName is null)
                {
                    return;
                }

                CustomAnnouncement fallback = Config.CustomTerminationAnnouncementConfig.FallbackTerminationAnnouncement;
                Cassie.MessageTranslated(fallback.Words.Replace("%subject%", subjectName.Words), fallback.Translation.Replace("%subject%", subjectName.Translation));
                return;
            }

            string announcementName = null;
            foreach (OverallRoleType newType in OmniUtilsPlugin.PluginInstance.Config.CustomTerminationAnnouncementConfig.ScpTerminationAnnouncementIndex.Keys)
            {
                if (attacker.HasOverallRole(newType))
                {
                    if (!OmniUtilsPlugin.PluginInstance.Config.CustomTerminationAnnouncementConfig.ScpTerminationAnnouncementIndex.TryGetValue(newType, out announcementName))
                    {
                        return;
                    }
                }
            }

            string cassie;
            string subs;
            CustomAnnouncement announcement;
            if (!OmniUtilsPlugin.PluginInstance.Config.CustomTerminationAnnouncementConfig.ScpTerminationCassieAnnouncements.TryGetValue(announcementName, out announcement))
            {
                return;
            }

            cassie = announcement.Words;
            subs = announcement.Translation;
            foreach (OverallRoleType newType in OmniUtilsPlugin.PluginInstance.Config.CustomTerminationAnnouncementConfig.ScpCassieString.Keys)
            {
                if (victim.HasOverallRole(newType))
                {
                    OmniUtilsPlugin.PluginInstance.Config.CustomTerminationAnnouncementConfig.ScpCassieString.TryGetValue(newType, out subjectName);
                }
            }

            if (subjectName is null)
            {
                return;
            }

            cassie = cassie.Replace("%subject%", subjectName.Words);
            subs = subs.Replace("%subject%", subjectName.Translation);

            // Make sure unit name is not empty.
            if (!string.IsNullOrWhiteSpace(attacker.UnitName))
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

        /// <summary>
        /// Event handler for DyingEvent.
        /// </summary>
        /// <param name="e">The event args.</param>
        public void OnPlayerDeath(DyingEventArgs e)
        {
            if (e.Player is null)
            {
                return;
            }

            if (Config.NicknameConfig.ResetNamesOnMortality)
            {
                e.Player.CustomName = null;
            }

            if (Config.CustomTerminationAnnouncementConfig.IsEnabled)
            {
                AnnounceSubjectDeath(e.Attacker, e.Player);
            }
        }
    }
}