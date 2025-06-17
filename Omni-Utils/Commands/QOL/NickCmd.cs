using System;
using CommandSystem;
using Exiled.API.Features;
using Omni_Utils.Configs;
using Omni_Utils.Extensions;
using Config = Omni_Utils.Configs.Config;

namespace Omni_Utils.Commands.QOL
{
    [CommandHandler(typeof(ClientCommandHandler))]
    //NickCmd.cs by icedchqi
    //Documented November 16th 2024
    public class NickCmd : ICommand
    {
        private static Translation Translation => OmniUtilsPlugin.PluginInstance.Translation;

        public string Command { get; } = Translation.Nickname;

        public string[] Aliases { get; } = Translation.NicknameAliases;

        public string Description { get; } = Translation.NicknameDescription;

        static Config config => OmniUtilsPlugin.PluginInstance.Config;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!config.NicknameConfig.IsEnabled)
            {
                response = Translation.DisabledCommand;
                return false;
            }

            // This is a client-side command, created as an improvement upon a previous nickname command for players.
            // The issues with the previous one became apparent when you had more than 24 characters in your nickname,
            // or if you had more than three words in the argument.
            Player player = Player.Get(sender);
            if (arguments.Count <= 0)
            {
                response = Translation.NicknameTutorial;
                return false;
            }

            if (player is null)
            {
                response = Translation.NullPlayerError;
                return false;
            }

            // The for loop is a bit fucked, but I don't care. This loop will cycle through every word in the command, rather than
            // only the first three.
            string name = arguments.At(0);
            for (int i = 1; i < arguments.Count; i++)
            {
                name += $" {arguments.At(i)}";
            }

            // Player.CustomName is the nickname, not Player.Nickname, which is their username.
            player.CustomName = name;
            player.SessionVariables.Remove("omni_name");
            player.SessionVariables.Add("omni_name", name);
            Log.Info($"{player.Nickname} ({player.UserId}) set nickname to {name}");
            response = string.Format(Translation.NicknameSuccess, player.CustomName);
            return true;
        }
    }

}
