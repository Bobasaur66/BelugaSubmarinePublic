using BloopAndBlaza.Mono;
using DeExtinction.Mono;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga.Compatabilitypatches
{
    [HarmonyPatch(typeof(GulperMeleeAttackMouth))]
    class DeextinctionPatch
    {

        [HarmonyPrefix]
        [HarmonyPatch("OnTouch")]
        public static bool OntouchPrefix(GulperMeleeAttackMouth __instance)
        {
            return false;
        }
    }
}