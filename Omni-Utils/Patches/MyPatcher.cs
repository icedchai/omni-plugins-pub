namespace Omni_Utils.Patches
{
    using System;
    using System.Collections.Generic;
    using CommandSystem;
    using CommandSystem.Commands.RemoteAdmin;
    using Exiled.API.Features;
    using HarmonyLib;
    
    using Utils;

    /// <summary>
    /// Patcher for Harmony.
    /// </summary>
    public class MyPatcher
    {
        // make sure DoPatching() is called at start either by
        // the mod loader or by your injector.

        /// <summary>
        /// Do patching.
        /// </summary>
        public static void DoPatching()
        {
            var harmony = new Harmony("me.icedchai.patch");
            harmony.PatchAll();
        }
    }
}
