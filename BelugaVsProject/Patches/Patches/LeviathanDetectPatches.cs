using Beluga.AudioShit;
using HarmonyLib;
using Nautilus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;

namespace Beluga.Patches.Patches
{
    [HarmonyPatch(typeof(Creature))]
    [HarmonyPatch("ChooseBestAction")]
    public class LeviathanDetectPatches
    {
        [HarmonyPostfix]
        public static void Postfix(Creature __instance)
        {
            Beluga beluga = Player.main.GetVehicle() as Beluga;

            float distanceThreshhold = 200f;

            if (!beluga) return;
            
            float dist = Vector3.Distance(beluga.gameObject.transform.position, __instance.transform.position);
            if (dist > distanceThreshhold) return;

            string inst = __instance.name.TrimClone().ToLower();

            bool flag = false;

            if (inst.Contains("leviathan")) flag = true;
            if (inst.Contains("seadragon")) flag = true;
            if (inst.Contains("gulper")) flag = true;
            if (inst.Contains("bloop")) flag = true;
            if (inst.Contains("blaza")) flag = true;

            if (!flag)
            {
                return;
            }

            beluga.gameObject.GetComponent<BelugaVoicelineManager>().DetectLeviathan();
        }
    }
}
