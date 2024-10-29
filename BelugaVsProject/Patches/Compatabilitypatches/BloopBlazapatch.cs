
using BloopAndBlaza.Mono;
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
        [HarmonyPatch(typeof(BloopMeleeAttack))]
        class Blooppatch
        {

            [HarmonyPrefix]
            [HarmonyPatch("CanAttackTargetFromPosition")]
            public static bool OntouchPrefix(BloopMeleeAttack __instance, bool __result)
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


    

    




    

