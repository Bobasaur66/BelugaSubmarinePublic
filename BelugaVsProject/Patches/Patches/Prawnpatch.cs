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


    



}
