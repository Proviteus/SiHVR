using System;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using VRGIN.Controls.Speech;
using VRGIN.Core;
using VRGIN.Helpers;
using VRGIN.Visuals;

namespace SiHVR
{
    [XmlRoot("Context")]
    public class SiHContext : IVRManagerContext
    {
        DefaultMaterialPalette _materials;
        VRSettings _settings;

        public SiHContext()
        {
            // We'll keep those always the same
            _materials = new DefaultMaterialPalette();
            bool isNew = !System.IO.File.Exists("VRSettings.xml");
            _settings = VRSettings.Load<VRSettings>("VRSettings.xml");

            // IPDのデフォルトを10に設定(巨人化防止)
            if (isNew)
            {
                _settings.IPDScale = 1f;
                _settings.Save();
            }
            // Runtime defaults
            ConfineMouse = true;
            EnforceDefaultGUIMaterials = false;
            GUIAlternativeSortingMode = false;
            GuiLayer = "Default";
            GuiFarClipPlane = 1000f;
            GuiNearClipPlane = -1000f;
            IgnoreMask = 0;
            InvisibleLayer = "Ignore Raycast";
            PrimaryColor = Color.cyan;
            SimulateCursor = true;
            UILayer = "UI";
            UILayerMask = LayerMask.GetMask(UILayer);
            UnitToMeter = 1f;
            NearClipPlane = 0.01f;
            PreferredGUI = GUIType.uGUI;
        }


        [XmlIgnore] public IMaterialPalette Materials => _materials;
        [XmlIgnore] public VRSettings Settings => _settings;
        [XmlIgnore] public Type VoiceCommandType => typeof(VoiceCommand);

        public bool ConfineMouse { get; set; }
        public bool EnforceDefaultGUIMaterials { get; set; }
        public bool GUIAlternativeSortingMode { get; set; }
        public string GuiLayer { get; set; }
        public float GuiFarClipPlane { get; set; }
        public float GuiNearClipPlane { get; set; }
        public int IgnoreMask { get; set; }
        public string InvisibleLayer { get; set; }
        public Color PrimaryColor { get; set; }
        public bool SimulateCursor { get; set; }
        public string UILayer { get; set; }
        public int UILayerMask { get; set; }
        public float UnitToMeter { get; set; }
        public float NearClipPlane { get; set; }
        public GUIType PreferredGUI { get; set; }
        public bool ForceIMGUIOnScreen => false;

    }
}
