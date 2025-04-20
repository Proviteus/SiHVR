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
            // Force perspective camera
                new MultiKeyboardShortcut(new KeyStroke("Ctrl+C"), new KeyStroke("Ctrl+C"), () => { VR.Manager.SetMode<SiHStandingMode>(); })
            });
        }
                VRPlugin.Logger.LogInfo("[SiHSeatedMode] Forced Camera.main to perspective");
        /// <summary>
        /// Disables controllers for seated mode.
        /// </summary>
        protected override void CreateControllers()
        {
        }

        protected override void OnUpdate()

            if (VR.Camera?.SteamCam?.head == null)

            return base.CreateShortcuts().Concat(new IShortcut[]

            return base.CreateShortcuts().Concat(new IShortcut[]

            base.OnUpdate(); // Run ControlMode.OnUpdate()
        {
            // Skip creating controllers for seated mode

        /// <summary>
        /// Uncomment to automatically switch into Standing Mode when controllers have been detected.
        /// </summary>
        //protected override void ChangeModeOnControllersDetected()
        //{
        //    VR.Manager.SetMode<GenericStandingMode>();
        //}
        {
            // Skip creating controllers for seated mode
        {
            // Skip creating controllers for seated mode
        {
            // Skip creating controllers for seated mode
        }

    }
}
