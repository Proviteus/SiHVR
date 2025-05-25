using System;
using System.Collections;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SiHVR.Interpreters;
using Unity.XR.OpenVR;
using UnityEngine;
using Valve.VR;
using VRGIN.Core;
using VRGIN.Helpers;

namespace SiHVR
{
    [BepInPlugin(GUID, Name, Version)]
    public class VRPlugin : BaseUnityPlugin
    {
        public const string GUID = "SiH.VR";
        public const string Name = "SiH VR Plugin";
        public const string Version = "0.1.0";

        internal static new ManualLogSource Logger;

        private void Awake()
        {
            Logger = base.Logger;

            PatchLoader.ApplyPatches();
            Logger.LogInfo("Harmony patches applied.");

            if (Environment.CommandLine.Contains("--vr"))
            {
                Logger.LogInfo("VR Mode detected, initializing...");
                StartCoroutine(InitializeVR());
            }
            else
            {
                Logger.LogInfo("VR Mode not enabled.");
            }
        }

        private IEnumerator InitializeVR()
        {
            Logger.LogInfo("Waiting for scene to load...");
            yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().isLoaded);

            Logger.LogInfo("Initializing OpenVR...");
            var ovrSettings = OpenVRSettings.GetSettings(true);
            ovrSettings.StereoRenderingMode = OpenVRSettings.StereoRenderingModes.MultiPass;
            ovrSettings.InitializationType = OpenVRSettings.InitializationTypes.Scene;
            ovrSettings.EditorAppKey = "summerinheat.vr";

            SteamVR_Settings.instance.autoEnableVR = true;
            SteamVR_Settings.instance.editorAppKey = "summerinheat.vr";

            var openVRLoader = ScriptableObject.CreateInstance<OpenVRLoader>();
            if (!openVRLoader.Initialize() || !openVRLoader.Start())
            {
                Logger.LogError("Failed to initialize or start OpenVR.");
                yield break;
            }

            try
            {
                SteamVR_Behaviour.Initialize(false);
            }
            catch (Exception ex)
            {
                Logger.LogError("SteamVR initialization failed:");
                Logger.LogError(ex);
                yield break;
            }

            Logger.LogInfo("OpenVR and SteamVR initialized!");

            VRManager.Create<SiHInterpreter>(new SiHContext());
            VR.Manager.SetMode<SiHSeatedMode>();

            Logger.LogInfo("VRManager initialized and SeatedMode set.");
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern void DisableProcessWindowsGhosting();
        }
    }
}
