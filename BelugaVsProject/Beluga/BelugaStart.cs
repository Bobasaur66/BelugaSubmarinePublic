using Beluga.AudioShit;
using Beluga.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;
using VehicleFramework.VehicleComponents;
using Beluga.Components;

namespace Beluga
{
    public partial class Beluga
    {
        public override void Start()
        {
            base.Start();

            Belugamanager.main.RegisterBeluga(this);

            // mute vehicle framework's voice and engine so we can add our own
            voice.balance = 0f;
            BelugaEngine engine = gameObject.GetComponent<BelugaEngine>();
            
            engine.WhistleFactor = 0f;
            engine.HumFactor = 0f;
            VehicleFramework.VehicleComponents.SteeringWheel Wheel = Steeringwheel.EnsureComponent<SteeringWheel>();
            Wheel.maxSteeringWheelAngle = 300f;
            
            SetupHandTargetShit();
            
            setupcams();

            AddPropCannonBeamStuff(false);

            AssignUIGameObjects();

            HideWreckedModelAtStart();

            disableshield();

            unscrabbledocking();            
        }


        public void unscrabbledocking()
        {
            Prawnload = true;
            seamothload = false;

        }

        public static IEnumerator Register()
        {
            GetAssets();

            ModVehicle Beluga = model.EnsureComponent<Beluga>() as ModVehicle;
            // do not make this one localized because it's the id or something and it will change
            // the literal techtype and stuff
            Beluga.name = "Beluga";
            yield return UWE.CoroutineHost.StartCoroutine(VehicleRegistrar.RegisterVehicle(Beluga));

            ExplosionVFXManager explosionVFXManager = model.transform.Find("FXControl").gameObject.EnsureComponent<ExplosionVFXManager>();
            explosionVFXManager.Setup();

            (Beluga as Beluga).AssignUnaffectedLights();

            model.EnsureComponent<BelugaVoicelineManager>();

            model.EnsureComponent<BelugaSaveDataHandler>();

            model.FindChild("EngineSounds").EnsureComponent<BelugaEngineFMODEmitter>();

            TemperatureDamage tempdamg = model.EnsureComponent<TemperatureDamage>();
            tempdamg.minDamageTemperature = 1000f;

            DealDamageOnImpact ddoi = model.GetComponent<DealDamageOnImpact>();
            ddoi.capMirrorDamage = 10f;
            ddoi.mirroredSelfDamage = false;
            ddoi.mirroredSelfDamageFraction = 0.1f;
            ddoi.minDamageInterval = 0.5f;

            ApplyMarmosetUBERShader(model);

            BelugaSkyApplierAdder.AddSkyApplierComponents(model);

            BelugaSkyApplierManager belugaSkyApplierManager = model.GetComponent<BelugaSkyApplierManager>();

            belugaSkyApplierManager.SetSkyApplierRenderers(model);

            var thing = Instantiate(Beluga);
            thing.gameObject.SetActive(false);

            UWE.CoroutineHost.StartCoroutine((Beluga as Beluga).DoCyclopsReferenceStuff());
        }

        public override void Awake()
        {
            base.Awake();

            
        }

        public void SetupHandTargetShit()
        {
            var Ladder1 = transform.Find("Ladders/Frontleft/Top").gameObject.EnsureComponent<LadderTop>();
            var Ladder2 = transform.Find("Ladders/Frontleft/Bottom").gameObject.EnsureComponent<LadderBottom>();
            var Ladder3 = transform.Find("Ladders/Frontright/Top").gameObject.EnsureComponent<LadderTop>();
            var Ladder4 = transform.Find("Ladders/Frontright/Bottom").gameObject.EnsureComponent<LadderBottom>();
            var Ladder5 = transform.Find("Ladders/Top/Top").gameObject.EnsureComponent<LadderTop>();
            var Ladder6 = transform.Find("Ladders/Top/Bottom").gameObject.EnsureComponent<LadderBottom>();
            var Ladder7 = transform.Find("Ladders/Backleft/Top").gameObject.EnsureComponent<LadderTop>();
            var Ladder8 = transform.Find("Ladders/Backleft/Bottom").gameObject.EnsureComponent<LadderBottom>();
            var Ladder9 = transform.Find("Ladders/Backright/Top").gameObject.EnsureComponent<LadderTop>();
            var Ladder10 = transform.Find("Ladders/Backright/Bottom").gameObject.EnsureComponent<LadderBottom>();

            var prawnhandtarget = transform.Find("Prawndock/prawndocked").gameObject.EnsureComponent<Prawnhandtarget>();
            var Seamothhandtarget = transform.Find("SeamothDock/seamothdocked").gameObject.EnsureComponent<SeaMothhandtarget>();

            var InteriorLightsTarget = transform.Find("UI/InteriorLighting").gameObject.EnsureComponent<InteriorLightsHandTarget>();
            var ExteriorLightsTarget = transform.Find("UI/ExteriorLighting").gameObject.EnsureComponent<ExteriorLightsHandTarget>();
            var EngineControlTarget = transform.Find("UI/EngineControl").gameObject.EnsureComponent<EngineControlHandTarget>();
            var EngineSpeedSlowTarget = transform.Find("UI/EngineSpeedSlow").gameObject.EnsureComponent<EngineSpeedSlowHandTarget>();
            var EngineSpeedNormalTarget = transform.Find("UI/EngineSpeedNormal").gameObject.EnsureComponent<EngineSpeedNormalHandTarget>();
            var EngineSpeedFastTarget = transform.Find("UI/EngineSpeedFast").gameObject.EnsureComponent<EngineSpeedFastHandTarget>();
            var AgilityControl = transform.Find("UI/AgilityControl").gameObject.EnsureComponent<AgilityControlHandTarget>();
            var ShieldControl = transform.Find("UI/ShieldControl").gameObject.EnsureComponent<ShieldControlHandTarget>();
        }

        public void AddPropCannonBeamStuff (bool active)
        {
            PropCannonBeamFX Emitter1 = transform.Find("Model/BeamProjector1Int").gameObject.EnsureComponent<PropCannonBeamFX>();
            PropCannonBeamFX Emitter2 = transform.Find("Model/BeamProjector2Int").gameObject.EnsureComponent<PropCannonBeamFX>();
            PropCannonBeamFX Emitter3 = transform.Find("Model/BeamProjector3Int").gameObject.EnsureComponent<PropCannonBeamFX>();
            PropCannonBeamFX Emitter4 = transform.Find("Model/BeamProjector4Int").gameObject.EnsureComponent<PropCannonBeamFX>();
            PropCannonBeamFX Emitter5 = transform.Find("Model/BeamProjector5Int").gameObject.EnsureComponent<PropCannonBeamFX>();
            PropCannonBeamFX Emitter6 = transform.Find("Model/BeamProjector6Int").gameObject.EnsureComponent<PropCannonBeamFX>();
            PropCannonBeamFX Emitter7 = transform.Find("Model/BeamProjector7Int").gameObject.EnsureComponent<PropCannonBeamFX>();
            PropCannonBeamFX Emitter8 = transform.Find("Model/BeamProjector8Int").gameObject.EnsureComponent<PropCannonBeamFX>();

            Transform target = transform.Find("Model/EngineInt");

            Emitter1.SetGravityBeam(active ? target : null);
            Emitter2.SetGravityBeam(active ? target : null);
            Emitter3.SetGravityBeam(active ? target : null);
            Emitter4.SetGravityBeam(active ? target : null);
            Emitter5.SetGravityBeam(active ? target : null);
            Emitter6.SetGravityBeam(active ? target : null);
            Emitter7.SetGravityBeam(active ? target : null);
            Emitter8.SetGravityBeam(active ? target : null);
        }

        public IEnumerator DoCyclopsReferenceStuff()
        {
            yield return UWE.CoroutineHost.StartCoroutine(CyclopsReferenceManager.EnsureCyclopsReferenceExists());

            gameObject.GetComponent<BelugaSkyApplierManager>().OnCyclopsReferenceFinished(CyclopsReferenceManager.CyclopsReference);
            transform.FindChild("FXControl").gameObject.GetComponent<ExplosionVFXManager>().OnCyclopsReferenceFinished(CyclopsReferenceManager.CyclopsReference);
            gameObject.GetComponent<Beluga>().OnCyclopsReferenceFinished(CyclopsReferenceManager.CyclopsReference);
            /*
            var components = GetComponentsInChildren<ICyclopsReferencer>();
            Logger.Log("Logging components found");
            foreach (var component in components)
            {
                Logger.Log(component.ToString());
            }
            foreach (var component in components)
            {
                component.OnCyclopsReferenceFinished(CyclopsReferenceManager.CyclopsReference);
            }*/
        }

        public void HideWreckedModelAtStart()
        {
            wreckedModel.SetActive(false);
        }

        public void AssignUnaffectedLights()
        {
            engineLight.EnsureComponent<ExcludeLightFromController>();
        }
    }
}
