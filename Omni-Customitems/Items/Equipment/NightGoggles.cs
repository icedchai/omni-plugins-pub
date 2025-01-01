using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using UnityEngine;
using PlayerHandler = Exiled.Events.Handlers.Player;
using Player = Exiled.API.Features.Player;
using MEC;
using Light = Exiled.API.Features.Toys.Light;
using Omni_Customitems;

namespace Omni_CustomItems.Items
{
    [CustomItem(ItemType.SCP1344)]
    public class NightGoggles : GogglesItem
    {
        private static Dictionary<int, Light> _playerLights = new Dictionary<int, Light>();
        public override uint Id { get; set; } = CustomItemsPlugin.pluginInstance.Config.IdPrefix + 13;
        public override string Name { get; set; } = "Night Vision Goggles";
        public override string Description { get; set; }
        public override float Weight { get; set; }
        public override SpawnProperties SpawnProperties { get; set; }
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            PlayerHandler.Verified += OnConnected;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            PlayerHandler.Verified -= OnConnected;
        }

        protected void OnConnected(VerifiedEventArgs e)
        {
            foreach (Light light in _playerLights.Values)
            {
                e.Player.ODestroyNetworkIdentity(light.AdminToyBase.netIdentity);
            }
        }
        protected override void RemoveGoggles(Player player, bool showMessage = true)
        {
            base.RemoveGoggles(player,showMessage);
            if(_playerLights.TryGetValue(player.Id, out var lights))
            {
                lights.Destroy();
                _playerLights.Remove(player.Id);
            }
        }
        protected override void EquipGoggles(Player player, bool showMessage = true)
        {
            base.EquipGoggles(player,showMessage);

            Light light = Light.Create(Vector3.zero,Vector3.zero,Vector3.one,true,new Color(0.5f,1,0.5f));
            light.Intensity = 100;
            light.Range = 100;
            light.ShadowStrength = 0;
            light.AdminToyBase.NetworkMovementSmoothing = 60;
            _playerLights.Add(player.Id, light);

            foreach (Player processingPlayer in Player.List)
            {
                if (processingPlayer != player)
                {
                        
                    processingPlayer.ODestroyNetworkIdentity(light.AdminToyBase.netIdentity);

                }
            }
            Timing.CallDelayed(1.5f, () =>
            {
                light.Base.transform.parent = player.Transform;
                light.Position = player.Position;
            });
        }
    }
}
