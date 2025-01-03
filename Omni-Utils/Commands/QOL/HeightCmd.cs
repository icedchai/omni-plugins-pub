using System;
using CommandSystem;
using Exiled.API.Features;
using Omni_Utils.Configs;
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

        public string Description { get; } = $"Set your height, anywhere between {config.HeightMin} and {config.HeightMax}.";
        static Config config => OmniUtilsPlugin.pluginInstance.Config;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if(!config.UseRoleplayHeight)
            {
                response = "This command is currently disabled.";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = $"Usage: height (NUMBER BETWEEN {config.HeightMin} and {config.HeightMax})";
                return false;
            }
            if (player.Scale.y < config.HeightMin-0.01f || player.Scale.y > config.HeightMax+0.01f)
            {
                response = $"Your height must be between {config.HeightMin} and {config.HeightMax} in order to use this command.";
                return false;
            }
            if(!float.TryParse(arguments.Array[1], out float height))
            {
                response = "Invalid float! Please enter a valid number.";
                return false;
            }

            if (height < config.HeightMin - 0.01f || height > config.HeightMax + 0.01f)
            {
                response = $"Invalid height! Please enter a number between {config.HeightMin} and {config.HeightMax}.";
                return false;
            }
            player.Scale=Vector3.one*height;
            response = $"Height set to {height}";
            return true;
        }
    }

}
