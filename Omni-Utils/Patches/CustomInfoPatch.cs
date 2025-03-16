namespace Omni_Utils.Patches
{
    using System;
    using System.Collections.Generic;
    using CommandSystem;
    using CommandSystem.Commands.RemoteAdmin;
    using Exiled.API.Features;
    using HarmonyLib;
    using OmniCommonLibrary;
    using Utils;

    /// <summary>
    /// Patches the ChangeCustomPlayerInfo command.
    /// </summary>
    [HarmonyPatch(typeof(ChangeCustomPlayerInfoCommand), nameof(ChangeCustomPlayerInfoCommand.Execute))]
    public class CustomInfoPatch
    {
        /// <summary>
        /// The prefix.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="response">The response to be sent to the sender.</param>
        /// <returns>Whether the original method will be allowed to proceed.</returns>
        public static bool Prefix(ArraySegment<string> arguments, ICommandSender sender, string response)
        {
            // This code is probably kind of fucked up but I prefer to keep everything consistent so that
            // I have to duplicate code (when changing functions, during a major update for instance) as
            // little as possible.

            // I implemented this command as a patch because I want admins to be able to change their habits
            // as little as possible while still using my own code. So this replaces the 'custominfo' cmd

            // CustomInfoCmdRa cmd = new CustomInfoCmdRa();
            // cmd.Execute(arguments, sender, out response);
            if (!OmniUtilsPlugin.PluginInstance.Config.RolenameConfig.IsEnabled)
            {
                return true;
            }

            if (!sender.CheckPermission(PlayerPermissions.PlayersManagement, out response))
            {
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "To execute this command provide at least 1 argument!\nUsage: " + arguments.Array[0] + " ";
                return false;
            }

            string[] newargs;
            List<ReferenceHub> referenceHubList = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out newargs);
            if (referenceHubList is null)
            {
                response = "Cannot find player! Try using the player ID!";
                return false;
            }

            string log = $"{Player.Get(sender).Nickname} ({Player.Get(sender).UserId}) set rolenames of ";
            string str = newargs is null ? (string)null : string.Join(" ", newargs);
            foreach (ReferenceHub me in referenceHubList)
            {
                if (str is null)
                {
                    Player.Get(me).OSetPlayerCustomInfoAndRoleName(
                        string.Empty,
                        Player.Get(me).GetRoleName());
                }
                else
                {
                    Player.Get(me).OSetPlayerCustomInfoAndRoleName(
                        str,
                        Player.Get(me).GetRoleName());
                }

                log += $"{Player.Get(me).Nickname} ({Player.Get(me).UserId}) ";
            }

            log += "to " + str;
            Log.Info(log);
            response = log;
            return false;
        }
    }
}
