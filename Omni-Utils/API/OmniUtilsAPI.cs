
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using OmniCommonLibrary;
using UnityEngine;

namespace Omni_Utils.API
{
    public static class OmniUtilsAPI
    {
    
        public static string MakeUnitNameReadable(string unit)
        {
            string output = string.Empty;
            string[] thing = unit.Split('-'); //thing = ["HOTEL", "09"]
            output += $"nato_{unit[0]} {thing[1]}"; //output = "nato_H 09"
            return output;
        }
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
