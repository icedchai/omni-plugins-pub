using OmniCommonLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customs
{
    public class RankGroup
    {
        public static bool operator ==(RankGroup left, RankGroup right)
        {
            if (left.Name != right.Name)
            {
                return false;
            }
            if (left.PossibleReplacements != right.PossibleReplacements)
            {
                return false;
            }
            return true;
        }

        public static bool operator !=(RankGroup left, RankGroup right)
        {
            return !(left == right);
        }
        [Description("This string will be able to be replaced with one of the random possible_replacements in a player's nickname, by " +
            "enclosing it with two %s on each side (e.g rank would be %rank%)")]
        public string Name { get; set; } = "rank";
        public List<string> PossibleReplacements { get; set; } = new List<string> {
                "Private",
                "Private First Class",
                "Corporal",
                "Specialist",
                "Sergeant",
                "Staff Sergeant",
                "Sergeant First Class",
                "Master Sergeant",
                "First Sergeant",
                "Sergeant Major",
                "Command Sergeant Major",
                "Second Lieutenant",
                "First Lieutenant",
                "Captain",
                "Major",
                "Lieutenant Colonel",
                "Colonel",
                "Brigadier General",
                "Major General",
                "Lieutenant General",
                "General",
                };
    }
}
