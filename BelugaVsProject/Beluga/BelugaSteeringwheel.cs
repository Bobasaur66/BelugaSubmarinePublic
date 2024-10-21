using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;
using VehicleFramework.VehicleTypes;

namespace Beluga
{
    public partial class Beluga : Submarine
    {
        public override GameObject SteeringWheelLeftHandTarget
        {
            get
            {
                return GameObject.Find("Model/SteeringConsoleWheelInt/LeftTarget");

            }
        }
        public override GameObject SteeringWheelRightHandTarget
        {
            get
            {
                return GameObject.Find("Model/SteeringConsoleWheelInt/RightTarget");

            }
        }

        public GameObject Steeringwheel
        {
            get
            {
                return transform.Find("Model/SteeringConsoleWheelInt").gameObject;
            }
        }

        public override PilotingStyle pilotingStyle => PilotingStyle.Cyclops;

    }
}
