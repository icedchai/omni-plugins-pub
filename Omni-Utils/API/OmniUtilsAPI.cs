
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using OmniCommonLibrary;
using UnityEngine;

namespace Omni_Utils.API
{
    public static class OmniUtilsAPI
    {
        public static void SetNextMtfWave(CustomSquad newsquad)
        {
            OmniUtilsPlugin.NextWaveMtf = newsquad;
        }
        public static CustomSquad GetNextMtfWave()
        {
            return OmniUtilsPlugin.NextWaveMtf;
        }
        public static void SetNextCiWave(CustomSquad newsquad)
        {
            OmniUtilsPlugin.NextWaveCi = newsquad;
        }
        public static CustomSquad GetNextCiWave()
        {
            return OmniUtilsPlugin.NextWaveCi;
        }
    }
}
