using CommandSystem;
using Exiled.API.Features;
using Omni_Utils.Configs;
using Omni_Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni_Utils.Commands.QOL
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class RoleNameClientCmd : ICommand
    {
        private static Translation Translation => OmniUtilsPlugin.PluginInstance.Translation;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player is null)
            {
                response = Translation.NullPlayerError;
                return false;
            }

            if (arguments.Count == 0)
            {
                response = Translation.RolenameTutorial;
                return false;
            }

            string role = arguments.At(0);
            for (int i = 1; i < arguments.Count; i++)
            {
                role += $" {arguments.At(i)}";
            }

            player.OSetPlayerCustomInfoAndRoleName(player.GetCustomInfo(), role);
            response = string.Format(Translation.RolenameSuccess, role);
            return true;
        }

        public string Command { get; } = "rolename";

        public string[] Aliases { get; } = new string[] { "role", "setrole" };

        public string Description { get; } = Translation.RolenameDescription;
    }
}
