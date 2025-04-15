using DeExtinction.Mono;
using HarmonyLib;
using UnityEngine;

namespace Beluga.Compatabilitypatches
{
    [HarmonyPatch(typeof(GulperMeleeAttackMouth))]
    class DeExtinctionPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GulperMeleeAttackMouth.OnTouch))]
        public static bool GulperMeleeAttackMouthOnTouchPrefix(GulperMeleeAttackMouth __instance, Collider collider)
        {
            return collider.gameObject.GetComponent<Beluga>() == null;
        }
    }
}