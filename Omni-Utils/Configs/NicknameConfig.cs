using Omni_Utils.Customs;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni_Utils.Configs
{
    public class NicknameConfig
    {
        [Description("Whether this feature set is enabled.\n" +
            "There are several keywords to look out for, but mostly they're %4digit% (four random digits), %nick% (player's display name)" +
            ", %rank% (random rank from E-1 to O-10). You can also create your own lists of ranks, below.")]
        public bool IsEnabled { get; set; } = true;
        [Description("Whether to show the hint to a player telling them their nickname, and role name.")]
        public bool ShowIntroText { get; set; } = true;
        [Description("Reset nicknames after death.")]
        public bool ResetNamesOnMortality { get; set; } = true;
        public Dictionary<RoleTypeId, string> RoleNicknames { get; set; } = new Dictionary<RoleTypeId, string> {

            { RoleTypeId.ClassD, "D-%nickfirst%%4digit%" },
            { RoleTypeId.Scientist, "Staff %nick%" },
            { RoleTypeId.FacilityGuard, "Agent %nick%" },

            { RoleTypeId.NtfCaptain, "%rankhigh% %nick%" },
            { RoleTypeId.NtfSergeant, "%rankmid% %nick%" },
            { RoleTypeId.NtfPrivate, "%ranklow% %nick%" },

            { RoleTypeId.ChaosConscript, "Recruit %nick%" },
            { RoleTypeId.ChaosMarauder, "%ranklow% %nick%" },
            { RoleTypeId.ChaosRepressor, "%ranklow% %nick%" },
            { RoleTypeId.ChaosRifleman, "%ranklow% %nick%" },
        };
        [Description("Make sure that when you use these, you enwrap it in two %s. These are meant to be used in nicknames, or rolenames! " +
            "Note that it stays the same no matter what during one life (If a player puts %rank% in their name, and they get the rank Private, and they do it again, it stays the same.)")]
        public List<RankGroup> RankGroups { get; set; } = new List<RankGroup>
        {
            new RankGroup(),
            new RankGroup{
                Name="ranklow",
                PossibleReplacements = new List<string>
                {
                    "Private",
                    "Private First Class",
                    "Corporal",
                    "Specialist",
                }
            },
            new RankGroup{
                Name="rankmid",
                PossibleReplacements = new List<string>
                {
                    "Corporal",
                    "Sergeant",
                    "Staff Sergeant",
                    "Gunnery Sergeant",
                    "Master Sergeant",
                }
            },
            new RankGroup{
                Name="rankhigh",
                PossibleReplacements = new List<string>
                {
                    "Second Lieutenant",
                    "First Lieutenant",
                    "Captain",
                    "Major",
                    "Lieutenant Colonel",
                }
            },
        };
        public List<RankGroup> RandomReplacements { get; set; } = new List<RankGroup>
        {
            new RankGroup{Name = "firstname_white",PossibleReplacements=new List<string>
            {
                "John",
                "Jacob",
                "Zachary",
                "Jonny",
                "Louis",
                "Bob"
            }
            }
        };
    }
}
