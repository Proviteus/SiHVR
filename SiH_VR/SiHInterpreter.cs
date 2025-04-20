using UnityEngine;
using VRGIN.Core;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using Valve.VR;

namespace SiHVR.Interpreters
{
    // ゲーム特有の処理を実装する.
    public class SiHInterpreter : GameInterpreter
    {
        private HashSet<Camera> _CheckedCameras = new HashSet<Camera>();
        private List<Camera> _AdjustCamera = new List<Camera>();

        Quaternion RotAdjust = new Quaternion(0f, 0f, 0f, 0f);

        // GUIカメラの歪む問題補正.
        protected override void OnUpdate()
        {
            base.OnUpdate();

            // Find new GUI Camera.
            foreach (var camera in Camera.allCameras.Except(_CheckedCameras).ToList())
            {
                _CheckedCameras.Add(camera);
                if (VRGUI.Instance.IsInterested(camera))
                {
                    VRLog.Info("Detected GUI camera ( {0} ) Adjusting Start", camera.name);
                    _AdjustCamera.Add(camera);
                }
            }

            // Adjust Camera
            foreach (var camera in _AdjustCamera)
            {
                camera.transform.rotation = RotAdjust;
            }
        }

        protected void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _CheckedCameras.Clear();
            _AdjustCamera.Clear();

            // Find and destroy SteamVR_Render
            var render = GameObject.FindObjectOfType<SteamVR_Render>();
            if (render)
            {
                VRLog.Info("Destroying SteamVR_Render to prevent flickering.");
                GameObject.Destroy(render);
            }
        }

    }
}
