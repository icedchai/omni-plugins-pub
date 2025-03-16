using Exiled.API.Enums;
using Exiled.API.Interfaces;
using OmniCommonLibrary;
using PlayerRoles;
using Respawning;
using System.Collections.Generic;
using System.ComponentModel;

namespace Omni_Utils.Configs
{
    public class Config : IConfig
    {
        [Description("Indicates plugin enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("Indicates debug mode enabled or not")]
        public bool Debug { get; set; } = false;

        public int NtfVanillaChance { get; set; } = 100;
        public int CiVanillaChance { get; set; } = 100;
        [Description("All squad names must be different. Leave empty if you're using my other custom squad plugin. PLEASE NOTE: THESE SQUADS ARE UNUSABLE IN THEIR CURRENT STATE, AND ARE FOR DEMONSTRATION PURPOSES ONLY!")]
        public List<CustomSquad> CustomSquads { get; set; } = new List<CustomSquad>
        {
            new CustomSquad()
            {
                UseCassieAnnouncement=true,
                SquadName= "delta4",
                SquadType=SpawnableFaction.NtfWave,
                SpawnChance = 0,
                UseTeamVehicle = true,
                EntranceAnnouncement = $"MTFUnit nato_d 4 designated Minute Men division %division% hasentered AllRemaining",
                EntranceAnnouncementSubs = $"Mobile Task Force Unit Delta-4 designated 'Minutemen', division %division% has entered the facility.<split>All remaining personnel are advised to proceed with standard evacuation protocols until an MTF squad reaches your destination.",
                CustomRoles = new Dictionary<char, OverallRoleType>
                {
                    {'c', new OverallRoleType{RoleId=12,RoleType=RoleVersion.BaseGameRole} },
                    {'s', new OverallRoleType{RoleId=11,RoleType=RoleVersion.BaseGameRole} },
                    {'p', new OverallRoleType{RoleId=13,RoleType=RoleVersion.BaseGameRole} }
                }
                ,
                SpawnQueue="csssspsspspsppspppsspspppp"
            },
            new CustomSquad()
            {
                UseCassieAnnouncement = true,
                SquadName = "swat",
                SquadType = SpawnableFaction.ChaosWave,
                SpawnChance = 0,
                UseTeamVehicle = true,
                EntranceAnnouncement = $"the Secret jam_1_1 weapons and tactical team from an core jam_40_2 agent p d hasentered",
                EntranceAnnouncementSubs = $"The Special Weapons and Tactical team from Anchorage PD has entered the facility.",
                CustomRoles = new Dictionary<char, OverallRoleType>
                    {
                    {'a', new OverallRoleType{ RoleId=12,RoleType=RoleVersion.BaseGameRole}
                    },
                },
                SpawnQueue="aaaaaaaaaaaaaaaaaaaaaaaaa"
            },
        };

        [Description("Amount of stamina to consume when jumping. Set to 0 to disable (especially if other plugin already does this).")]
        public float StaminaUseOnJump { get; set; } = 30;
        [Description("Roleplay Height features include randomized height and a player-command to change it")]
        public bool UseRoleplayHeight { get; set; } = true;
        public float HeightMin { get; set; } = 0.9f;
        public float HeightMax { get; set; } = 1.1f;
        [Description("Nickname configs, good for roleplay purposes.")]
        public NicknameConfig NicknameConfig { get; set; } = new();
        [Description("Rolename (good for custom RP events) related config.")]
        public RolenameConfig RolenameConfig { get; set; } = new();
        [Description("Custom Termination Announcement config.")]
        public CustomTerminationAnnouncementConfig CustomTerminationAnnouncementConfig { get; set; } = new();
        

    }
    
}
