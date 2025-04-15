using Beluga.AudioShit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using VehicleFramework;

namespace Beluga
{
    public partial class Beluga

    {
        
        public VFXController vfxcontroller;

        public override void DestroyMV()
        {
            if (isScuttled)
            {
                return;
            }
            UWE.CoroutineHost.StartCoroutine(EpicDeathBehavior());
        }
        public void turnRed() 
        {
            foreach (GameObject Light in UnityLights)
            {
                GetComponent<LightingController>().state = LightingController.LightingState.Operational;
                Light.GetComponent<Light>().color = Color.red;

            }
        }
        public void turnOffLights()
        {
            foreach (GameObject Light in UnityLights)
            {
                GetComponent<LightingController>().state = LightingController.LightingState.Damaged;
                Light.SetActive(false);

            }
        }
        public IEnumerator EpicDeathBehavior()
        {
            string deathVoiceline = "AbandonShip";
            float voicelineLength = 1f;
            VoicelinesInfo.infoDic.TryGetValue(deathVoiceline, out voicelineLength);
            float targetRoll = 20f;
            turnRed();
            gameObject.GetComponent<BelugaVoicelineManager>().AddVoicelineToQueue(deathVoiceline);

            stabilizeRoll = false;

            yield return gameObject.GetComponent<BelugaEngine>().RollOverTime(targetRoll, voicelineLength);

            seamothBay.Detach(false);
            prawnBay.Detach(false);

            BelugaUtils.PlayFMODSound("explode", transform);

            float damageToPlayer = 0f;

            // Play explosion VFX
            if (Player.main.GetCurrentSub() == GetComponent<SubRoot>())
            {
                vfxcontroller.Play(0);
                damageToPlayer = 1000f;
            }
            else
            {
                vfxcontroller.Play(1);
                float distanceToPlayer = Vector3.Distance(Player.main.transform.position, transform.position);
                damageToPlayer = (50 - Mathf.Clamp(distanceToPlayer, 0f, 30f)) * 3;
            }

            engineSoundEmitter.engineOn = false;

            PlayerExit();
            ScuttleVehicle();

            gameObject.GetComponent<LightingController>().state = LightingController.LightingState.Damaged;

            //disable dock handtargets
            var seamothDock = transform.Find("SeamothDock").gameObject;
            var prawnDock = transform.Find("Prawndock").gameObject;
            seamothDock.SetActive(false);
            prawnDock.SetActive(false);

            // switch to destroyed model and colliders
            unwreckedModel.SetActive(false);
            unwreckedColliders.SetActive(false);
            wreckedModel.SetActive(true);

            engineLight.SetActive(false);

            Player.main.liveMixin.TakeDamage(damageToPlayer);

            yield return new WaitForSeconds(4);

            // sink
            worldForces.enabled = true;
            worldForces.handleGravity = true;
            worldForces.underwaterGravity = 4f;

            vfxcontroller.StopAndDestroy(0, 1f);
            minimap.GetComponent<MiniWorld>().DisableMap();
        }

        private IEnumerator waitWhileDestabilizingRoll(float time)
        {
            float currentTime = 0f;

            while (currentTime < time) 
            {
                stabilizeRoll = false;

                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        public override void ScuttleVehicle()
        {
            base.ScuttleVehicle();

            foreach (Transform child in transform.Find("Hatches"))
            {
                child.gameObject.SetActive(false);
            }

            pingInstance.enabled = false;
        }

        public void DestroyUIModels()
        {
            transform.Find("Model/HUDSlowSpeedInt").gameObject.SetActive(false);
            transform.Find("Model/HUDNormalSpeedInt").gameObject.SetActive(false);
            transform.Find("Model/HUDFastSpeedInt").gameObject.SetActive(false);
            transform.Find("Model/HUDEmptyInt").gameObject.SetActive(false);
            transform.Find("Model/HUDEngineToggleInt").gameObject.SetActive(false);
        }

        public void DestroyVehicleInstantly()
        {
            stabilizeRoll = false;

            seamothBay.Detach(false);
            prawnBay.Detach(false);

            engineSoundEmitter.engineOn = false;

            PlayerExit();
            ScuttleVehicle();

            gameObject.GetComponent<LightingController>().state = LightingController.LightingState.Damaged;

            //disable dock handtargets
            var seamothDock = transform.Find("SeamothDock").gameObject;
            var prawnDock = transform.Find("Prawndock").gameObject;
            seamothDock.SetActive(false);
            prawnDock.SetActive(false);

            // switch to destroyed model and colliders
            unwreckedModel.SetActive(false);
            unwreckedColliders.SetActive(false);
            wreckedModel.SetActive(true);

            engineLight.SetActive(false);

            turnOffLights();

            // sink
            worldForces.enabled = true;
            worldForces.handleGravity = true;
            worldForces.underwaterGravity = 4f;

            vfxcontroller.StopAndDestroy(0, 1f);
        }
    }
}
