using Exiled.API.Features;
using Exiled.Events.Handlers;
using MapEditorReborn.API.Features.Objects;
using MEC;
using Omni_Utils.EventHandlers;
using OmniCommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using RueI;
using YamlDotNet.Serialization;
using Map = Exiled.Events.Handlers.Map;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using Omni_Utils.Patches;
using MyPatcher = Omni_Utils.Patches.MyPatcher;
using Config = Omni_Utils.Configs.Config;
using System.Linq;
using Customs;

namespace Omni_Utils
{
    public class OmniUtilsPlugin : Plugin<Config>
    {
        public static OmniUtilsPlugin pluginInstance;

        public override string Name => "Omni-2 Roleplay Utilities";

        public override string Author => "icedchqi";

        public override string Prefix => "omni-utils";

        public override Version Version => new(1,0,2);



        #region customSquad stuff
        //squadsToIndex is used to go from the squadname to the index in Config.customSquads, to 
        //allow accessing other properties of the squad from just the name.
        public static Dictionary<string, int> squadNameToIndex = new Dictionary<string, int>();
        public static List<string> SquadStrings = new List<string>();
        public static CustomSquad NextWaveMtf = null;
        public static CustomSquad NextWaveCi = null;
        public static CustomSquad TryGetCustomSquad(string squadName)
        {
            try
            {
                return pluginInstance.Config.CustomSquads[OmniUtilsPlugin.squadNameToIndex[squadName]];
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
                return pluginInstance.Config.CustomSquads[squadIndex];
            }
            catch (Exception ex)
            {
                Log.Info(ex);
                return null;
            }
        }
        #endregion



        PluginEventHandler _eventHandler;
        CustomSquadEventHandlers _squadEventHandler;
        
        public override void OnEnabled()
        {
            pluginInstance = this;
            
            RueIMain.EnsureInit();

            for (int i = 0; i <= Config.CustomSquads.Count - 1; i++)
            {
                CustomSquad squad = Config.CustomSquads[i];
                squadNameToIndex.Add(squad.SquadName.ToLower(), i);
                Log.Info($"{squad.SquadName} registered under id {i}");
            }
            OmniCommonLibrary.OmniCommonLibrary.consistentReplacements = OmniCommonLibrary.OmniCommonLibrary.consistentReplacements.Concat(Config.NicknameConfig.RankGroups).ToList();
            OmniCommonLibrary.OmniCommonLibrary.inconsistentReplacements = OmniCommonLibrary.OmniCommonLibrary.inconsistentReplacements.Concat(Config.NicknameConfig.RandomReplacements).ToList();

            //Look under MyPatcher.cs in /Patches
            MyPatcher.DoPatching();
            RegisterEvents();

        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            pluginInstance = null;
        }
        private void RegisterEvents()
        {
            _squadEventHandler = new CustomSquadEventHandlers();
            _eventHandler = new PluginEventHandler();
            if (Config.StaminaUseOnJump>0)
            {
                Player.Jumping += _eventHandler.OnPlayerJump;
            }
            
            Player.Dying += _eventHandler.OnPlayerDeath;
            Player.ChangingNickname += _eventHandler.OnChangingNickname;
            Player.ChangingRole += _eventHandler.OnChangingRole;

            
            Map.AnnouncingScpTermination += _eventHandler.OnAnnouncingScpTermination;
            
            Map.SpawningTeamVehicle += _squadEventHandler.OnTeamVehicleSpawning;
            Map.AnnouncingNtfEntrance += _squadEventHandler.OnNtfAnnouncing;
            Map.AnnouncingChaosEntrance += _squadEventHandler.OnChaosAnnouncing;
            Server.RespawningTeam += _squadEventHandler.OnSpawnWave;
        }

        private void UnregisterEvents()
        {

            if (Config.StaminaUseOnJump>0)
            {
                Player.Jumping -= _eventHandler.OnPlayerJump;
            }
            Player.Dying -= _eventHandler.OnPlayerDeath;
            Player.ChangingNickname -= _eventHandler.OnChangingNickname;
            Player.ChangingRole -= _eventHandler.OnChangingRole;


            Map.AnnouncingScpTermination -= _eventHandler.OnAnnouncingScpTermination;
            
            Map.SpawningTeamVehicle -= _squadEventHandler.OnTeamVehicleSpawning;
            Map.AnnouncingNtfEntrance -= _squadEventHandler.OnNtfAnnouncing;
            Map.AnnouncingChaosEntrance -= _squadEventHandler.OnChaosAnnouncing;
            Server.RespawningTeam -= _squadEventHandler.OnSpawnWave;
            _eventHandler = null;

        }
    }
}
