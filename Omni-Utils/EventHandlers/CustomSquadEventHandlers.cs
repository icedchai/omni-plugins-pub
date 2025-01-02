using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Cassie;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using MEC;
using OmniCommonLibrary;
using PlayerRoles;
using Respawning.NamingRules;

namespace Omni_Utils.EventHandlers
{
    public class CustomSquadEventHandlers
    {
        public void OnTeamVehicleSpawning(SpawningTeamVehicleEventArgs e)
        {
            if (OmniUtilsPlugin.NextWaveCi != null)
            {
                e.IsAllowed = OmniUtilsPlugin.NextWaveCi.UseTeamVehicle;
            }
            if (OmniUtilsPlugin.NextWaveMtf != null)
            {
                e.IsAllowed = OmniUtilsPlugin.NextWaveMtf.UseTeamVehicle;
            }
        }
        public void OnChaosAnnouncing(AnnouncingChaosEntranceEventArgs e)
        {
            Log.Debug("Announcing CHAOS ENTRANCE");
            if (OmniUtilsPlugin.NextWaveCi is null)
            {
                return;
            }

            if (!OmniUtilsPlugin.NextWaveCi.UseCassieAnnouncement)
            {
                e.IsAllowed = false;
                OmniUtilsPlugin.NextWaveCi = null;
                return;
            }
            Log.Debug("Announcing CHAOS ENTRANCE: Custom Squad Detected");
            CustomSquad customSquad = OmniUtilsPlugin.NextWaveCi;
            e.IsAllowed = false;

        }
        public void OnNtfAnnouncing(AnnouncingNtfEntranceEventArgs e)
        {
            Log.Debug("Announcing NTF ENTRANCE");
            if (OmniUtilsPlugin.NextWaveMtf is null)
            {
                return;
            }

            if (!OmniUtilsPlugin.NextWaveMtf.UseCassieAnnouncement)
            {
                e.IsAllowed = false;
                OmniUtilsPlugin.NextWaveMtf = null;
                return;

            }
            Log.Debug("Announcing NTF ENTRANCE: Custom Squad Detected");
            CustomSquad customSquad = OmniUtilsPlugin.NextWaveMtf;
            e.IsAllowed = false;

        }
        public void OnSpawnWave(RespawningTeamEventArgs e)
        {
            CustomSquad customSquad;
            List<Player> players = e.Players;
            Queue<RoleTypeId> queue = e.SpawnQueue;
            if (players.Count == 0)
            {
                return;
            }
            switch (e.NextKnownTeam)
            {
                case Faction.FoundationEnemy:
                    {
                        customSquad = OmniUtilsPlugin.NextWaveCi;
                        if (customSquad is null)
                        {
                            return;
                        }
                        e.IsAllowed = false;


                        OmniUtilsPlugin.NextWaveCi = null;
                        foreach (char c in customSquad.SpawnQueue)
                        {
                            if (e.Players.IsEmpty())
                            {
                                Log.Info($"Finished spawning {customSquad.SquadName}");
                                break;
                            }
                            OverallRoleType roleType;
                            if (!customSquad.CustomRoles.TryGetValue(c, out roleType))
                            {
                                Log.Info($"Couldn't find the specified role of Key {c} in {customSquad.SquadName}'s roles.");
                                break;
                            }

                            Player player = e.Players.RandomItem();
                            player.SetOverallRole(roleType);
                            e.Players.Remove(player);
                            Log.Info($"Spawned {player} for {customSquad.SquadName}");
                        }
                        if(customSquad.UseCassieAnnouncement)
                        {
                            string announcement = customSquad.EntranceAnnouncement;
                            string announcementSubs = customSquad.EntranceAnnouncementSubs;

                            Cassie.MessageTranslated(announcement, announcementSubs);
                        }
                        break;
                    }
                case Faction.FoundationStaff:
                    {
                        customSquad = OmniUtilsPlugin.NextWaveMtf;
                        NamingRulesManager.TryGetNamingRule(Team.FoundationForces, out UnitNamingRule rule);
                        
                        if (customSquad is null)
                        {
                            return;
                        }
                        foreach (char c in customSquad.SpawnQueue)
                        {
                            if (e.Players.IsEmpty())
                            {
                                Log.Info($"Finished spawning {customSquad.SquadName}");
                                break;
                            }
                            OverallRoleType roleType;
                            if (!customSquad.CustomRoles.TryGetValue(c, out roleType))
                            {
                                Log.Info($"Couldn't find the specified role of Key {c} in {customSquad.SquadName}'s roles.");
                                break;
                            }

                            Player player = e.Players.RandomItem();
                            player.SetOverallRole(roleType);
                            e.Players.Remove(player);
                            Log.Info($"Spawned {player} for {customSquad.SquadName}");
                        }
                        if (customSquad.UseCassieAnnouncement)
                        {
                            string announcement = customSquad.EntranceAnnouncement;
                            string announcementSubs = customSquad.EntranceAnnouncementSubs;

                            announcementSubs = announcementSubs.Replace("%division%", NamingRulesManager.GeneratedNames[Team.FoundationForces].LastOrDefault());

                            Cassie.MessageTranslated(announcement, announcementSubs);
                        }
                        break;
                    }
                default:
                    return;
            }
        }
        public string MakeUnitNameReadable(string unit)
        {
            string output = string.Empty;
            string[] thing = unit.Split('-');
            output += "nato_" + thing[0];
        }
    }
}