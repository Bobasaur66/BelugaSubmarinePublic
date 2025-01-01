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
            Logger.Log("Kaaaaarl");
            
            this.gameObject.AddComponent<MiniWorld>();
            MiniWorld Karl = this.gameObject.GetComponent<MiniWorld>();
            Karl.hologramHolder = minimap.transform;
            Karl.hologramObject = minimap.gameObject;
            Karl.hologramRadius = 12;
            Karl.mapWorldRadius = 10;

            Karl.hologramMaterial = MaterialUtils.GhostMaterial;
            Material minimat = MaterialUtils.GhostMaterial;
            Karl.materialInstance = minimat;
            if (Karl == null) { Logger.Log("Kaaaaarl is deaaaad nooooo"); };
           // Karl.transform.position = this.transform.position;
            //Karl.transform.parent = this.transform;
            //Karl.materialInstance = MaterialUtils.GhostMaterial;
            Karl.EnableMap();
            Karl.InitializeHologram();




        }
        




    }
}