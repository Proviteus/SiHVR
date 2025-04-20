using HarmonyLib;
using Valve.VR;
using VRGIN.Core;

namespace SiHVR.Patches
{
    [HarmonyPatch(typeof(SteamVR_Render), "OnEnable")]
    internal static class Patch_SteamVR_Render_OnEnable
    {
        static bool Prefix()
        {
            VRLog.Warn("SteamVR_Render.OnEnable() skipped by SiHVR to prevent flickering.");
            return false; // Skip original method
        }
    }
}
