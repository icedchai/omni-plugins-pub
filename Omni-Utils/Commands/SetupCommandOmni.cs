using System;
using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Waves;
using PlayerRoles;
using Respawning;

namespace Omni_Utils.Commands.QOL
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class SetupCommandOmni : ICommand

    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            RoundSummary.RoundLock = true;
            if(!Round.IsStarted) Round.Start();
            foreach (Player player in Player.List)
            {
                player.Role.Set(RoleTypeId.Tutorial);
            }

            foreach (TimedWave timedWave in TimedWave.GetTimedWaves())
            {
                timedWave.Timer.Pause(999999);
            }

            Map.IsDecontaminationEnabled = false;
            Door.LockAll(999999,DoorLockType.AdminCommand);

            response = "You have successfully setup!";
            return true;
        }

        public string Command { get; } = "setup";

        public string[] Aliases { get; } = new string[]
        {
            "oset",
            "osetup",
            "osp",
            "ost",
        };

        public string Description { get; } =
            "Does all event setup actions automatically (roundlock, forcestart, all tutorials, lock doors, pause waves)";
    }
}