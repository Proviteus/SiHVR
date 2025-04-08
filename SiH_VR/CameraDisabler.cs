using UnityEngine;
using VRGIN.Core;

namespace SiHVR
{
    internal class CameraDisabler : ProtectedBehaviour
    {
        private Camera _camera;

        protected override void OnStart()
        {
            _camera = GetComponent<Camera>();

            if (_camera != null)
            {
                VRPlugin.Logger.LogInfo($"[CameraDisabler] Disabling camera '{_camera.name}' to prevent flicker.");
                _camera.enabled = false;
            }
        }

        protected override void OnUpdate()
        {
            // Ensure it stays disabled (some games re-enable cameras per frame)
            if (_camera && _camera.enabled)
            {
                _camera.enabled = false;
                VRPlugin.Logger.LogInfo($"[CameraDisabler] Re-disabled camera '{_camera.name}'");
            }
        }
    }
}
