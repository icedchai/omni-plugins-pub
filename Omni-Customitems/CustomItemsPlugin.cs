using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using Exiled.CustomRoles.API.Features;

namespace Omni_CustomItems
{
    public class CustomItemsPlugin : Plugin<Config>
    {
        public static CustomItemsPlugin pluginInstance;

        public override string Name => "Omni-2 Custom Items";

        public override string Author => "icedchqi";

        public override string Prefix => "omni-customitems";

        public override Version Version => new(1,0,0);

        public override void OnEnabled()
        {
            base.OnEnabled();
            pluginInstance = this;
            


            Timing.CallDelayed(pluginInstance.Config.RegistryDelay, () => { 
                CustomItem.RegisterItems();
            });
            RegisterEvents();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            CustomItem.UnregisterItems();
            UnregisterEvents();
        }
        public void RegisterEvents()
        {

        }
        public void UnregisterEvents()
        {

        }
    }
}
