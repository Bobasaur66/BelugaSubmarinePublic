

using BepInEx;
using Nautilus.Json;
using Nautilus.Json.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    [FileName("BelugaSaveData")]
    public class SaveData : SaveDataCache
    {
        // dictionary of saved belugas (prefab id, data)
        public Dictionary<string, BelugaData> belugasSaved;

        // first time voicelines
        public bool craftVoiceline;
        public bool agilityVoiceline;
    }

    public class BelugaData
    {
        public LightingController.LightingState lightingState;
        public bool shieldActive;
        public bool destroyed;
    }

    public class BelugaDataLoader : MonoBehaviour
    {
        public void Start()
        {
            Debug.Log("[Beluga] Loading data for a beluga");
            UWE.CoroutineHost.StartCoroutine(LoadData());
        }

        public IEnumerator LoadData()
        {
            // we need these because otherwise it loads the data up when the beluga doesn't exist yet and causes all kinds of nasty errors
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();


            Belugamanager.agilityflag = MainPatcher.save.agilityVoiceline;
            Belugamanager.craftflag = MainPatcher.save.craftVoiceline;

            Debug.Log("1");

            if (BelugaSaveHandler.saveExists == false)
            {
                yield break;
            }
            Debug.Log("2");

            Dictionary<string, BelugaData> save = MainPatcher.save?.belugasSaved;
            if (save == null)
            {
                Debug.LogWarning("[Beluga] No save data found.");
            }
            Debug.Log("3");

            string id = gameObject.GetComponent<PrefabIdentifier>()?.id;
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("[Beluga] PrefabIdentifier id is null or empty.");
            }
            Debug.Log("4");

            if (save != null && !string.IsNullOrEmpty(id) && save.ContainsKey(id))
            {
                Debug.Log("5");

                BelugaData saveData = save[id];
                if (saveData != null)
                {
                    Debug.Log("6");

                    LightingController.LightingState lightState = saveData.lightingState;
                    Debug.Log("9");

                    bool shield = saveData.shieldActive;
                    Debug.Log("10");

                    bool destructed = saveData.destroyed;
                    Debug.Log("11");

                    Beluga beluga = gameObject.GetComponent<Beluga>();
                    if (beluga != null)
                    {
                        Debug.Log("12");

                        LightingController lightingController = beluga.GetComponent<LightingController>();
                        if (lightingController != null)
                        {
                            lightingController.state = lightState;
                        }
                        Debug.Log("15");

                        beluga.shielded = shield;
                        Debug.Log("16");

                        if (destructed)
                        {
                            beluga.DestroyVehicleInstantly();
                        }
                        Debug.Log("17");
                    }
                    else
                    {
                        Debug.LogWarning("[Beluga] Beluga component not found.");
                    }
                }
                else
                {
                    Debug.LogWarning("[Beluga] Save data for id is null.");
                }
            }
        }
    }

    internal class BelugaSaveHandler : MonoBehaviour
    {
        public static bool saveExists;

        internal void Awake()
        {
            saveExists = false;
        }

        internal void OnDestroy()
        {
            MainPatcher.save.OnStartedSaving -= OnStartedSaving;
            MainPatcher.save.OnFinishedLoading -= OnFinishedLoading;
        }

        internal static void OnStartedSaving(object sender, JsonFileEventArgs args)
        {
            try
            {
                OnStartedSavingInternal(sender, args);
            }
            catch(Exception e)
            {
                VehicleFramework.Logger.LogException("Failed to save Beluga Data!", e, true);
            }
        }
        internal static void OnStartedSavingInternal(object sender, JsonFileEventArgs args)
        {
            if (sender == null)
            {
                return;
            }
            MainPatcher.save.agilityVoiceline = Belugamanager.agilityflag;
            MainPatcher.save.craftVoiceline = Belugamanager.craftflag;


            if (Belugamanager.AllBeluga == null || Belugamanager.AllBeluga.Count == 0)
            {
                Debug.Log("[Beluga] No belugas exist.");
                MainPatcher.save.belugasSaved = new Dictionary<string, BelugaData>();
                return;
            }

            Dictionary<string, BelugaData> save = new Dictionary<string, BelugaData>();

            foreach (Beluga beluga in Belugamanager.AllBeluga)
            {
                PrefabIdentifier prefabIdentifier = beluga.GetComponent<PrefabIdentifier>();
                if (prefabIdentifier == null)
                {
                    Debug.LogWarning("[Beluga] PrefabIdentifier is null for a Beluga.");
                    continue;
                }

                string prefabIdentifierId = prefabIdentifier.id;
                if (string.IsNullOrEmpty(prefabIdentifierId))
                {
                    Debug.LogWarning("[Beluga] PrefabIdentifier id is null or empty.");
                    continue;
                }

                Debug.Log("[Beluga] Getting data for Beluga with id " + prefabIdentifierId);

                LightingController lightingController = beluga.GetComponent<LightingController>();
                if (lightingController == null)
                {
                    Debug.LogWarning("[Beluga] LightingController is null for a Beluga.");
                    continue;
                }

                LightingController.LightingState lightingState = lightingController.state;
                bool shield = beluga.shielded;
                bool destructed = beluga.isScuttled;

                BelugaData saveData = new BelugaData
                {
                    lightingState = lightingState,
                    shieldActive = shield,
                    destroyed = destructed
                };

                save.Add(prefabIdentifierId, saveData);
            }

            MainPatcher.save.belugasSaved = save;
        }

        internal static void OnFinishedLoading(object sender, JsonFileEventArgs args)
        {
            if(sender == null)
            {
                return;
            }
            saveExists = true;
        }
    }
}
