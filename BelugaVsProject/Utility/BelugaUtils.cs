using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Nautilus;
using System.Collections.Generic;
using System.Collections;
using static HandReticle;
using VehicleFramework;
using System.Linq;

namespace Beluga
{
    public static class BelugaUtils
    {
        public static void NautilusBasicText(string msg, float time)
        {
            Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
            message.ShowMessage(msg, time * Time.deltaTime);
        }

        public static void PlayFMODSound(string soundName, Transform position)
        {
            var asset = Nautilus.Utility.AudioUtils.GetFmodAsset(soundName);
            Utils.PlayFMODAsset(asset, position);
        }

        public static IEnumerator SpawnBeluga(bool setInside, Vector3? pos = null)
        {
            if (pos == null) pos = Player.main.transform.position + (Camera.main.transform.forward * 50);

            var task = CraftData.GetPrefabForTechTypeAsync(GetBelugaTechType());
            yield return task;
            var obj = GameObject.Instantiate(task.GetResult());

            obj.transform.position = (Vector3)pos;
            if (setInside)
                Player.main.currentSub = obj.GetComponent<SubRoot>();

            CrafterLogic.NotifyCraftEnd(obj, GetBelugaTechType());
        }

        public static float GetTemperature(Vector3 position)
        {
            WaterTemperatureSimulation main = WaterTemperatureSimulation.main;
            if (!(main != null))
            {
                return 0f;
            }
            return main.GetTemperature(position);
        }

        public static TechType GetBelugaTechType()
        {
            string myVehicleName = "Beluga";
            TechType tt = VehicleManager.vehicleTypes.Where(x => x.name.Contains(myVehicleName)).FirstOrDefault().techType;
            return tt;
        }
    }
}
