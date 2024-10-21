using Beluga.AudioShit;
using Nautilus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework.VehicleTypes;

namespace Beluga
{
    public partial class Beluga
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            AnimateHatchDoors();
            AnimateDockFrontDoors();
            AnimateDockBackDoors();
            shieldupdate();
            
        }

        public override void Update()
        {
            base.Update();

            prawndockupdate();
            seamothdockupdate();

            CheckUIForActiveness();
            
            CheckHatchDistance();

            CheckEngineForEffects();

            gameObject.GetComponent<BelugaVoicelineManager>().CheckForVoicelines();
        }

        public void CheckHatchDistance()
        {
            // if dead don't
            if (isScuttled) return;


            float distanceToPlayer = Vector3.Distance(Player.main.transform.position, transform.Find("Hatches/BottomHatch").position);

            if (distanceToPlayer < 10f && Player.main.IsUnderwaterForSwimming() && !Player.main.IsInBase())
            {
                targetHatchDoors = true;
            }
            else
            {
                targetHatchDoors = false;
            }
        }
        public override void BeginPiloting()
        {

            base.BeginPiloting();
            
        }
        public void CheckEngineForEffects()
        {
            if (isScuttled)
            {
                PropCannonBeamFX Emitter1 = transform.Find("Model/BeamProjector1Int").gameObject.EnsureComponent<PropCannonBeamFX>();
                PropCannonBeamFX Emitter2 = transform.Find("Model/BeamProjector2Int").gameObject.EnsureComponent<PropCannonBeamFX>();
                PropCannonBeamFX Emitter3 = transform.Find("Model/BeamProjector3Int").gameObject.EnsureComponent<PropCannonBeamFX>();
                PropCannonBeamFX Emitter4 = transform.Find("Model/BeamProjector4Int").gameObject.EnsureComponent<PropCannonBeamFX>();
                PropCannonBeamFX Emitter5 = transform.Find("Model/BeamProjector5Int").gameObject.EnsureComponent<PropCannonBeamFX>();
                PropCannonBeamFX Emitter6 = transform.Find("Model/BeamProjector6Int").gameObject.EnsureComponent<PropCannonBeamFX>();
                PropCannonBeamFX Emitter7 = transform.Find("Model/BeamProjector7Int").gameObject.EnsureComponent<PropCannonBeamFX>();
                PropCannonBeamFX Emitter8 = transform.Find("Model/BeamProjector8Int").gameObject.EnsureComponent<PropCannonBeamFX>();

                Emitter1.SetGravityBeam(null);
                Emitter2.SetGravityBeam(null);
                Emitter3.SetGravityBeam(null);
                Emitter4.SetGravityBeam(null);
                Emitter5.SetGravityBeam(null);
                Emitter6.SetGravityBeam(null);
                Emitter7.SetGravityBeam(null);
                Emitter8.SetGravityBeam(null);

                return;
            };

            var engine = gameObject.GetComponent<BelugaEngine>();

            if (!engine) return;

            RotateEngine(engine.engineActive);
        }
    }
}
