namespace Omni_Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Customs;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.Handlers;
    using MapEditorReborn.API.Features.Objects;
    using MEC;
    using Omni_Utils.EventHandlers;
    using Omni_Utils.Patches;
    
    using YamlDotNet.Serialization;
    using Config = Omni_Utils.Configs.Config;
    using Map = Exiled.Events.Handlers.Map;
    using MyPatcher = Omni_Utils.Patches.MyPatcher;
    using Player = Exiled.Events.Handlers.Player;
    using Server = Exiled.Events.Handlers.Server;

    /// <inheritdoc/>
    public class OmniUtilsPlugin : Plugin<Config>
    {
        /// <summary>
        /// The singleton.
        /// </summary>
        public static OmniUtilsPlugin PluginInstance;

        /// <inheritdoc/>
        public override string Name => "Omni-2 Roleplay Utilities (ABRIDGED)";

        /// <inheritdoc/>
        public override string Author => "icedchqi";

        /// <inheritdoc/>
        public override string Prefix => "omni-utils";

        /// <inheritdoc/>
        public override Version Version => new (2, 0, 0);

        /// <summary>
        /// The plugin event handler instance.
        /// </summary>
        private PluginEventHandler eventHandler;

        // I call these 'ranks' because they stay the same per-life, and all use the same data slot on a player.
        public static List<RankGroup> consistentReplacements = new List<RankGroup>();

        // These can be randomized per-life.
        public static List<RankGroup> inconsistentReplacements = new List<RankGroup>();

        public const string VanillaSquad = "vaniller";

        public override void OnEnabled()
        {
            PluginInstance = this;

            // RueIMain.EnsureInit();
            consistentReplacements = consistentReplacements.Concat(Config.NicknameConfig.RankGroups).ToList();
            inconsistentReplacements = inconsistentReplacements.Concat(Config.NicknameConfig.RandomReplacements).ToList();

            //Look under MyPatcher.cs in /Patches
            MyPatcher.DoPatching();
            RegisterEvents();

        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            PluginInstance = null;
        }

        private void RegisterEvents()
        {
            eventHandler = new PluginEventHandler();
            if (Config.StaminaUseOnJump>0)
            {
                Player.Jumping += eventHandler.OnPlayerJump;
            }

            Player.Dying += eventHandler.OnPlayerDeath;
            Player.ChangingNickname += eventHandler.OnChangingNickname;
            Player.ChangingRole += eventHandler.OnChangingRole;
        }

        private void UnregisterEvents()
        {

            if (Config.StaminaUseOnJump > 0)
            {
                Player.Jumping -= eventHandler.OnPlayerJump;
            }

            Player.Dying -= eventHandler.OnPlayerDeath;
            Player.ChangingNickname -= eventHandler.OnChangingNickname;
            Player.ChangingRole -= eventHandler.OnChangingRole;
            eventHandler = null;

        }
    }
}
