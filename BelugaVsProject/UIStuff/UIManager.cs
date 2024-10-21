using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;

namespace Beluga
{
    public partial class Beluga
    {
        public GameObject UIInteriorLightsOn;
        public GameObject UIInteriorLightsOff;

        public GameObject UIExteriorLightsOn;
        public GameObject UIExteriorLightsOff;

        public GameObject UIEngineTurbine;
        public float UIEngineTurbineRotationSpeed = 0f;

        public GameObject UIAgilityControlOn;
        public GameObject UIAgilityControlOff;

        public GameObject UIEngineSpeedSlowOn;
        public GameObject UIEngineSpeedSlowOff;
        public GameObject UIEngineSpeedNormalOn;
        public GameObject UIEngineSpeedNormalOff;
        public GameObject UIEngineSpeedFastOn;
        public GameObject UIEngineSpeedFastOff;

        public void AssignUIGameObjects()
        {
            UIInteriorLightsOn = transform.Find("Model/HUDLightsInOn").gameObject;
            UIInteriorLightsOff = transform.Find("Model/HUDLightsInOff").gameObject;

            UIExteriorLightsOn = transform.Find("Model/HUDLightsOutOn").gameObject;
            UIExteriorLightsOff = transform.Find("Model/HUDLightsOutOff").gameObject;

            UIEngineTurbine = transform.Find("Model/HUDEngine").gameObject;

            UIAgilityControlOn = transform.Find("Model/HUDAgilityOn").gameObject;
            UIAgilityControlOff = transform.Find("Model/HUDAgilityOff").gameObject;

            UIEngineSpeedSlowOn = transform.Find("Model/HUDSpeedSlowOn").gameObject;
            UIEngineSpeedSlowOff = transform.Find("Model/HUDSpeedSlowOff").gameObject;
            UIEngineSpeedNormalOn = transform.Find("Model/HUDSpeedNormalOn").gameObject;
            UIEngineSpeedNormalOff = transform.Find("Model/HUDSpeedNormalOff").gameObject;
            UIEngineSpeedFastOn = transform.Find("Model/HUDSpeedFastOn").gameObject;
            UIEngineSpeedFastOff = transform.Find("Model/HUDSpeedFastOff").gameObject;
        }

        public void CheckUIForActiveness()
        {
            GameObject beluga = gameObject;

            // if dead deactiveate all ui
            // if dead don't rotate engine
            if (isScuttled)
            {
                UIInteriorLightsOn.SetActive(false);
                UIInteriorLightsOff.SetActive(false);
                UIExteriorLightsOn.SetActive(false);
                UIExteriorLightsOff.SetActive(false);
                UIEngineTurbine.SetActive(false);
                UIAgilityControlOn.SetActive(false);
                UIAgilityControlOff.SetActive(false);
                UIEngineSpeedSlowOn.SetActive(false);
                UIEngineSpeedSlowOff.SetActive(false);
                UIEngineSpeedNormalOn.SetActive(false);
                UIEngineSpeedNormalOff.SetActive(false);
                UIEngineSpeedFastOn.SetActive(false);
                UIEngineSpeedFastOff.SetActive(false);

                return;
            }

            // interior lighting
            if (beluga.GetComponent<LightingController>().state == LightingController.LightingState.Operational)
            {
                UIInteriorLightsOn.SetActive(true);
                UIInteriorLightsOff.SetActive(false);
            }
            else
            {
                UIInteriorLightsOn.SetActive(false);
                UIInteriorLightsOff.SetActive(true);
            }

            // exterior lighting
            if (beluga.GetComponent<HeadLightsController>().isHeadlightsOn)
            {
                UIExteriorLightsOn.SetActive(true);
                UIExteriorLightsOff.SetActive(false);
            }
            else
            {
                UIExteriorLightsOn.SetActive(false);
                UIExteriorLightsOff.SetActive(true);
            }

            // engine spinny thing
            if (beluga.GetComponent<Beluga>().isPoweredOn)
            {
                UIEngineTurbine.transform.Rotate(UIEngineTurbineRotationSpeed * Time.deltaTime, 0f, 0f, Space.Self);
            }

            // agility mode
            if (beluga.GetComponent<BelugaEngine>().agilityControls)
            {
                UIAgilityControlOn.SetActive(true);
                UIAgilityControlOff.SetActive(false);
            }
            else
            {
                UIAgilityControlOn.SetActive(false);
                UIAgilityControlOff.SetActive(true);
            }

            // engine speed
            if (beluga.GetComponent<BelugaEngine>().engineSpeed == 1)
            {
                UIEngineSpeedSlowOn.SetActive(true);
                UIEngineSpeedSlowOff.SetActive(false);
                UIEngineSpeedNormalOn.SetActive(false);
                UIEngineSpeedNormalOff.SetActive(true);
                UIEngineSpeedFastOn.SetActive(false);
                UIEngineSpeedFastOff.SetActive(true);
            }
            else if (beluga.GetComponent<BelugaEngine>().engineSpeed == 3)
            {
                UIEngineSpeedSlowOn.SetActive(false);
                UIEngineSpeedSlowOff.SetActive(true);
                UIEngineSpeedNormalOn.SetActive(false);
                UIEngineSpeedNormalOff.SetActive(true);
                UIEngineSpeedFastOn.SetActive(true);
                UIEngineSpeedFastOff.SetActive(false);
            }
            else
            {
                UIEngineSpeedSlowOn.SetActive(false);
                UIEngineSpeedSlowOff.SetActive(true);
                UIEngineSpeedNormalOn.SetActive(true);
                UIEngineSpeedNormalOff.SetActive(false);
                UIEngineSpeedFastOn.SetActive(false);
                UIEngineSpeedFastOff.SetActive(true);
            }
        }

        public IEnumerator GraduallyChangeUIEngineTurbineRotationSpeed(float timeToFull, float endRotationSpeed)
        {
            float startRotationSpeed = UIEngineTurbineRotationSpeed;

            float timeElapsed = 0f;

            while (timeElapsed < timeToFull)
            {
                UIEngineTurbineRotationSpeed = Mathf.Lerp(0f, endRotationSpeed, timeElapsed / timeToFull);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
