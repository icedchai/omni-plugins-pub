using CommandSystem;
using Exiled.API.Features;
using OmniCommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni_Utils.Commands.QOL
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CheckOverallRole : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            RoleVersion role;
            Enum.TryParse( arguments.At(0),out role);
            OverallRoleType check = new OverallRoleType {
                RoleId = int.Parse(arguments.At(1)),
                RoleType = role,
            };
            response = $"{Player.Get(arguments.At(2)).HasOverallRole(check)}";
            return true;
        }

        public string Command { get; } = "checkr";
        public string[] Aliases { get; }
        public string Description { get; } = "check overall role";
    }
}
