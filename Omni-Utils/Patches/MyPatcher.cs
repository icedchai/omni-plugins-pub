using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using OmniCommonLibrary;
using Utils;

namespace Omni_Utils.Patches
{
    using HarmonyLib;
    using UncomplicatedCustomRoles.Extensions;

    public class MyPatcher
    {
        // make sure DoPatching() is called at start either by
        // the mod loader or by your injector

        public static void DoPatching()
        {
            var harmony = new Harmony("me.icedchai.patch");
            harmony.PatchAll();

        }


    }

    [HarmonyPatch(typeof(PlayerExtension), nameof(PlayerExtension.ApplyCustomInfoAndRoleName))]
    //CustomRoleNamePatch by icedchqi
    //Documented November 7th 2024

    //Notes:
    //This patch patches the UCR function "ApplyCustomInfoAndRoleName" which runs whenever a rolename is applied.
    //This way, I can piggyback off the UCR functions and do minimal work.
    //ApplyCustomInfoAndRoleName doesn't get called if the custom_info field in a custom role is empty,
    //so I added a feature so that if you set it to 'none' it will be empty.    
    public class CustomRoleNamePatch
    {
        public static bool Prefix(Player player, string customInfo, string role)
        {
            player.SetPlayerCustomInfoAndRoleName(customInfo, role);
            return false;
        }
    }

    [HarmonyPatch(typeof(ChangeCustomPlayerInfoCommand), nameof(ChangeCustomPlayerInfoCommand.Execute))]
    public class CustomInfoPatch
    {
        public static bool Prefix(ArraySegment<string> arguments, ICommandSender sender, string response)
        {

            //This code is probably kind of fucked up but I prefer to keep everything consistent so that
            //I have to duplicate code (when changing functions, during a major update for instance) as 
            //little as possible.

            //I implemented this command as a patch because I want admins to be able to change their habits
            //as little as possible while still using my own code. So this replaces the 'custominfo' cmd

            //CustomInfoCmdRa cmd = new CustomInfoCmdRa();
            //cmd.Execute(arguments, sender, out response);

            if (!OmniUtilsPlugin.pluginInstance.Config.RolenameConfig.IsEnabled) return true;

            if (!sender.CheckPermission(PlayerPermissions.PlayersManagement, out response))
                return false;
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
                    Player.Get(me).SetPlayerCustomInfoAndRoleName("",
                        Player.Get(me).GetRoleName());
                }
                else
                {
                    Player.Get(me).SetPlayerCustomInfoAndRoleName(str,
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
