using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni_Utils.Configs
{
    public class RolenameConfig
    {
        [Description("Whether this feature set is enabled")]
        public bool IsEnabled { get; set; } = true;
        public Dictionary<RoleTypeId, string> RoleRoleNames { get; set; } = new Dictionary<RoleTypeId, string> {

            { RoleTypeId.ClassD, "Class-D Personnel" },
            { RoleTypeId.Scientist, "Research Personnel" },
            { RoleTypeId.FacilityGuard, "FAC-SEC Personnel" },

            { RoleTypeId.NtfCaptain, "Mobile Task Force %rankhigh%" },
            { RoleTypeId.NtfSergeant, "Mobile Task Force %rankmid%" },
            { RoleTypeId.NtfPrivate, "Mobile Task Force %ranklow%" },

            { RoleTypeId.Tutorial, "Unknown Personnel" },

            { RoleTypeId.ChaosConscript, "Chaos Insurgency Conscript" },
            { RoleTypeId.ChaosMarauder, "Chaos Insurgency Marauder" },
            { RoleTypeId.ChaosRepressor, "Chaos Insurgency Repressor" },
            { RoleTypeId.ChaosRifleman, "Chaos Insurgency Rifleman" },
        };
    }
}
