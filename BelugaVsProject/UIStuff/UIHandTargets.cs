using Beluga.AudioShit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;

namespace Beluga
{
    public class InteriorLightsHandTarget : HandTarget, IHandTarget
    {
        
        public void OnHandClick(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                
                    if (beluga.GetComponent<LightingController>().state == LightingController.LightingState.Operational)
                    {
                        beluga.GetComponent<LightingController>().state = LightingController.LightingState.Damaged;
                        BelugaUtils.PlayFMODSound("buttonon", Player.main.transform);
                    }
                    else if (beluga.GetComponent<LightingController>().state == LightingController.LightingState.Damaged)
                    {
                        beluga.GetComponent<LightingController>().state = LightingController.LightingState.Operational;
                        BelugaUtils.PlayFMODSound("buttonoff", Player.main.transform);


                    }
                
               
            }


        }

        

        public void OnHandHover(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("ToggleIntLights");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
        }
    }

    public class ExteriorLightsHandTarget : HandTarget, IHandTarget
    {
        public void OnHandClick(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                beluga.GetComponent<HeadLightsController>().ToggleHeadlights();
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("ToggleExtLights");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
        }
    }

    public class EngineControlHandTarget : HandTarget, IHandTarget
    {
        public void OnHandClick(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            var belugaEngine = beluga.GetComponent<BelugaEngine>();
            var belugaComponent = beluga.GetComponent<Beluga>();
            var engineAudio = beluga.FindChild("EngineSounds").GetComponent<BelugaEngineFMODEmitter>();
            var belugaAudio = beluga.GetComponent<BelugaVoicelineManager>();
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                BelugaUtils.PlayFMODSound("buttonon", Player.main.transform);

                belugaEngine.engineActive = !belugaEngine.engineActive;
                if (belugaEngine.engineActive)
                {
                    UWE.CoroutineHost.StartCoroutine(belugaComponent.GraduallyChangeUIEngineTurbineRotationSpeed(3f, 700f));
                    belugaComponent.AddPropCannonBeamStuff(true);
                    belugaAudio.AddVoicelineToQueue("EnginePowerUp");
                    engineAudio.engineOn = true;
                    MainCameraControl.main.ShakeCamera(0.5f, 4f, MainCameraControl.ShakeMode.Sqrt);
                }
                else
                {
                    UWE.CoroutineHost.StartCoroutine(belugaComponent.GraduallyChangeUIEngineTurbineRotationSpeed(2f, 0f));
                    belugaComponent.AddPropCannonBeamStuff(false);
                    belugaAudio.AddVoicelineToQueue("EnginePowerDown");
                    engineAudio.engineOn = false;
                }
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("ToggleEngine");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
        }
    }

    public class EngineSpeedSlowHandTarget : HandTarget, IHandTarget
    {
        public void OnHandClick(GUIHand hand)
        {
            var belugaEngine = transform.parent.parent.gameObject.GetComponent<BelugaEngine>();
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;


            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                belugaEngine.engineSpeed = 1;
                BelugaUtils.PlayFMODSound("buttonon", Player.main.transform);
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("LowEngineSpeed");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
        }
    }

    public class EngineSpeedNormalHandTarget : HandTarget, IHandTarget
    {
        public void OnHandClick(GUIHand hand)
        {
            var belugaEngine = transform.parent.parent.gameObject.GetComponent<BelugaEngine>();
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                belugaEngine.engineSpeed = 2;
                BelugaUtils.PlayFMODSound("buttonon", Player.main.transform);
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("NormalEngineSpeed");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
        }
    }

    public class EngineSpeedFastHandTarget : HandTarget, IHandTarget
    {
        public void OnHandClick(GUIHand hand)
        {
            var belugaEngine = transform.parent.parent.gameObject.GetComponent<BelugaEngine>();
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                belugaEngine.engineSpeed = 3;
                BelugaUtils.PlayFMODSound("buttonon", Player.main.transform);
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            // IME = I Must Escape mode
            string displayString = Language.main.Get("HighEngineSpeed");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
        }
    }

    public class AgilityControlHandTarget : HandTarget, IHandTarget
    {
        public void OnHandClick(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            var belugaEngine = beluga.GetComponent<BelugaEngine>();
            var belugaAudio = beluga.GetComponent<BelugaVoicelineManager>();
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                BelugaUtils.PlayFMODSound("buttonon", Player.main.transform);

                belugaEngine.agilityControls = !belugaEngine.agilityControls;

                if (belugaEngine.agilityControls)
                {
                    belugaAudio.AddVoicelineToQueue("AgilityOn");
                    if (!Belugamanager.agilityflag)
                    {
                        belugaAudio.AddVoicelineToQueue("FirstAgility");
                        Belugamanager.agilityflag = true;

                    }
                }
                else
                {
                    belugaAudio.AddVoicelineToQueue("AgilityOff");
                }
                
                BelugaEngine.crosshairPos = Vector2.zero;
            }
        }

        public void OnHandHover(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("ToggleAgility");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
        }
    }

    public class ShieldControlHandTarget : HandTarget, IHandTarget
    {
        public void OnHandClick(GUIHand hand)
        {
            BelugaUtils.PlayFMODSound("buttonon", Player.main.transform);

            var beluga = Belugamanager.main.closestBeluga();
            if (beluga.shielded) 
            {
                beluga.disableshield();

            } 
            else 
            { 
                beluga.enableshield();
            
            }
            
            
            
        }

        public void OnHandHover(GUIHand hand)
        {
            var beluga = transform.parent.parent.gameObject;
            if (beluga.GetComponent<Beluga>().isScuttled) return;

            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("ToggleShield");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
        }
    }


}
