using Beluga;
using Beluga.Components;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga.Patches
{

    /*
    [HarmonyPatch(typeof(MiniWorld))]
    internal class MiniWorldPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("UpdatePosition")]
        public static bool Prefix(MiniWorld __instance)
        {
            Logger.Log("Map1");

            __instance.hologramHolder.rotation = Quaternion.identity;
            Logger.Log("Map1.5");
            // Use AccessTools to get the private transform
            Transform transform = (Transform)AccessTools.Property(typeof(MiniWorld), "transform").GetValue(__instance);
            Logger.Log("Map2");
            // Access transform.position
            Vector3 instancePosition = transform.position;
            Logger.Log("Map3");
            __instance.materialInstance.SetVector(ShaderPropertyID._MapCenterWorldPos, instancePosition);
            Logger.Log("Map4");
            Vector3 vector = LargeWorldStreamer.main.land.transform.InverseTransformPoint(instancePosition) / 4f;
            Logger.Log("Map5");
            foreach (KeyValuePair<Int3, MiniWorld.Chunk> keyValuePair in __instance.loadedChunks)
            {
                // Use a local variable to access the value
                MiniWorld.Chunk chunk = keyValuePair.Value;
                Logger.Log("Map6");
                // Compute the new local position
                Vector3 vector2 = (keyValuePair.Key * 32).ToVector3() - vector;
                chunk.gameObject.transform.localPosition = vector2 * __instance.chunkScale;
            }

            return false;
        }
    }*/
}
    
    
    

