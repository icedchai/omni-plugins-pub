using Exiled.API.Features;
using Exiled.CustomItems.API.EventArgs;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using PlayerHandler = Exiled.Events.Handlers.Player;

namespace Omni_CustomItems.Items
{
    //GogglesItem.cs by icedchqi
    //November 19th 2024

    //Notes: This doesn't necessarily HAVE to be used for goggles, it's more generally defined as anything that goes
    //on your head and has a function. Or it doesn't, it can just be for cosmetic purposes.
    //The reason I made this is so that players can't equip more than one type of goggle at a time.
    public abstract class GogglesItem : CustomItem
    {
        public static Dictionary<int, GogglesItem> equippedGoggles = new Dictionary<int, GogglesItem>();
        protected bool PlayerHasGoggles(Player player)
        {
            if(equippedGoggles.TryGetValue(player.Id, out GogglesItem gogglesItem))
            {
                return this == gogglesItem;
            }
            return false;
        }
        protected virtual void RemoveGoggles(Player player, bool showMessage = true)
        {
            if (!equippedGoggles.TryGetValue(player.Id, out GogglesItem item)) return;
            if (item != this) return;
            equippedGoggles.Remove(player.Id);
            if(showMessage)
            {
                Player.Get(player.Id).ShowHint($"You remove the {Name}");
            }
            
        }
        protected virtual void EquipGoggles(Player player, bool showMessage = true)
        {
            if (equippedGoggles.TryGetValue(player.Id, out GogglesItem item)) return;
            equippedGoggles.Add(player.Id, this);


            if (showMessage)
            {
                Player.Get(player.Id).ShowHint($"You put on the {Name}");
            }
        }
        protected override void OnOwnerChangingRole(OwnerChangingRoleEventArgs e)
        {
            base.OnOwnerChangingRole(e);
            RemoveGoggles(e.Player, false);
        }
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            PlayerHandler.UsingItemCompleted += OnUsingCompleted;
            PlayerHandler.UsingItem += OnUsing;
        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            PlayerHandler.UsingItem -= OnUsing;
            PlayerHandler.UsingItemCompleted -= OnUsingCompleted;
        }
        protected override void OnWaitingForPlayers()
        {
            base.OnWaitingForPlayers();
            equippedGoggles.Clear();
        }
        protected override void OnOwnerDying(OwnerDyingEventArgs e)
        {
            base.OnOwnerDying(e);
            RemoveGoggles(e.Player,false);
        }

        protected override void OnDropping(DroppingItemEventArgs e)
        {
            base.OnDropping(e);
            if (Check(e.Item))
            {
                equippedGoggles.TryGetValue(e.Player.Id, out GogglesItem item);
                if(item != this)
                {
                    return;
                }
                e.IsAllowed = false;
                RemoveGoggles(e.Player);
            }
        }
        protected void OnUsing(UsingItemEventArgs e)
        {
            if (!Check(e.Item)) return;
            if (equippedGoggles.ContainsKey(e.Player.Id))
            {
                e.IsAllowed = false;
                e.Player.ShowHint("You are already wearing something!");
            }
        }
        protected void OnUsingCompleted(UsingItemCompletedEventArgs e)
        {
            if (!Check(e.Item)) return;
            e.IsAllowed = false;
            Timing.CallDelayed(0.01f, () => { e.Player.CurrentItem = null; });
            if (!equippedGoggles.ContainsKey(e.Player.Id))
            {
                EquipGoggles(e.Player);
            }
        }
    }
}
