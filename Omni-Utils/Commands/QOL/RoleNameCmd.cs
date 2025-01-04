using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using OmniCommonLibrary;

namespace Omni_Utils.Commands.QOL
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RoleNameCmd : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            IEnumerable<Player> list = Player.GetProcessedData(arguments, 0);
            string name = arguments.At(1);
            for (int i = 2; i < arguments.Count; i++)
            {
                name += $" {arguments.At(i)}";
            }
            foreach (Player p in list)
            {
                p.SetPlayerCustomInfoAndRoleName(p.GetCustomInfo(),name);
            }
            response = $"Added rolename {name} to {list.Count()} players.";
            return true;
        }

        public string Command { get; } = "rolename";
        public string[] Aliases { get; }
        public string Description { get; } = "Sets the rolename of all specified players. USAGE: rolename (player name or ID) (rolename)";
    }
}