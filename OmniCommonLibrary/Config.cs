using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using OmniCommonLibrary;
using PlayerRoles;

namespace OmniCommonLibrary
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

    }
}