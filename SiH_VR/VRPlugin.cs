using System;
using System.Collections;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SiHVR.Interpreters;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.VR;
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

            bool enabled = Environment.CommandLine.Contains("--vr");
            if (enabled)
            {
                Logger.LogInfo("VR mode enabled, starting initialization...");
                //BepInExVrLogBackend.ApplyYourself();
                StartCoroutine(InitializeOpenVR());
            }
            else
            {
                Logger.LogInfo("VR mode not enabled.");
            }
        }

        private IEnumerator InitializeOpenVR()
        {
            Logger.LogInfo("Waiting for scene manager to initialize...");
            yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().isLoaded);

            Logger.LogInfo("Initializing OpenVR runtime...");
            var ovrSettings = OpenVRSettings.GetSettings(true);
            ovrSettings.StereoRenderingMode = OpenVRSettings.StereoRenderingModes.MultiPass;
            ovrSettings.InitializationType = OpenVRSettings.InitializationTypes.Scene;
            ovrSettings.EditorAppKey = "summerinheat.vr";

            SteamVR_Settings.instance.autoEnableVR = true;
            SteamVR_Settings.instance.editorAppKey = "summerinheat.vr";

            var openVRLoader = ScriptableObject.CreateInstance<OpenVRLoader>();
            if (!openVRLoader.Initialize())
            {
                Logger.LogError("Failed to initialize OpenVR.");
                yield break;
            }

            if (!openVRLoader.Start())
            {
                Logger.LogError("Failed to start OpenVR.");
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

            Logger.LogInfo("OpenVR and SteamVR initialized successfully!");

            Logger.LogInfo("Initializing VRGIN...");
            new Harmony(GUID).PatchAll(typeof(VRPlugin).Assembly);

            VRManager.Create<SiHInterpreter>(new SiHContext());

            // Optional: NearClip/IPD bindings here
            // VR.Settings.AddListener("IPDScale", (_, __) => ... );

            VR.Manager.SetMode<SiHSeatedMode>();

            // Optional: Create fade
            //VRFade.Create();

            // Optional: Extra safety
            NativeMethods.DisableProcessWindowsGhosting();

            Logger.LogInfo("SiHVR loaded and ready.");
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern void DisableProcessWindowsGhosting();
        }
    }
}
