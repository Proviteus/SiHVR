using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using VRGIN.Core;

namespace SiHVR.Patches
{
    [HarmonyPatch(typeof(VRManager), "OnUpdate")]
    internal class Patch_VRManager_OnUpdate
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var getAllCameras = typeof(Camera).GetProperty("allCameras").GetGetMethod();
            var skipListMethod = typeof(Patch_VRManager_OnUpdate).GetMethod("FilterCameras", BindingFlags.Static | BindingFlags.NonPublic);

            for (int i = 0; i < codes.Count; i++)
            {
                // Look for Camera.allCameras
                if (codes[i].Calls(getAllCameras))
                {
                    // Replace it with a call to our filter
                    codes[i] = new CodeInstruction(OpCodes.Call, skipListMethod);
                }
            }

            return codes;
        }

        private static Camera[] FilterCameras()
        {
            return Camera.allCameras
                .Where(cam =>
                    cam != null &&
                    cam.enabled &&
                    cam != VR.Camera.GetComponent<Camera>() &&
                    !VRGUI.Instance.IsInterested(cam) &&
                    !cam.name.Contains("Camera_Main") &&
                    !cam.name.Contains("Camera_HDR") &&
                    !cam.name.Contains("GraphicsLayerCamera") &&
                    !cam.name.Contains("Camera_xyz"))
                .ToArray();
        }
    }
}
