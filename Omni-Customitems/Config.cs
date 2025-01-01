using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Omni_CustomItems
{
    public class Config : IConfig
    {
        [Description("Is plugin enabled or not?")]
        public bool IsEnabled { get; set; } = true;
        [Description("Is plugin in debug mode?")]
        public bool Debug { get; set; } = true;
        [Description("Custom Item ID prefix")]
        public uint IdPrefix { get; set; } = 1100;
        [Description("Delay before items are registered.")]
        public float RegistryDelay { get; set; } = 6f;
    }
}