using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework.VehicleComponents;
using VehicleFramework.VehicleTypes;

namespace Beluga
{
    public partial class Beluga : Submarine 
    {
        public MVCameraController cams = null;
        private Transform forwardCam => transform.Find("Cams/Top");
        private Transform bottomCam => transform.Find("Cams/Bottom");
        private Transform rearCam => transform.Find("Cams/Back");
        
        public void setupcams()
        {
            cams = gameObject.AddComponent<MVCameraController>();
            cams.AddCamera(forwardCam, "forward");
           
            cams.AddCamera(rearCam, "rear");
           
            cams.AddCamera(bottomCam, "bottom");
           



        }

        public override void StopPiloting()
        {
            this.GetComponent<MVCameraController>().SetState("player");
            base.StopPiloting();
        }







    }
}
