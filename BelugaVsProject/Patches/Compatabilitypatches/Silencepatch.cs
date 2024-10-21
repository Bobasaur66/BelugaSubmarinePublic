using CallOfTheVoid.Mono.Creatures.Silence;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Beluga.Compatabilitypatches
{
    [HarmonyPatch(typeof(SilenceSmallVehicleGrab))]
    class Silencepatcher
    {

        [HarmonyPrefix]
        [HarmonyPatch("GrabVehicle")]
        public static bool GrabPrefix(Vehicle vehicle)
        {
            if (vehicle is Beluga)
            {
                


                


                return false;
            }
            return true;
        }
        
        
        }
            
    }

