using CommandSystem;
using Exiled.API.Features;
using Omni_Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni_Utils.Commands.QOL
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RoleNameClientCmd : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player is null)
            {
                response = "You must exist to run this command.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "USAGE: rolename (NICK)" +
                           "\n \nYou can use placeholders, for example %nick% to get your username, or %name% to get the last name you set or got, or %rank% to get your rank (or a randomly picked one, if you lack one), or" +
                           " you can use %division% to get your MTF division, if you have one." +
                           " You can use %4digit% or %1digit% to get random numbers, if you wish. ";
                return false;
            }

            string role = arguments.At(0);
            for (int i = 1; i < arguments.Count; i++)
            {
                role += $" {arguments.At(i)}";
            }

            player.OSetPlayerCustomInfoAndRoleName(player.GetCustomInfo(), role);
            response = $"Set role name to {role}";
            return true;
        }

        public string Command { get; } = "rolename";

        public string[] Aliases { get; } = new string[] { "role", "setrole" };

        public string Description { get; } = "Set your role name (text appearing below your username)";
    }
}
