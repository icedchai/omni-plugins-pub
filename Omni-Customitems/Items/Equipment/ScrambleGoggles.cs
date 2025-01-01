using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.EventArgs;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.Handlers;
using InventorySystem.Items.Usables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerHandler = Exiled.Events.Handlers.Player;

namespace Omni_CustomItems.Items
{
    [CustomItem(ItemType.SCP1344)]
    public class ScrambleGoggles : GogglesItem
    {
        public override uint Id { get; set; } = CustomItemsPlugin.pluginInstance.Config.IdPrefix + 12;
        public override string Name { get; set; } = "SCRAMBLE Goggles";
        public override string Description { get; set; }
        public override float Weight { get; set; }
        public override SpawnProperties SpawnProperties { get; set; }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            Scp096.AddingTarget += OnTriggering096;
        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            Scp096.AddingTarget -= OnTriggering096;
        }
        protected void OnTriggering096(AddingTargetEventArgs e)
        {
            if (!e.IsLooking) return;
            if (PlayerHasGoggles(e.Target))
            {
                e.IsAllowed = false;
            }
        }
    }
}
