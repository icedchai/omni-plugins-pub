namespace Omni_Utils.EventHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ColdWaterLibrary.Enums;
    using ColdWaterLibrary.Extensions;
    using ColdWaterLibrary.Types;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.Events.EventArgs.Map;
    using Exiled.Events.EventArgs.Player;
    using MEC;
    using Omni_Utils.Extensions;
    using PlayerRoles;
    using UnityEngine;
    using Config = Omni_Utils.Configs.Config;
    using PlayerExtensions = Extensions.PlayerExtensions;
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
        // private static Dictionary<Player, DynamicElement> playerIntroElements = new Dictionary<Player, DynamicElement>();

        /// <summary>
        /// Gets the plugin's config.
        /// </summary>
        public static Config Config => OmniUtilsPlugin.PluginInstance.Config;

        /*/// <summary>
        /// Gets the intro token for RUEI.
        /// </summary>
        public static JobToken IntroToken { get; } = new ();*/

        /// <summary>
        /// Gets the intro text for a player.
        /// </summary>
        /// <param name="hub">The display core to show to.</param>
        /// <returns>The intro text to display to the player.</returns>
        public static string IntroGetter(ReferenceHub hub)
        {
            Player player = Player.Get(hub);

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
            if (!string.IsNullOrWhiteSpace(e.NewName))
            {
                e.NewName = PlayerExtensions.ProcessNickname(e.NewName, e.Player);
            }

            if (Config.RolenameConfig.IsEnabled)
            {
                Timing.CallDelayed(0.01f, () => e.Player.OSetPlayerCustomInfoAndRoleName(e.Player.GetCustomInfo(), e.Player.GetRoleName()));
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

            // Sets random height if the height is appropriate
            if (player.Scale.y < 1.11 && player.Scale.y > 0.74
                && Config.UseRoleplayHeight)
            {
                player.Scale = Vector3.one * Random.Range(Config.HeightMin, Config.HeightMax);
            }

            Timing.CallDelayed(0.1f, () =>
            {
                if (player.GetOverallRoleType().RoleType == TypeSystem.Uncomplicated)
                {
                    player.OSetPlayerCustomInfoAndRoleName(string.Empty, player.GetOverallRoleType().GetName());
                    if (Config.NicknameConfig.ShowIntroText)
                    {
                        ShowIntro(player);
                    }

                    return;
                }

                if (!player.GetCustomRoles().IsEmpty())
                {
                    if (Config.NicknameConfig.ShowIntroText)
                    {
                        ShowIntro(player);
                    }

                    return;
                }

                if (Config.NicknameConfig.IsEnabled)
                {
                    if (Config.NicknameConfig.RoleNicknames.TryGetValue(e.NewRole, out string nickname))
                    {
                        player.CustomName = nickname;
                    }
                    else
                    {
                        player.CustomName = null;
                    }
                }

                if (Config.RolenameConfig.IsEnabled)
                {
                    if (Config.RolenameConfig.RoleRoleNames.TryGetValue(e.NewRole, out string roleName))
                    {
                        player.OSetPlayerCustomInfoAndRoleName(string.Empty, roleName);
                    }
                    else
                    {
                        player.InfoArea |= PlayerInfoArea.Nickname | PlayerInfoArea.Role;
                    }
                }
            });

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

                player.ShowHint(IntroGetter(player.ReferenceHub), 10);
                // Show hint
                /*DisplayCore core = DisplayCore.Get(player.ReferenceHub);
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
                core.Update();*/
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
        }
    }
}