using System;
using System.Collections.Generic;
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
        public string Command { get; } = Translation.HeightCommand;

        public string[] Aliases { get; } = Translation.HeightCommandAliases;

        public string Description { get; } = Translation.HeightCommandDescription;

        private static Translation Translation => OmniUtilsPlugin.PluginInstance.Translation;

        private static Config config => OmniUtilsPlugin.PluginInstance.Config;

        internal static List<Player> ChangedHeightThisLife = new List<Player>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if(!config.AllowHeightChange)
            {
                response = Translation.DisabledCommand;
                return false;
            }
            if (arguments.Count == 0)
            {
                response = string.Format(Translation.HeightCommandTutorial, config.HeightChangeMin, config.HeightChangeMax);
                return false;
            }
            if (player is null)
            {
                response = Translation.NullPlayerError;
            }
            if (ChangedHeightThisLife.Contains(player) && OmniUtilsPlugin.PluginInstance.Config.AllowHeightChangeMoreThanOncePerLife)
            {
                response = Translation.HeightCommandAlreadySetHeightFailure;
                return false;
            }
            if (player.Scale.y < config.HeightMin - 0.01f || player.Scale.y > config.HeightMax + 0.01f)
            {
                response = string.Format(Translation.HeightCommandHeightOutOfRange, config.HeightChangeMin, config.HeightChangeMax);
                return false;
            }
            if(!float.TryParse(arguments.Array[1], out float height))
            {
                response = Translation.InvalidInput;
                return false;
            }

            if (height < config.HeightMin - 0.01f || height > config.HeightMax + 0.01f)
            {
                response = string.Format(Translation.HeightCommandInputOutOfRange, config.HeightChangeMin, config.HeightChangeMax);
                return false;
            }
            player.Scale=Vector3.one*height;
            response = string.Format(Translation.HeightCommandSuccess, height);
            if (!ChangedHeightThisLife.Contains(player))
            {
                ChangedHeightThisLife.Add(player);
            }
            return true;
        }
    }

}
