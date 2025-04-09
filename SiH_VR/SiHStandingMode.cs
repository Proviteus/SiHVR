using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRGIN.Controls;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Modes;

namespace SiHVR
{
    public class SiHStandingMode : StandingMode
    {
        private bool _hasInitialized;

        protected override void OnStart()
        {
            base.OnStart();
            VRPlugin.Logger.LogInfo("[SiHStandingMode] Entered Standing Mode");

            // If you want to position the camera above player height
            var camera = VR.Camera.GetComponent<Camera>();
            if (camera != null)
            {
                VR.Camera.transform.position = new Vector3(0f, 1.6f, 0f); // ~6ft eye level
                VR.Camera.transform.rotation = Quaternion.identity;
                VRPlugin.Logger.LogInfo("[SiHStandingMode] Positioned VR camera to standing height");
            }

            _hasInitialized = true;
        }

        protected override void OnUpdate()
        {
            // Prevent null references in base VRGIN logic
            if (!_hasInitialized || VR.Camera == null) return;

            // Skip camera syncing if you don’t have tracked controllers
            // Optionally: You can implement head tracking logic here later
        }

        protected override IEnumerable<IShortcut> CreateShortcuts()
        {
            return base.CreateShortcuts().Concat(new IShortcut[]
            {
                new MultiKeyboardShortcut(
                    new KeyStroke("Ctrl+C"),
                    new KeyStroke("Ctrl+C"),
                    () => VR.Manager.SetMode<SiHSeatedMode>()
                )
            });
        }

        protected override void CreateControllers()
        {
            // Skip controller creation entirely
            // You can later hook this to OpenXR inputs if needed
        }

        protected override void ChangeModeOnControllersDetected()
        {
            // No-op: Do not auto-switch modes based on controllers
        }
    }
}
