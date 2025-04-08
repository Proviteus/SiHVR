using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRGIN.Core;

namespace SiHVR.Interpreters
{
    public class SiHInterpreter : GameInterpreter
    {

        private HashSet<Camera> _checkedCameras = new();
        private List<Camera> _adjustCameras = new();

        private readonly Quaternion _uiRotationFix = Quaternion.identity;

        public bool CanInterpret() => false;
        protected override void OnStart()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            VRLog.Info("[SiHInterpreter] Interpreter started.");

            if (VR.Camera != null && VR.Camera.GetComponent<Camera>() != null)
            {
                VRLog.Info($"[SiHInterpreter] VRGIN camera is '{VR.Camera.GetComponent<Camera>().name}'");
            }
            else
            {
                VRLog.Warn("[SiHInterpreter] VRGIN camera is null or missing Camera component!");
            }
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            VRLog.Info($"[SiHInterpreter] Scene loaded: {scene.name}");
            _checkedCameras.Clear();
            _adjustCameras.Clear();
        }

        protected override void OnUpdate()
        {
            foreach (var cam in Camera.allCameras.Except(_checkedCameras).ToList())
            {
                _checkedCameras.Add(cam);

                // Skip the VRGIN camera
                if (cam == VR.Camera.GetComponent<Camera>()) continue;

                // Skip any camera VRGUI is interested in (it's needed for VRGIN's UI)
                if (VRGUI.Instance.IsInterested(cam)) continue;

                // Already has disabler? Skip
                if (!cam.gameObject.GetComponent<CameraDisabler>())
                {
                    cam.gameObject.AddComponent<CameraDisabler>();
                }

                // Also do GUI rotation fix
                if (VRGUI.Instance.IsInterested(cam))
                {
                    VRLog.Info($"[SiHInterpreter] GUI Camera detected: {cam.name} — applying rotation fix");
                    _adjustCameras.Add(cam);
                }
            }


            foreach (var cam in _adjustCameras)
            {
                cam.transform.rotation = _uiRotationFix;
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            VRLog.Info("[SiHInterpreter] Interpreter disabled.");
        }
    }
}
