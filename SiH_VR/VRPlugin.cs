using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;
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
using System.IO;

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

            // Apply Harmony patches (auto-patch everything)
            PatchLoader.ApplyPatches();
            Logger.LogInfo("Harmony patches applied.");

            // Check for --vr argument
            if (Environment.CommandLine.Contains("--vr"))
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

            var cam = VR.Camera.GetComponent<Camera>();
            if (cam)
            {
                VRLog.Info($"[Sanity] VRGIN Camera is {cam.name}, stereo: {cam.stereoEnabled}, target eye: {cam.stereoTargetEye}");
            }
            Logger.LogInfo("VRManager created.");
            VR.Manager.SetMode<SiHSeatedMode>();
            if (!File.Exists(Path.Combine(Paths.ConfigPath, "VRSettings.xml")))
            {
                VR.Settings.Save();
                Logger.LogInfo("Generated new VRSettings.xml with default shortcuts.");
            }

            Logger.LogInfo("OpenVR and SteamVR initialized successfully!");

            Logger.LogInfo("Initializing VRGIN...");
            VRManager.Create<SiHInterpreter>(new SiHContext());
            Logger.LogInfo("VRManager created.");
            Logger.LogInfo($"VR Mode: {VR.Manager.Mode?.GetType().Name}");
            Logger.LogInfo($"VR Camera: {VR.Camera?.name}");
            Logger.LogInfo($"VR Camera Parent: {VR.Camera?.transform.parent?.name}");


            var cam = VR.Camera.GetComponent<Camera>();
            if (cam)
            {
                VRLog.Info($"[Sanity] VRGIN Camera is {cam.name}, stereo: {cam.stereoEnabled}, target eye: {cam.stereoTargetEye}");
            }

            VR.Manager.SetMode<SiHSeatedMode>();
            if (!File.Exists(Path.Combine(Paths.ConfigPath, "VRSettings.xml")))
            {
                VR.Settings.Save();
                Logger.LogInfo("Generated new VRSettings.xml with default shortcuts.");
            }


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
