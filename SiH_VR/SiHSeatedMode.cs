using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRGIN.Controls;
using VRGIN.Controls.Tools;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Modes;

namespace SiHVR
{
    public class SiHSeatedMode : SeatedMode
    {
        protected override IEnumerable<IShortcut> CreateShortcuts()
        {
            return base.CreateShortcuts().Concat(new IShortcut[]
            {
                new MultiKeyboardShortcut(new KeyStroke("Ctrl+C"), new KeyStroke("Ctrl+C"), () => { VR.Manager.SetMode<SiHStandingMode>(); })
            });
        }

        /// <summary>
        /// Disables controllers for seated mode.
        /// </summary>
        protected override void CreateControllers()
        {
        }

        protected override void OnUpdate()
        {
            if (VR.Camera?.SteamCam?.head == null)
            {
                VRLog.Warn("Skipping SeatedMode.OnUpdate due to missing VR.Camera.SteamCam.head");
                return;
            }

            base.OnUpdate(); // Run ControlMode.OnUpdate()
        }

        /// <summary>
        /// Uncomment to automatically switch into Standing Mode when controllers have been detected.
        /// </summary>
        protected override void ChangeModeOnControllersDetected()
        {
            VR.Manager.SetMode<SiHStandingMode>();
        }
    }
}
