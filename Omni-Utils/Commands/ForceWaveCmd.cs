using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Server;
using Omni_Utils.EventHandlers;
using OmniCommonLibrary;
using PlayerRoles;
using Exiled.API.Enums;

namespace Omni_Utils.Commands
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    //ForceWaveCmd.cs by icedchqi
    //Documented November 16th 2024
    public class ForceNextWave : ICommand
    {
        public string Command { get; } = "forcecustomwave";

        public string[] Aliases { get; } = new[]
        {
            "forcenextwave" ,
            "forcewave",
            "fwave",
        };

        public string Description { get; } = "Force next wave to be a specific custom squad. Only evaluates first argument.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!sender.CheckPermission(PlayerPermissions.RoundEvents))
            {
                response = "You do not have permission to use this command! Permission: PlayerPermissions.RoundEvents";
                return false;
            }
            if (arguments.Count == 0)
            {
                response = "List of available squads:";
                foreach (string crew in OmniUtilsPlugin.squadNameToIndex.Keys)
                {
                    response += $"\n{crew}";
                }
                response += $"\nCurrent squad MTF : {OmniUtilsPlugin.NextWaveMtf}";
                response += $"\nCurrent squad CI : {OmniUtilsPlugin.NextWaveCi}";
                return false;
            }

            string arg0 = arguments.At(0).ToLower();
            if (!OmniUtilsPlugin.squadNameToIndex.TryGetValue(arg0, out var squadIndex))
            {
                response = "Please input a squad";
                return false;
            }

            CustomSquad customSquad = OmniUtilsPlugin.TryGetCustomSquad(squadIndex);
            if (customSquad.SquadType.GetFaction() == Faction.FoundationStaff)
            {
                OmniUtilsPlugin.NextWaveMtf = customSquad;
                response = $"Set next MTF Spawnwave to {arg0}";
            }
            else if (customSquad.SquadType.GetFaction() == Faction.FoundationEnemy)
            {
                OmniUtilsPlugin.NextWaveCi = customSquad;
                response = $"Set next CI Spawnwave to {arg0}";
            }
            else
            {
                response = "Please input a squad";
                return false;
            }
            Log.Info($"{sender.LogName} {response}");
            return true;
        }
    }
}
