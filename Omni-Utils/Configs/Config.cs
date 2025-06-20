﻿namespace Omni_Utils.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using ColdWaterLibrary.Features.Roles;
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;
    using PlayerRoles;
    using Respawning;

    public class Config : IConfig
    {
        [Description("Indicates plugin enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("Indicates debug mode enabled or not")]
        public bool Debug { get; set; } = false;

        public bool DisableMiniWaves { get; set; } = false;

        public bool DisableRadioDrain { get; set; } = true;

        [Description("Amount of stamina to consume when jumping. Set to 0 to disable (especially if other plugin already does this).")]
        public float StaminaUseOnJump { get; set; } = 30;
        [Description("Roleplay Height features include randomized height")]
        public bool UseRandomizedHeight { get; set; } = true;
        public float HeightMin { get; set; } = 0.9f;
        public float HeightMax { get; set; } = 1.1f;
        [Description("Allows .height command")]
        public bool AllowHeightChange { get; set; } = true;
        public float HeightChangeMin { get; set; } = 0.9f;
        public float HeightChangeMax { get; set; } = 1.1f;
        [Description("Nickname configs, good for roleplay purposes.")]
        public NicknameConfig NicknameConfig { get; set; } = new();
        [Description("Rolename (good for custom RP events) related config.")]
        public RolenameConfig RolenameConfig { get; set; } = new();

    }
}
