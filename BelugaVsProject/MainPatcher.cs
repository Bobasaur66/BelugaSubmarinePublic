using BepInEx;
using HarmonyLib;
using Nautilus;

using System;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using VehicleFramework;
using VehicleFramework.VehicleParts;
using VehicleFramework.VehicleTypes;
using VehicleFramework.Admin;
using Beluga.AudioShit;
using Nautilus.Handlers;
using BepInEx.Bootstrap;
using BepInEx.Bootstrap;
using BepInEx;
using Beluga.Compatabilitypatches;
using Beluga.Patches;
using BepInEx.Logging;
using Beluga.Patches.Patches;
using Beluga.Upgrades;
using Beluga.Commands;
using Nautilus.Json;
using static FlexibleGridLayout;
using VehicleFramework.Engines;



namespace Beluga
{
    public static class Logger
    {
        public static void Log(string message)
        {
            UnityEngine.Debug.Log("[Beluga]:" + message);
        }

    }


    [BepInPlugin("com.blizzard.subnautica.beluga.mod", "Beluga", "1.3.0")]
    [BepInDependency("com.mikjaw.subnautica.vehicleframework.mod","1.3.3")]
    [BepInDependency("com.snmodding.nautilus")]
    [BepInDependency("com.lee23.bloopandblaza", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.aci.thesilence", BepInDependency.DependencyFlags.SoftDependency)]



    public class MainPatcher : BaseUnityPlugin
    {
        public static AssetBundle theUltimateBundleOfAssets;
        public static SavingShit save { get; private set; }

        public void Start()
        {
            // patches
            var harmony = new Harmony("com.blizzard.subnautica.beluga.mod");
            harmony.PatchAll(typeof(SeaMothregisterpatch));
            harmony.PatchAll(typeof(Exosuitregisterpatch));
            harmony.PatchAll(typeof(LeviathanDetectPatches));
            CompPatch();

            // reference
            string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // asset bundle
            theUltimateBundleOfAssets = AssetBundle.LoadFromFile(Path.Combine(modFolder, "Assets/beluga"));

            // register audioclips
            AudioRegistrar.RegisterAudio(theUltimateBundleOfAssets);

            // register localizations
            LanguageHandler.RegisterLocalizationFolder();

            // register submarine
            UWE.CoroutineHost.StartCoroutine(Beluga.Register());

            // register console commands
            ConsoleCommandsHandler.RegisterConsoleCommands(typeof(ConsoleCommands));

            // register all upgrades
            RegisterAllUpgrades();

            // add the ency for the beluga
            AddEncy();

            // register save data
            DoSaveDataStuff();
        }



        public void DoSaveDataStuff()
        {
            save = SaveDataHandler.RegisterSaveDataCache<SavingShit>();

            save.OnStartedSaving += (object sender, JsonFileEventArgs e) =>
            {
                SavingShit data = e.Instance as SavingShit;

                List<Tuple<Beluga, SeaMoth, Exosuit, LightingController.LightingState, bool, bool>> saveList = new List<Tuple<Beluga, SeaMoth, Exosuit, LightingController.LightingState, bool, bool>>();

                foreach (Beluga beluga in Belugamanager.main.AllBeluga)
                {
                    BelugaEngine engine = beluga.GetComponent<ModVehicleEngine>() as BelugaEngine;

                    SeaMoth seamoth = beluga.currentSeaMoth;
                    Exosuit exosuit = beluga.currentprawn;
                    LightingController.LightingState lightingState = beluga.GetComponent<LightingController>().state;
                    bool shield = beluga.shielded;
                    bool destroyed = beluga.isScuttled;

                    Tuple<Beluga, SeaMoth, Exosuit, LightingController.LightingState, bool, bool> newData = new Tuple<Beluga, SeaMoth, Exosuit, LightingController.LightingState, bool, bool>(beluga, seamoth, exosuit, lightingState, shield, destroyed);
                    saveList.Add(newData);
                }

                data.belugaSaveData = saveList;
            };

            save.OnFinishedLoading += (object sender, JsonFileEventArgs e) =>
            {
                save = e.Instance as SavingShit;

                foreach (var tuple in save.belugaSaveData)
                {
                    Beluga beluga = tuple.Item1;
                    SeaMoth seamoth = tuple.Item2;
                    Exosuit exosuit = tuple.Item3;
                    LightingController.LightingState lightingState = tuple.Item4;
                    bool shield = tuple.Item5;
                    bool destroyed = tuple.Item6;

                    beluga.currentSeaMoth = seamoth;
                    beluga.currentprawn = exosuit;
                    beluga.GetComponent<LightingController>().state = lightingState;
                    beluga.shielded = shield;

                    beluga.isScuttled = destroyed;
                }


            };
        }


        public void CompPatch()
        {
            BepInEx.PluginInfo pluginInfo;
            Chainloader.PluginInfos.TryGetValue("com.aci.thesilence", out pluginInfo);
            bool flag = pluginInfo != null && pluginInfo.Instance != null;
            bool flag2 = flag;
            if (flag2)
            {
                BaseUnityPlugin instance = pluginInfo.Instance;
                bool flag3 = instance;
                if (flag3)
                {
                    var harmony = new Harmony("com.blizzard.patch");
                    harmony.PatchAll(typeof(Silencepatcher)); // Apply the patch only if Somepatch is true
                    Debug.Log("Silence Found Enabling");
                    

                }
                else
                {
                    Debug.Log("Silence not found");
                }
            }

            BepInEx.PluginInfo pluginInfo2;
            Chainloader.PluginInfos.TryGetValue("com.lee23.bloopandblaza", out pluginInfo2);
            bool flag4 = pluginInfo2 != null && pluginInfo2.Instance != null;
            bool flag5 = flag4;
            if (flag5)
            {
                BaseUnityPlugin instance2 = pluginInfo2.Instance;
                bool flag6 = instance2;
                if (flag6)
                {
                    var harmony = new Harmony("com.blizzard.patch");
                    harmony.PatchAll(typeof(Blooppatch));
                    Debug.Log("com.lee23.bloopandblaza Found Enabling");
                }
                else
                {
                    Debug.Log("com.lee23.bloopandblaza not found");
                }
            }

            BepInEx.PluginInfo pluginInfo3;
            Chainloader.PluginInfos.TryGetValue("com.lee23.deextinction", out pluginInfo3);
            bool deFlag = pluginInfo3 != null && pluginInfo3.Instance != null;
            if (deFlag)
            {
                BaseUnityPlugin instance3 = pluginInfo3.Instance;
                bool deFlag2 = instance3;
                if (deFlag2)
                {
                    var harmony = new Harmony("com.blizzard.patch");
                    harmony.PatchAll(typeof(DeextinctionPatch));
                    Debug.Log("com.lee23.deextinction Found Enabling");
                }
                else
                {
                    Debug.Log("com.lee23.deextinction not found");
                }
            }
        }

        public void RegisterAllUpgrades()
        {
            UpgradeCompat compat = new UpgradeCompat
            {
                skipCyclops = true,
                skipModVehicle = false,
                skipSeamoth = true,
                skipExosuit = true
            };
            UpgradeTechTypes speedUTT = UpgradeRegistrar.RegisterUpgrade(new BelugaSpeedUpgradeModule(), compat);
            UpgradeTechTypes thermalUTT = UpgradeRegistrar.RegisterUpgrade(new BelugaThermalRepairModule(), compat);
        }

        public void AddEncy()
        {
            PDAHandler.AddEncyclopediaEntry("BelugaEncy", "Tech/Vehicles", Language.main.Get("Beluga"), Language.main.Get("Ency"));
            StoryGoalHandler.RegisterItemGoal("BelugaEncy", Story.GoalType.Encyclopedia, TechType.Constructor);
        }
    }
}