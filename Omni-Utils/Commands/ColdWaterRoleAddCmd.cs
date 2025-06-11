namespace Omni_Utils.Commands
{
    using ColdWaterLibrary.Features.Roles;
    using CommandSystem;
    using Exiled.API.Features;
    using Omni_Utils.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ColdWaterRoleAddCmd : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            IEnumerable<Player> list = Player.GetProcessedData(arguments, 0);
            string name = arguments.At(1);

            if (!int.TryParse(name, out int id) || ColdWaterRoleManager.Singleton.RegisteredRoles.TryGetValue(id, out ColdWaterRole coldWaterRole))
            {
                response = "ID not valid";
                return false;
            }

            foreach (Player p in list)
            {
                ColdWaterRoleManager.Singleton.AddRole(p, coldWaterRole);
            }

            response = $"Added Role {coldWaterRole.Name} / {coldWaterRole.Id} to {list.Count()} players.";
            return true;
        }

        public string Command { get; } = "coldwaterrole";

        public string[] Aliases { get; } = new string[] { "setcwrole", "addrole", "addrole" };

        public string Description { get; } = "Sets the rolename of all specified players. USAGE: rolename (player name or ID) (rolename)";
    }
}
