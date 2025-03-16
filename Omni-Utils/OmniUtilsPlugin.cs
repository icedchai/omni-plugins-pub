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
    using OmniCommonLibrary;
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
        public override string Name => "Omni-2 Roleplay Utilities";

        /// <inheritdoc/>
        public override string Author => "icedchqi";

        /// <inheritdoc/>
        public override string Prefix => "omni-utils";

        /// <inheritdoc/>
        public override Version Version => new(1,1,0);

        #region customSquad stuff

        // squadsToIndex is used to go from the squadname to the index in Config.customSquads, to 
        // allow accessing other properties of the squad from just the name.
        public static Dictionary<string, int> squadNameToIndex = new Dictionary<string, int>();
        public static List<string> SquadStrings = new List<string>();
        public static CustomSquad NextWaveMtf = null;
        public static CustomSquad NextWaveCi = null;

        /// <summary>
        /// Try to get a custom squad.
        /// </summary>
        /// <param name="squadName"></param>
        /// <returns></returns>
        public static CustomSquad TryGetCustomSquad(string squadName)
        {
            try
            {
                return PluginInstance.Config.CustomSquads[OmniUtilsPlugin.squadNameToIndex[squadName]];
            }
            catch (Exception ex)
            {
                Log.Info(ex);
                return null;
            }
        }
        public static CustomSquad TryGetCustomSquad(int squadIndex)
        {
            try
            {
                return PluginInstance.Config.CustomSquads[squadIndex];
            }
            catch (Exception ex)
            {
                Log.Info(ex);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// The plugin event handler instance.
        /// </summary>
        private PluginEventHandler eventHandler;

        /// <summary>
        /// The custom squad event handler instance.
        /// </summary>
        private CustomSquadEventHandlers squadEventHandler;

        public const string VanillaSquad = "vaniller";

        public override void OnEnabled()
        {
            PluginInstance = this;

            // RueIMain.EnsureInit();

            CustomSquad vanilla = new CustomSquad { SquadName = VanillaSquad , UseCassieAnnouncement = true };

            CustomSquadEventHandlers.CiPool.AddEntry(vanilla, Config.CiVanillaChance);

            CustomSquadEventHandlers.NtfPool.AddEntry(vanilla, Config.NtfVanillaChance);

            for (int i = 0; i <= Config.CustomSquads.Count - 1; i++)
            {
                CustomSquad squad = Config.CustomSquads[i];

                // Shouldn't cause any issues unless the user has typed an infinite amount of the same squad...
                while (squadNameToIndex.ContainsKey(squad.SquadName) || squad.SquadName == VanillaSquad)
                {
                    squad.SquadName += "_";
                }
                
                if (squad.SpawnChance > 0)
                {
                    switch (squad.SquadType)
                    {
                        case SpawnableFaction.NtfWave:
                            CustomSquadEventHandlers.NtfPool.AddEntry(squad, squad.SpawnChance);
                            break;
                        case SpawnableFaction.NtfMiniWave:
                            CustomSquadEventHandlers.NtfPool.AddEntry(squad, squad.SpawnChance);
                            break;
                        case SpawnableFaction.ChaosWave:
                            CustomSquadEventHandlers.CiPool.AddEntry(squad, squad.SpawnChance);
                            break;
                        case SpawnableFaction.ChaosMiniWave:
                            CustomSquadEventHandlers.CiPool.AddEntry(squad, squad.SpawnChance);
                            break;
                    }
                    Log.Debug($"Registered squad {squad.SquadName} with chance {squad.SpawnChance} under {squad.SquadType}");
                }
                squadNameToIndex.Add(squad.SquadName.ToLower(), i);
                Log.Info($"{squad.SquadName} registered under id {i}");
            }

            OmniCommonLibrary.consistentReplacements = OmniCommonLibrary.consistentReplacements.Concat(Config.NicknameConfig.RankGroups).ToList();
            OmniCommonLibrary.inconsistentReplacements = OmniCommonLibrary.inconsistentReplacements.Concat(Config.NicknameConfig.RandomReplacements).ToList();

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
            squadEventHandler = new CustomSquadEventHandlers();
            eventHandler = new PluginEventHandler();
            if (Config.StaminaUseOnJump>0)
            {
                Player.Jumping += eventHandler.OnPlayerJump;
            }
            
            Player.Dying += eventHandler.OnPlayerDeath;
            Player.ChangingNickname += eventHandler.OnChangingNickname;
            Player.ChangingRole += eventHandler.OnChangingRole;

            
            Map.AnnouncingScpTermination += eventHandler.OnAnnouncingScpTermination;
            
            Map.SpawningTeamVehicle += squadEventHandler.OnTeamVehicleSpawning;
            Map.AnnouncingNtfEntrance += squadEventHandler.OnNtfAnnouncing;
            Map.AnnouncingChaosEntrance += squadEventHandler.OnChaosAnnouncing;
            Server.RespawningTeam += squadEventHandler.OnSpawnWave;
        }

        private void UnregisterEvents()
        {

            if (Config.StaminaUseOnJump>0)
            {
                Player.Jumping -= eventHandler.OnPlayerJump;
            }
            Player.Dying -= eventHandler.OnPlayerDeath;
            Player.ChangingNickname -= eventHandler.OnChangingNickname;
            Player.ChangingRole -= eventHandler.OnChangingRole;


            Map.AnnouncingScpTermination -= eventHandler.OnAnnouncingScpTermination;
            
            Map.SpawningTeamVehicle -= squadEventHandler.OnTeamVehicleSpawning;
            Map.AnnouncingNtfEntrance -= squadEventHandler.OnNtfAnnouncing;
            Map.AnnouncingChaosEntrance -= squadEventHandler.OnChaosAnnouncing;
            Server.RespawningTeam -= squadEventHandler.OnSpawnWave;
            eventHandler = null;

        }
    }
}
