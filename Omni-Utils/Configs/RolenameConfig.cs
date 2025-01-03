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

            { RoleTypeId.NtfSpecialist, "Mobile Task Force Specialist" },
            { RoleTypeId.NtfCaptain, "Mobile Task Force Captain" },
            { RoleTypeId.NtfSergeant, "Mobile Task Force Sergeant" },
            { RoleTypeId.NtfPrivate, "Mobile Task Force Private" },

            { RoleTypeId.Tutorial, "Unknown Personnel" },

            { RoleTypeId.ChaosConscript, "Chaos Insurgency Conscript" },
            { RoleTypeId.ChaosMarauder, "Chaos Insurgency Marauder" },
            { RoleTypeId.ChaosRepressor, "Chaos Insurgency Repressor" },
            { RoleTypeId.ChaosRifleman, "Chaos Insurgency Rifleman" },


            { RoleTypeId.Scp079, "SCP-079" },
            { RoleTypeId.Scp096, "SCP-096" },
            { RoleTypeId.Scp939, "SCP-939" },
            { RoleTypeId.Scp049, "SCP-049" },
            { RoleTypeId.Scp173, "SCP-173" },
            { RoleTypeId.Scp0492, "SCP-049-2" },
        };
    }
}
