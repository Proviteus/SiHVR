using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRGIN.Controls;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Modes;

namespace SiHVR
{
    public class SiHSeatedMode : SeatedMode
    {
        protected override void OnStart()
        {
            base.OnStart();
            VRPlugin.Logger.LogInfo("[SiHSeatedMode] Started seated VR mode");

            // Force perspective camera
            if (Camera.main != null && Camera.main.orthographic)
            {
                Camera.main.orthographic = false;
                VRPlugin.Logger.LogInfo("[SiHSeatedMode] Forced Camera.main to perspective");
            }

            var vrCam = VR.Camera?.GetComponent<Camera>();
            if (vrCam != null && vrCam.orthographic)
            {
                vrCam.orthographic = false;
                VRPlugin.Logger.LogInfo("[SiHSeatedMode] Forced VR.Camera to perspective");
            }

            // Recenter the view at startup
            Recenter();
        }

        protected override IEnumerable<IShortcut> CreateShortcuts()
        {
            return base.CreateShortcuts().Concat(new IShortcut[]
            {
                new MultiKeyboardShortcut(
                    new KeyStroke("Ctrl+C"),
                    new KeyStroke("Ctrl+C"),
                    () => VR.Manager.SetMode<SiHStandingMode>()
                )
            });
        }

        protected override void CreateControllers()
        {
            // Skip creating controllers for seated mode
        }

    }
}
