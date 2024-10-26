using BepInEx;
using JetBrains.Annotations;
using Nautilus.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    internal class BelugaSaveDataHandler : MonoBehaviour
    {
        internal static EventHandler<JsonFileEventArgs> OnStartedSaving()
        {
            Dictionary<string, BelugaSaveData> save = new Dictionary<string, BelugaSaveData>();

            foreach (Beluga beluga in Belugamanager.main.AllBeluga)
            {
                string prefabIdentifierId = beluga.GetComponent<PrefabIdentifier>().id;
                string seamothId = "";
                string exosuitId = "";
                LightingController.LightingState lightingState;
                bool shield;
                bool destructed;

                if (beluga.currentSeaMoth)
                {
                    seamothId = beluga.currentSeaMoth.GetComponent<PrefabIdentifier>().id;
                }
                if (beluga.currentprawn)
                {
                    exosuitId = beluga.currentprawn.GetComponent<PrefabIdentifier>().id;
                }

                lightingState = beluga.GetComponent<LightingController>().state;

                shield = beluga.shielded;

                destructed = beluga.isScuttled;


                BelugaSaveData saveData = new BelugaSaveData();
                saveData.seamothPrefabIdentifierId = seamothId;
                saveData.exosuitPrefabIdentifierId= exosuitId;
                saveData.lightingState = lightingState;
                saveData.shieldActive = shield;
                saveData.destroyed = destructed;

                save.Add(prefabIdentifierId, saveData);
            }

            return null;
        }

        internal static EventHandler<JsonFileEventArgs> OnFinishedSaving()
        {
            return null;
        }

        internal static EventHandler<JsonFileEventArgs> OnStartedLoading()
        {
            return null;
        }

        internal static EventHandler<JsonFileEventArgs> OnFinishedLoading()
        {
            Dictionary<string, BelugaSaveData> save = MainPatcher.save.belugasSaved;

            foreach (Beluga beluga in Belugamanager.main.AllBeluga)
            {
                string id = beluga.GetComponent<PrefabIdentifier>().id;

                foreach (string savedId in save.Keys)
                {
                    if (id != savedId)
                    {
                        continue;
                    }

                    BelugaSaveData saveData = save[id];


                    SeaMoth seamoth = null;
                    if (!saveData.seamothPrefabIdentifierId.IsNullOrWhiteSpace())
                    {
                        seamoth = GetSeamothFromId(saveData.seamothPrefabIdentifierId);
                    }
                    Exosuit exosuit = null;
                    if (!saveData.exosuitPrefabIdentifierId.IsNullOrWhiteSpace())
                    {
                        exosuit = GetExosuitFromId(saveData.exosuitPrefabIdentifierId);
                    }

                    LightingController.LightingState lightState = saveData.lightingState;

                    bool shield = saveData.shieldActive;

                    bool destructed = saveData.destroyed;


                    if (seamoth)
                    {
                        beluga.currentSeaMoth = seamoth;
                    }
                    if (exosuit)
                    {
                        beluga.currentprawn = exosuit;
                    }

                    beluga.GetComponent<LightingController>().state = lightState;

                    beluga.shielded = shield;

                    if (destructed && !beluga.isScuttled)
                    {
                        beluga.DestroyVehicleInstantly();
                    }
                }
            }

            return null;
        }






        // utility methods


        private static SeaMoth GetSeamothFromId(string id)
        {
            foreach (SeaMoth seamoth in SeaMothmanager.main.AllSeaMoth)
            {
                if (id == seamoth.GetComponent<PrefabIdentifier>().id)
                {
                    return seamoth;
                }
            }
            Debug.LogError("[Beluga] Found no seamoth with id " + id);
            return null;
        }

        private static Exosuit GetExosuitFromId(string id)
        {
            foreach (Exosuit exosuit in PrawnManager.main.AllPrawns)
            {
                if (id == exosuit.GetComponent<PrefabIdentifier>().id)
                {
                    return exosuit;
                }
            }
            Debug.LogError("[Beluga] Found no exosuit with id " + id);
            return null;
        }
    }
}
