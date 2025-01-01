using Beluga;
using HarmonyLib;
using JetBrains.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using static VFXParticlesPool;

namespace Beluga.Patches
{
    [HarmonyPatch(typeof(Exosuit))]
    internal class Exosuitregisterpatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void Postfix(Exosuit __instance)
        {
            PrawnManager.main.RegisterPrawn(__instance);

        }








    }

    [HarmonyPatch(typeof(Exosuit))]
    internal class ExosuitUpdatepatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        public static void Postfix(Exosuit __instance)
        {
            if (Belugamanager.FindNearestBeluga(__instance.transform.position) != null)
            {
                if (__instance = Belugamanager.FindNearestBeluga(__instance.transform.position).currentprawn)

                {
                    //__instance.mainAnimator.SetBool("sit", true);
                    //Logger.Log("spammm");

                }
            }
        }








    }






}
