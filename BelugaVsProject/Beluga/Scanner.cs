using Beluga.AudioShit;
using Beluga.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;
using VehicleFramework.VehicleComponents;
using Beluga.Components;
using RootMotion.FinalIK;
using JetBrains.Annotations;
using Nautilus.Utility;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;

namespace Beluga
{
    partial class Beluga
    {
        public Transform minimap
        {
            get
            {
                return transform.Find("Minimap");
            }
        }

        public void Mapsetup()
        {
            if (minimap == null)
            {
                Logger.Log("Mapsetup Error: Minimap transform not found!");
                return;
            }

            minimap.gameObject.AddComponent<MiniWorld>();
            MiniWorld Karl = minimap.gameObject.GetComponent<MiniWorld>();

            if (Karl == null)
            {
                Logger.Log("Mapsetup Error: Failed to add MiniWorld component!");
                return;
            }

            Karl.hologramHolder = minimap;
            Karl.hologramObject = minimap.gameObject;
            Karl.hologramRadius = 12;
            Karl.mapWorldRadius = 100;
            Karl.fadeRadius = 1.8f;
            Karl.fadeSharpness = 5;

            if (MaterialUtils.HolographicUIMaterial == null)
            {
                Logger.Log("Mapsetup Error: GhostMaterial is null!");
                return;
            }

            // The HolographicUIMaterial makes the minimap invisible.
            //Material mat = MaterialUtils.HolographicUIMaterial;
            Material mat = MaterialUtils.GhostMaterial;


            Karl.hologramMaterial = mat;
            Karl.materialInstance = mat;

            MapRoomFunctionality MapC = gameObject.EnsureComponent<MapRoomFunctionality>();

            if (MapC == null)
            {
                Logger.Log("Mapsetup Error: Failed to add MapRoomFunctionality component!");
                return;
            }

            MapC.wireFrameWorld = minimap;
            MapC.worlddisplay = minimap.gameObject;
            MapC.miniWorld = Karl;

            MapC.powerConsumer = gameObject.EnsureComponent<PowerConsumer>();
            if (MapC.powerConsumer == null)
            {
                Logger.Log("Mapsetup Error: PowerConsumer component is missing!");
                return;
            }

            Logger.Log("Mapsetup completed successfully.");
        }
    }
}
