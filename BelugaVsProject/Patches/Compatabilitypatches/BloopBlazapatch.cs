using BloopAndBlaza.Mono;
using HarmonyLib;
using UnityEngine;

namespace Beluga.Compatabilitypatches
{
    [HarmonyPatch(typeof(BloopMeleeAttack))]
    class BloopAndBlazaPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("CanAttackTargetFromPosition")]
        public static bool BloopMeleeAttackCanAttackTargetFromPositionPrefix(BloopMeleeAttack __instance, bool __result)
        {
            if (Vector3.Distance(__instance.transform.position, Belugamanager.closestBeluga().transform.position) < 50)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
