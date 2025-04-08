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

            if (Camera.main != null)
            {
                Camera.main.orthographic = false;
                VRPlugin.Logger.LogInfo("Forced Camera.main.orthographic = false");
            }

            if (VR.Camera != null)
            {
                var cam = VR.Camera.GetComponent<Camera>();
                if (cam != null)
                {
                    cam.orthographic = false;
                    VRPlugin.Logger.LogInfo("Forced VR.Camera.GetComponent<Camera>().orthographic = false");
                }
            }
        }

        protected override IEnumerable<IShortcut> CreateShortcuts()
        {
            // Optional debug shortcut: Ctrl+C twice to switch to standing mode
            return base.CreateShortcuts().Concat(new IShortcut[]
            {
                new MultiKeyboardShortcut(
                    new KeyStroke("Ctrl+C"),
                    new KeyStroke("Ctrl+C"),
                    () => VR.Manager.SetMode<SiHStandingMode>()
                )
            });
        }

        /// <summary>
        /// Prevents VRGIN from creating controller objects.
        /// This avoids errors in OpenXR when no controllers are connected.
        /// </summary>
        protected override void CreateControllers()
        {
            // Intentionally left blank to disable controller creation
        }

        // Optional: uncomment to auto-switch to standing if controllers are detected
        // protected override void ChangeModeOnControllersDetected()
        // {
        //     VR.Manager.SetMode<SiHStandingMode>();
        // }
    }
}
