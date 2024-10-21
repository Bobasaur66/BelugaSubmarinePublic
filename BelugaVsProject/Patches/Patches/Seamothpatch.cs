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
    [HarmonyPatch(typeof(SeaMoth))]
    internal class SeaMothregisterpatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void Postfix(SeaMoth __instance)
        {
            SeaMothmanager.main.RegisterSeaMoth(__instance);

        }








    }






}
