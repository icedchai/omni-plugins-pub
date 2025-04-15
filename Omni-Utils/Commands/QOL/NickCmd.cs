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
        public string Command { get; } = "nickname";

        public string[] Aliases { get; } = new[] { "nick", "name", "rename" };

        public string Description { get; } = "Set your nickname";
        static Config config => OmniUtilsPlugin.PluginInstance.Config;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!config.NicknameConfig.IsEnabled)
            {
                response = "This command is currently disabled.";
                return false;
            }
            //This is a client-side command, created as an improvement upon a previous nickname command for players.
            //The issues with the previous one became apparent when you had more than 24 characters in your nickname,
            //or if you had more than three words in the argument.
            Player player = Player.Get(sender);
            if (arguments.Count <= 0)
            {
                response = "USAGE: nickname (NICK)" +
                           "\n \nYou can use placeholders, for example %nick% to get your username, or %rank% to get your rank (or a randomly picked one, if you lack one), or" +
                           " you can use %division% to get your MTF division, if you have one." +
                           " You can use %4digit% or %1digit% to get random numbers, if you wish. ";
                return false;
            }
            if (player is null)
            {
                response = "You must exist to run this command!";
                return false;
            }
            //The for loop is a bit fucked, but I don't care. This loop will cycle through every word in the command, rather than
            //only the first three.
            string name = arguments.At(0);
            for (int i = 1; i < arguments.Count; i++)
            {
                name += $" {arguments.At(i)}";
            }

            //Player.CustomName is the nickname, not Player.Nickname, which is their username.
            player.CustomName = name;
            Log.Info($"{player.Nickname} ({player.UserId}) set nickname to {name}");
            response = $"Set your nickname to {player.CustomName}.";
            return true;
        }
    }

}
