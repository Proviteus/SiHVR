using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRGIN.Core;

namespace SiHVR.Interpreters
{
    public class SiHInterpreter : GameInterpreter
    {
        private readonly List<Camera> _adjustCameras = new();
        private readonly Quaternion _uiRotationFix = Quaternion.identity;

        public bool CanInterpret() => false;

        protected override void OnStart()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            VRLog.Info("[SiHInterpreter] Interpreter started.");

            var vrCam = VR.Camera?.GetComponent<Camera>();
            if (vrCam != null)
            {
                VRLog.Info($"[SiHInterpreter] VRGIN camera is '{vrCam.name}'");
            }
            else
            {
                VRLog.Warn("[SiHInterpreter] VRGIN camera is null or missing Camera component!");
            }

            // Do NOT manually copy any cameras here.
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            VRLog.Info($"[SiHInterpreter] Scene loaded: {scene.name}");
            _adjustCameras.Clear();
        }

        protected override void OnUpdate()
        {
            foreach (var cam in Camera.allCameras)
            {
                if (cam == null) continue;

                string source = "Unknown";
                if (cam == VR.Camera.GetComponent<Camera>())
                    source = "VRGIN Camera";
                else if (VRGUI.Instance.IsInterested(cam))
                    source = "GUI Camera";
                else if (cam.name.Contains("Camera_Main"))
                    source = "Main Camera";
                else if (cam.name.Contains("HDR") || cam.name.Contains("xyz"))
                    source = "Effect/Overlay Camera";

                VRPlugin.Logger.LogInfo($"[CameraDebug] {cam.name} - enabled: {cam.enabled}, active: {cam.gameObject.activeInHierarchy}, tag: {cam.tag}, source: {source}");
            }
        }


        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            VRLog.Info("[SiHInterpreter] Interpreter disabled.");
        }
    }
}
