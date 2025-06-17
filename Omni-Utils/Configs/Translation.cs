using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni_Utils.Configs
{
    public class Translation : ITranslation
    {
        public string DisabledCommand { get; set; } = "This command is currently disabled.";

        public string InvalidInput { get; set; } = "Invalid input";

        public string NullPlayerError { get; set; } = "You must be in-game to use this command.";

        public string IntroText { get; set; } = "\n\nYour name is {0}. You are {1}.\nYour height is {2} meters.";

        public string HeightCommand { get; set; } = "height";

        public string[] HeightCommandAliases { get; set; } = new[] { "scale" };

        public string HeightCommandDescription { get; set; } = "Sets your height, using a number.";

        public string HeightCommandTutorial { get; set; } = "Usage: height (NUMBER BETWEEN {0} and {1})";

        public string HeightCommandHeightOutOfRange { get; set; } = "Your height must be between {0}  and  {1} in order to use this command.";

        public string HeightCommandInputOutOfRange { get; set; } = "Invalid height! Please enter a number between {0}  and  {1}.";

        public string HeightCommandSuccess { get; set; } = "Height set to {0}";

        public string Nickname { get; set; } = "nickname";

        public string[] NicknameAliases { get; set; } = new[] { "nick", "name", "rename" };

        public string NicknameTutorial { get; set; } = "USAGE: nickname (NICK)" +
                           "\n \nYou can use placeholders, for example %nick% to get your username, or %name% to get the last name you set or got, or %rank% to get your rank (or a randomly picked one, if you lack one), or" +
                           " you can use %division% to get your MTF division, if you have one." +
                           " You can use %4digit% or %1digit% to get random numbers, if you wish. ";

        public string NicknameDescription { get; set; } = "Set your nickname";

        public string NicknameSuccess { get; set; } = "Set your nickname successfully to {0}";

        public string[] RolenameAliases { get; set; } = new string[] { "role", "setrole" };

        public string Rolename { get; set; } = "rolename";

        public string RolenameTutorial { get; set; } = "USAGE: rolename (NICK)" +
                           "\n \nYou can use placeholders, for example %nick% to get your username, or %name% to get the last name you set or got, or %rank% to get your rank (or a randomly picked one, if you lack one), or" +
                           " you can use %division% to get your MTF division, if you have one." +
                           " You can use %4digit% or %1digit% to get random numbers, if you wish. ";

        public string RolenameDescription { get; set; } = "Set your role name (text appearing below your username)";
        public string RolenameSuccess { get; set; } = "Set your rolename successfully to {0}";
    }
}
