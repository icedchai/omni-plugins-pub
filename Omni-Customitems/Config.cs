using Exiled.API.Interfaces;
using Omni_CustomItems.Items;
using System.ComponentModel;

namespace Omni_CustomItems
{
    public class Config : IConfig
    {
        [Description("Is plugin enabled or not?")]
        public bool IsEnabled { get; set; } = true;
        [Description("Is plugin in debug mode?")]
        public bool Debug { get; set; } = true;
        [Description("Delay before items are registered.")]
        public float RegistryDelay { get; set; } = 6f;
        public NightGoggles NvgGoggleSettings { get; set; } = new();
        public ScrambleGoggles ScrambleSettings { get; set; } = new();
    }
}