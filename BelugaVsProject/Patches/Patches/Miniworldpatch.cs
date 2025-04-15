using Beluga;
using Beluga.Components;
using HarmonyLib;
using Nautilus.Utility;
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
    [HarmonyPatch(typeof(MiniWorld))]
    internal class MiniWorldPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MiniWorld.UpdatePosition))]
        public static bool Prefix(MiniWorld __instance)
        {
            return true; // this patch is the same as the original, but with logging statements interspersed.

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
    }

    [HarmonyPatch(typeof(MapRoomFunctionality))]
    internal class MapRoomFunctionalityPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MapRoomFunctionality.Start))]
        public static bool MapRoomFunctionalityStartPrefix(MapRoomFunctionality __instance)
        {
            if(__instance.GetComponent<Beluga>() != null)
            {
                __instance.wireFrameWorld.rotation = Quaternion.identity;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MapRoomFunctionality.Update))]
        public static bool MapRoomFunctionalityUpdatePrefixPrefix(MapRoomFunctionality __instance)
        {
            Beluga beluga = __instance.GetComponent<Beluga>();
            return beluga == null; // for now, do nothing when on a Beluga

            Logger.Log("Checking if the parent of miniWorld is a Beluga...");

            // Check if miniWorld is valid and if its parent is a Beluga
            if (__instance.miniWorld != null && __instance.miniWorld.gameObject != null)
            {
                GameObject parentObject = __instance.miniWorld.gameObject.transform.parent?.gameObject;

                if (parentObject != null && parentObject.GetComponent<Beluga>() != null)
                {
                    Logger.Log("Parent is a Beluga, executing custom Update method");

                    // Call the replacement method here
                    ReplacementUpdate(__instance);

                    // Prevent the original method from running
                    return false;
                }
            }

            // Let the original method run if no Beluga is found
            return true;
        }

        private static void ReplacementUpdate(MapRoomFunctionality __instance)
        {
            // Custom logic for the replacement method, to be executed if the parent is a Beluga
            Logger.Log("ReplacementUpdate executed!");

            try
            {
                // Replace the original logic with your custom functionality.
                // Example: If the parent is a Beluga, change behavior here.

                // Debug log
                Logger.Log("Running replacement logic for MapRoomFunctionality update.");

                // Example logic: Change scan interval based on Beluga condition
                if (__instance.miniWorld != null)
                {
                    __instance.scanInterval = Mathf.Max(1f, 12f); // Example of altering the scan interval
                    Logger.Log("Adjusted scan interval to " + __instance.scanInterval);
                }

                // Handle power state, scanning, etc. Here we can tweak behaviors.
                __instance.UpdateScanning();

                // You can add any other custom behavior needed here.
            }
            catch (Exception ex)
            {
                Logger.Log("Error in ReplacementUpdate: " + ex.Message);
            }
        }
        /*[HarmonyPrefix]
        [HarmonyPatch("UpdateScanRangeAndInterval")]
        public static bool PrefixScanRange(MapRoomFunctionality __instance)
        {
            Logger.Log("Entering UpdateScanRangeAndInterval");

            if (__instance == null)
            {
                Logger.Log("UpdateScanRangeAndInterval Error: __instance is null");
                return false;
            }

            if (__instance.storageContainer == null)
            {
                Logger.Log("UpdateScanRangeAndInterval Error: storageContainer is null");
                return false;
            }

            if (__instance.storageContainer.container == null)
            {
                Logger.Log("UpdateScanRangeAndInterval Error: container is null");
                return false;
            }

            float previousScanRange = __instance.scanRange;
            Logger.Log("UpdateScanRangeAndInterval: previousScanRange = " + previousScanRange);

            __instance.scanRange = Mathf.Min(500f, 300f + (float)__instance.storageContainer.container.GetCount(TechType.MapRoomUpgradeScanRange) * 50f);
            Logger.Log("UpdateScanRangeAndInterval: new scanRange = " + __instance.scanRange);

            __instance.scanInterval = Mathf.Max(1f, 14f - (float)__instance.storageContainer.container.GetCount(TechType.MapRoomUpgradeScanSpeed) * 3f);
            Logger.Log("UpdateScanRangeAndInterval: scanInterval = " + __instance.scanInterval);

            if (__instance.scanRange != previousScanRange)
            {
                Logger.Log("UpdateScanRangeAndInterval: Scan range changed");
                __instance.ObtainResourceNodes(__instance.typeToScan);
                Logger.Log("UpdateScanRangeAndInterval: Resource nodes updated");

                __instance.onScanRangeChanged?.Invoke();
                Logger.Log("UpdateScanRangeAndInterval: onScanRangeChanged event triggered");
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch("UpdateScanning")]
        public static bool PrefixUpdateScanning(MapRoomFunctionality __instance)
        {
            Logger.Log("Entering UpdateScanning");

            if (__instance == null)
            {
                Logger.Log("UpdateScanning Error: __instance is null");
                return false;
            }

            if (__instance.miniWorld == null)
            {
                Logger.Log("UpdateScanning Error: miniWorld is null");
                return false;
            }

            if (__instance.miniWorld.materialInstance == null)
            {
                Logger.Log("UpdateScanning Error: materialInstance is null");
                return false;
            }

            if (__instance.powerConsumer == null)
            {
                Logger.Log("UpdateScanning Error: powerConsumer is null");
                return false;
            }

            DayNightCycle main = DayNightCycle.main;
            if (!main)
            {
                Logger.Log("UpdateScanning Error: DayNightCycle is null");
                return false;
            }

            Logger.Log("UpdateScanning: All checks passed, proceeding with update");
            Material materialInstance = __instance.miniWorld.materialInstance;
            double timePassed = main.timePassed;
            Logger.Log("UpdateScanning: timePassed = " + timePassed);

            if (__instance.timeLastScan + (double)__instance.scanInterval <= timePassed && __instance.powered)
            {
                Logger.Log("UpdateScanning: Scanning condition met");
                __instance.timeLastScan = timePassed;
                __instance.UpdateBlips();
                Logger.Log("UpdateScanning: Blips updated");

                __instance.UpdateCameraBlips();
                Logger.Log("UpdateScanning: Camera blips updated");

                float num = __instance.scanRange * __instance.mapScale;
                if (__instance.prevFadeRadius != num)
                {
                    materialInstance.SetFloat(ShaderPropertyID._FadeRadius, num);
                    __instance.prevFadeRadius = num;
                    Logger.Log("UpdateScanning: Fade radius updated to " + num);
                }
            }

            if (__instance.scanActive != __instance.prevScanActive || __instance.scanInterval != __instance.prevScanInterval)
            {
                Logger.Log("UpdateScanning: Scan state or interval changed");

                float num2 = 1f / __instance.scanInterval;
                materialInstance.SetFloat(ShaderPropertyID._ScanIntensity, __instance.scanActive ? __instance.scanIntensity : 0f);
                materialInstance.SetFloat(ShaderPropertyID._ScanFrequency, __instance.scanActive ? num2 : 0f);

                __instance.prevScanActive = __instance.scanActive;
                __instance.prevScanInterval = __instance.scanInterval;
                Logger.Log("UpdateScanning: Scan material properties updated");
            }

            if (__instance.powered && __instance.timeLastPowerDrain + 1f < Time.time)
            {
                Logger.Log("UpdateScanning: Power drain condition met");

                float powerUsed;
                __instance.powerConsumer.ConsumePower(__instance.scanActive ? 0.5f : 0.15f, out powerUsed);
                __instance.timeLastPowerDrain = Time.time;

                Logger.Log("UpdateScanning: Power consumed = " + powerUsed);
            }

            return false;
        }*/
    }

}










