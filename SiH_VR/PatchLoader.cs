using HarmonyLib;

namespace SiHVR
{
    internal static class PatchLoader
    {
        internal static void ApplyPatches()
        {
            var harmony = new Harmony(VRPlugin.GUID);
            harmony.PatchAll(typeof(PatchLoader).Assembly);
            VRPlugin.Logger.LogInfo("Harmony patches applied.");
        }
    }
}
