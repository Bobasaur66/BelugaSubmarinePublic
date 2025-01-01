using BepInEx;
using HarmonyLib;
using Nautilus.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using VehicleFramework;
using VehicleFramework.Engines;
using VehicleFramework.Patches;
using VehicleFramework.StorageComponents;
using VehicleFramework.VehicleParts;
using VehicleFramework.VehicleTypes;
using Newtonsoft.Json;
using Nautilus.Crafting;
using static RootMotion.FinalIK.RagdollUtility;
using Discord;
using Beluga.AudioShit;
using UnityEngine.Experimental.GlobalIllumination;



namespace Beluga
{
    public partial class Beluga : Submarine
    {
        public static GameObject model = null;

        public static Atlas.Sprite pingSprite;

        public static Atlas.Sprite crafterSprite;

        public static GameObject crosshairCanvas;

        public bool interiorLightsActive = false;

        
        
        public static void GetAssets()
        {
            model = MainPatcher.theUltimateBundleOfAssets.LoadAsset<GameObject>("BelugaVehicle");

            SpriteAtlas spriteAtlas = MainPatcher.theUltimateBundleOfAssets.LoadAsset<SpriteAtlas>("SpriteAtlas");
            pingSprite = new Atlas.Sprite(spriteAtlas.GetSprite("BelugaPingSprite"), false);
            crafterSprite = new Atlas.Sprite(spriteAtlas.GetSprite("BelugaCrafterSprite"), false);

            crosshairCanvas = MainPatcher.theUltimateBundleOfAssets.LoadAsset<GameObject>("CrosshairCanvas");
        }

        public GameObject interiorLights
        {
            get
            {
                return transform.Find("InteriorLights").gameObject;
            }
        }

        
        public override string vehicleDefaultName
        {
            get
            {
                return Language.main.Get("Beluga");
            }
        }

        
        public Transform voicelinesLoc
        {
            get
            {
                return transform.Find("VoicelinesLoc");
            }
        }
        
        public override GameObject VehicleModel
        {
            get
            {
                return model;
            }
        }
        

        public VFXController fxControl
        {
            get
            {
                return transform.Find("FXControl").gameObject.EnsureComponent<VFXController>();
            }
        }

        public override GameObject StorageRootObject
        {
            get
            {
                return transform.Find("StorageRoot").gameObject;
            }
        }

        public override GameObject ModulesRootObject
        {
            get
            {
                return transform.Find("ModulesRoot").gameObject;
            }
        }

      

        public override List<VehiclePilotSeat> PilotSeats
        {
            get
            {
                var list = new List<VehicleFramework.VehicleParts.VehiclePilotSeat>();
                VehicleFramework.VehicleParts.VehiclePilotSeat vps = new VehicleFramework.VehicleParts.VehiclePilotSeat();
                Transform mainSeat = transform.Find("PilotSeat");
                vps.Seat = mainSeat.gameObject;
                vps.SitLocation = mainSeat.Find("SitLocation").gameObject;
                vps.LeftHandLocation = mainSeat;
                vps.RightHandLocation = mainSeat;
                vps.ExitLocation = mainSeat.Find("ExitLocation");
                // TODO exit location
                list.Add(vps);
                return list;
            }
        }

        public override List<VehicleHatchStruct> Hatches
        {
            get
            {
                var list = new List<VehicleFramework.VehicleParts.VehicleHatchStruct>();

                VehicleFramework.VehicleParts.VehicleHatchStruct interior_vhs = new VehicleFramework.VehicleParts.VehicleHatchStruct();
                Transform intHatch = transform.Find("Hatches/TopHatch/InsideHatch");
                interior_vhs.Hatch = intHatch.gameObject;
                interior_vhs.EntryLocation = intHatch.Find("Entry");
                interior_vhs.ExitLocation = intHatch.Find("Exit");
                interior_vhs.SurfaceExitLocation = intHatch.Find("SurfaceExit");

                VehicleFramework.VehicleParts.VehicleHatchStruct exterior_vhs = new VehicleFramework.VehicleParts.VehicleHatchStruct();
                Transform extHatch = transform.Find("Hatches/TopHatch/OutsideHatch");
                exterior_vhs.Hatch = extHatch.gameObject;
                exterior_vhs.EntryLocation = interior_vhs.EntryLocation;
                exterior_vhs.ExitLocation = interior_vhs.ExitLocation;
                exterior_vhs.SurfaceExitLocation = interior_vhs.SurfaceExitLocation;

                list.Add(interior_vhs);
                list.Add(exterior_vhs);


                VehicleFramework.VehicleParts.VehicleHatchStruct interior_vhs2 = new VehicleFramework.VehicleParts.VehicleHatchStruct();
                Transform intHatch2 = transform.Find("Hatches/BottomHatch/InsideHatch");
                interior_vhs2.Hatch = intHatch2.gameObject;
                interior_vhs2.EntryLocation = intHatch2.Find("Entry");
                interior_vhs2.ExitLocation = intHatch2.Find("Exit");
                interior_vhs2.SurfaceExitLocation = intHatch2.Find("SurfaceExit");

                VehicleFramework.VehicleParts.VehicleHatchStruct exterior_vhs2 = new VehicleFramework.VehicleParts.VehicleHatchStruct();
                Transform extHatch2 = transform.Find("Hatches/BottomHatch/OutsideHatch");
                exterior_vhs2.Hatch = extHatch2.gameObject;
                exterior_vhs2.EntryLocation = interior_vhs2.EntryLocation;
                exterior_vhs2.ExitLocation = interior_vhs2.ExitLocation;
                exterior_vhs2.SurfaceExitLocation = interior_vhs2.SurfaceExitLocation;

                list.Add(interior_vhs2);
                list.Add(exterior_vhs2);

                return list;
            }
        }

        


        public override List<VehicleStorage> ModularStorages
        {
            get
            {
                return null;
            }
        }

        public override List<Transform> LavaLarvaAttachPoints 
        { 
            get {


                var list = new List<Transform>();
                foreach (Transform child in transform.Find("Slugs"))
                {
                    list.Add(child);
                }
                return list;

            }

        
        
        }

        public override List<VehicleBattery> Batteries
        {
            get
            {
                var list = new List<VehicleFramework.VehicleParts.VehicleBattery>();

                VehicleFramework.VehicleParts.VehicleBattery vb1 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb1.BatterySlot = transform.Find("Batteries/Battery (1)").gameObject;
                vb1.BatteryProxy = null;
                list.Add(vb1);

                VehicleFramework.VehicleParts.VehicleBattery vb2 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb2.BatterySlot = transform.Find("Batteries/Battery (2)").gameObject;
                vb2.BatteryProxy = null;
                list.Add(vb2);

                VehicleFramework.VehicleParts.VehicleBattery vb3 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb3.BatterySlot = transform.Find("Batteries/Battery (3)").gameObject;
                vb3.BatteryProxy = null;
                list.Add(vb3);

                VehicleFramework.VehicleParts.VehicleBattery vb4 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb4.BatterySlot = transform.Find("Batteries/Battery (4)").gameObject;
                vb4.BatteryProxy = null;
                list.Add(vb4);

                VehicleFramework.VehicleParts.VehicleBattery vb5 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb5.BatterySlot = transform.Find("Batteries/Battery (5)").gameObject;
                vb5.BatteryProxy = null;
                list.Add(vb5);

                VehicleFramework.VehicleParts.VehicleBattery vb6 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb6.BatterySlot = transform.Find("Batteries/Battery (6)").gameObject;
                vb6.BatteryProxy = null;
                list.Add(vb6);

                VehicleFramework.VehicleParts.VehicleBattery vb7 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb7.BatterySlot = transform.Find("Batteries/Battery (7)").gameObject;
                vb7.BatteryProxy = null;
                list.Add(vb7);

                VehicleFramework.VehicleParts.VehicleBattery vb8 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb8.BatterySlot = transform.Find("Batteries/Battery (8)").gameObject;
                vb8.BatteryProxy = null;
                list.Add(vb8);

                VehicleFramework.VehicleParts.VehicleBattery vb9 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb9.BatterySlot = transform.Find("Batteries/Battery (9)").gameObject;
                vb9.BatteryProxy = null;
                list.Add(vb9);

                VehicleFramework.VehicleParts.VehicleBattery vb10 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb10.BatterySlot = transform.Find("Batteries/Battery (10)").gameObject;
                vb10.BatteryProxy = null;
                list.Add(vb10);

                VehicleFramework.VehicleParts.VehicleBattery vb11 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb11.BatterySlot = transform.Find("Batteries/Battery (11)").gameObject;
                vb11.BatteryProxy = null;
                list.Add(vb11);

                VehicleFramework.VehicleParts.VehicleBattery vb12 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb12.BatterySlot = transform.Find("Batteries/Battery (12)").gameObject;
                vb12.BatteryProxy = null;
                list.Add(vb12);

                VehicleFramework.VehicleParts.VehicleBattery vb13 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb13.BatterySlot = transform.Find("Batteries/Battery (13)").gameObject;
                vb13.BatteryProxy = null;
                list.Add(vb13);

                VehicleFramework.VehicleParts.VehicleBattery vb14 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb14.BatterySlot = transform.Find("Batteries/Battery (14)").gameObject;
                vb14.BatteryProxy = null;
                list.Add(vb14);

                VehicleFramework.VehicleParts.VehicleBattery vb15 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb15.BatterySlot = transform.Find("Batteries/Battery (15)").gameObject;
                vb15.BatteryProxy = null;
                list.Add(vb15);

                VehicleFramework.VehicleParts.VehicleBattery vb16 = new VehicleFramework.VehicleParts.VehicleBattery();
                vb16.BatterySlot = transform.Find("Batteries/Battery (16)").gameObject;
                vb16.BatteryProxy = null;
                list.Add(vb16);

                return list;
            }
        }

        public override List<VehicleBattery> BackupBatteries
        {
            get
            {
                var list = new List<VehicleFramework.VehicleParts.VehicleBattery>();
                return null;
            }
        }

        public override List<VehicleFloodLight> HeadLights
        {
            get
            {
                List<VehicleFloodLight> returnList = new List<VehicleFloodLight>();

                returnList.Add(SetVehicleFloodlight("lights_parent/headlights/headlight1"));
                returnList.Add(SetVehicleFloodlight("lights_parent/headlights/headlight2"));
                returnList.Add(SetVehicleFloodlight("lights_parent/headlights/headlight3"));

                return returnList;
            }
        }

        public VehicleFloodLight SetVehicleFloodlight(string location)
        {
            var item = default(VehicleFloodLight);
            item.Light = base.transform.Find(location).gameObject;
            item.Angle = 70f;
            item.Color = Color.white;
            item.Intensity = 0.6f;
            item.Range = 800f;

            return item;
        }


      







        public override List<GameObject> WaterClipProxies
        {
            get
            {
                var list = new List<GameObject>();
                foreach (Transform child in transform.Find("WaterClipProxies"))
                {
                    list.Add(child.gameObject);

                    foreach (Transform child2 in child)
                    {
                        list.Add(child2.gameObject);
                    }
                }
                return list;
            }
        }

        public override List<GameObject> CanopyWindows
        {
            get
            {
                return null;
            }
        }




        public override GameObject CollisionModel
        {
            get
            {
                return transform.Find("Collider").gameObject;

            }
        }



        public override List<VehicleUpgrades> Upgrades
        {
            get
            {
                var list = new List<VehicleFramework.VehicleParts.VehicleUpgrades>();
                VehicleFramework.VehicleParts.VehicleUpgrades vu = new VehicleFramework.VehicleParts.VehicleUpgrades();
                vu.Interface = transform.Find("UpgradesInterface").gameObject;
                vu.Flap = vu.Interface;
                vu.AnglesClosed = Vector3.zero;
                vu.AnglesOpened = new Vector3(0, 0, 0);

                vu.ModuleProxies = null;

                list.Add(vu);
                return list;
            }
        }

        public override ModVehicleEngine Engine

        {
            get
            {
                return Radical.EnsureComponent<BelugaEngine>(base.gameObject);
            }

        }
        
        public override Dictionary<TechType, int> Recipe
        {
            get
            {
                // don't delete this please
                /*string modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filePath = Path.Combine(modPath, "Recipes/Beluga.json");

                var content = File.ReadAllText(filePath);

                Dictionary<TechType, int> recipe = null;

                recipe = JsonConvert.DeserializeObject<Dictionary<TechType, int>>(content);

                return recipe;*/

                Dictionary<TechType, int> recipe = new Dictionary<TechType, int>();

                recipe.Add(TechType.PlasteelIngot, 5);
                recipe.Add(TechType.Kyanite, 2);
                recipe.Add(TechType.Aerogel, 1);
                recipe.Add(TechType.SeaTreaderPoop, 2);
                recipe.Add(TechType.ReactorRod, 8);
                recipe.Add(TechType.EnameledGlass, 5);
                recipe.Add(TechType.AdvancedWiringKit, 2);
                recipe.Add(TechType.CyclopsHullModule3, 1);
                recipe.Add(TechType.CyclopsShieldModule, 1);
                recipe.Add(TechType.PowerCell, 4);


                return recipe;
            }
        }

        public override Atlas.Sprite PingSprite
        {
            get
            {
                return Beluga.pingSprite;

            }

        }

        public override Atlas.Sprite CraftingSprite
        {
            get
            {
                return Beluga.crafterSprite;
            }
        }

        public override string Description
        {
            get
            {
                return Language.main.Get("Description");
            }

        }

        public override int BaseCrushDepth
        {
            get
            {
                return 2000;
            }

        }

        public override int CrushDepthUpgrade1
        {
            get
            {
                return 2000;
            }
        }

        public override int CrushDepthUpgrade2
        {
            get
            {
                return 2000;
            }
        }

        public override int CrushDepthUpgrade3
        {
            get
            {
                return 2000;
            }
        }

        public override int MaxHealth
        {
            get
            {
                return 10000;
            }

        }

        public override int Mass
        {
            get
            {
                return 100000;

            }

        }

        public override int NumModules
        {
            get
            {
                return 8;

            }

        }

        public override bool HasArms
        {
            get
            {
                return false;

            }

        }

        public override BoxCollider BoundingBoxCollider 
        {
            get
            {
                return transform.Find("BoundingBox").gameObject.GetComponent<BoxCollider>();
            }
        }

        public override List<VehicleFloodLight> FloodLights
        {
            get
            {
                return null;

            }

        }
        public override List<GameObject> NavigationPortLights
        {
            get
            {
                return null;

            }

        }
        public override List<GameObject> NavigationStarboardLights
        {
            get
            {
                return null;

            }

        }
        public override List<GameObject> NavigationPositionLights
        {
            get
            {
                return null;

            }

        }
        public override List<GameObject> NavigationWhiteStrobeLights
        {
            get
            {
                return null;

            }

        }
        public override List<GameObject> NavigationRedStrobeLights
        {
            get
            {
                return null;

            }

        }
        public override List<GameObject> TetherSources
        {
            get
            {
                var list = new List<GameObject>();
                foreach (Transform child in transform.Find("TetherSources"))
                {
                    list.Add(child.gameObject);
                }
                return list;

            }
        }
        public override GameObject ControlPanel
        {
            get
            {
                return null;

            }

        }

        public override GameObject Fabricator
        {
            get
            {
                return null;
            }
        }

        public override List<VehicleStorage> InnateStorages
        {
            get
            {


                return null;
            }
        }

        public override bool CanLeviathanGrab
        {
            get
            {
                return false;
            }
        }

        public override bool AutoApplyShaders
        {
            get
            {
                return false;
            }
        }

        public override float TimeToConstruct
        {
            get
            {
                return 60f;
            }
        }

        public override bool CanMoonpoolDock {
            get
            {
                return false;
            }
        }

        public override void PlayerExit()
        {
            InstantHatchDoors(true);
            disableshield();

            base.PlayerExit();
        }

        public override void PlayerEntry()
        {
            base.PlayerEntry();

            InstantHatchDoors(false);

            gameObject.GetComponent<BelugaVoicelineManager>().PlayEntryVoiceline(1);
        }

        // stuff for destroying sub
        public GameObject wreckedModel
        {
            get
            {
                return transform.Find("Wrecked").gameObject;
            }
        }
        public GameObject unwreckedModel
        {
            get
            {
                return transform.Find("Model").gameObject;
            }
        }
        public GameObject unwreckedColliders
        {
            get
            {
                return transform.Find("Collider").gameObject;
            }
        }



        public GameObject engineLight
        {
            get
            {
                return transform.Find("EngineLight").gameObject;
            }
        }
        public  List<GameObject> UnityLights
        {
            get
            {
                var list = new List<GameObject>();
                foreach (Transform child in transform.Find("InteriorLights"))
                {
                    list.Add(child.gameObject);
                }
                return list;

            }
        }

        public BelugaEngineFMODEmitter engineSoundEmitter
        {
            get
            {
                return transform.Find("EngineSounds").gameObject.GetComponent<BelugaEngineFMODEmitter>();
            }
        }


        public float GetPercentageOfHealth()
        {
            float health;
            float charge;

            GetHUDValues(out health, out charge);

            return health;
        }

        public float GetPercentageOfPower()
        {
            float charge;
            float capacity;

            GetEnergyValues(out charge, out capacity);

            float result = charge / capacity * 100f;

            return result;
        }
    }
}