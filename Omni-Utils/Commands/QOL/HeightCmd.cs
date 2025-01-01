using System;
using CommandSystem;
using Exiled.API.Features;
using UnityEngine;

namespace Omni_Utils.Commands.QOL
{
    [CommandHandler(typeof(ClientCommandHandler))]
    //HeightCmd.cs by icedchqi
    //Created November 28th 2024
    public class HeightCmd : ICommand
    {
        public string Command { get; } = "height";

        public string[] Aliases { get; } = new[] { "scale" };

        public string Description { get; } = "Set your height, anywhere between 0.75 and 1.1.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if(!OmniUtilsPlugin.pluginInstance.Config.UseRoleplayHeight)
            {
                response = "This command is currently disabled.";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Usage: height (NUMBER)";
                return false;
            }
            if (player.Scale.y < 0.75 || player.Scale.y > 1.11)
            {
                response = "Your height must be between 0.75 and 1.1 in order to use this command.";
                return false;
            }
            if(!float.TryParse(arguments.Array[1], out float height))
            {
                response = "Invalid float! Please enter a valid number.";
                return false;
            }

            if (height < 0.75 || height > 1.11)
            {
                response = "Invalid height! Please enter a number between 0.75 and 1.1.";
                return false;
            }
            player.Scale=Vector3.one*height;
            response = $"Height set to {height}";
            return true;
        }
    }

}
