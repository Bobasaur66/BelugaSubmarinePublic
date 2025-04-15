using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    public partial class Beluga
    {
        // Seamoth dock hatch door
        public GameObject seamothHatchDoor = model.transform.Find("Model/FrontDockHatchInt").gameObject;

        // Front lower hatch doors

        public bool hatchDoorsOpen = false;
        public bool targetHatchDoors = false;
        public float hatchDoorsDuration = 0.5f;

        public GameObject hatchDoorLeft = model.transform.Find("Model/ExitDoorLExt").gameObject;
        public GameObject hatchDoorRight = model.transform.Find("Model/ExitDoorRExt").gameObject;

        public Quaternion hatchDoorRightOpenRotation = Quaternion.Euler(0f, 270f, 90f);
        public Quaternion hatchDoorLeftOpenRotation = Quaternion.Euler(0f, 90f, 270f);
        public Quaternion hatchDoorsClosedRotation = Quaternion.Euler(270f, 0f, 0f);

        public Coroutine hatchDoorsCoroutine;
        public void AnimateHatchDoors()
        {
            // if dead don't
            if (isScuttled) return;

            if (targetHatchDoors != hatchDoorsOpen)
            {
                hatchDoorsOpen = targetHatchDoors;

                if (hatchDoorsCoroutine != null)
                {
                    UWE.CoroutineHost.StopCoroutine(hatchDoorsCoroutine);
                }
                hatchDoorsCoroutine = UWE.CoroutineHost.StartCoroutine(RotateHatchDoors(hatchDoorsOpen));
            }
        }
        public IEnumerator RotateHatchDoors(bool open)
        {
            Quaternion targetRotationRight = open ? hatchDoorRightOpenRotation : hatchDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? hatchDoorLeftOpenRotation : hatchDoorsClosedRotation;

            Quaternion startRotationRight = hatchDoorRight.transform.localRotation;
            Quaternion startRotationLeft = hatchDoorLeft.transform.localRotation;

            if (this.hatchDoorsOpen)
            {
                Transform pos = this.transform.Find("Hatches/BottomHatch/OutsideHatch");
                BelugaUtils.PlayFMODSound("cyclops_door_open", pos);
            }
            else
            {
                Transform pos = this.transform.Find("Hatches/BottomHatch/OutsideHatch");
                BelugaUtils.PlayFMODSound("cyclops_door_close", pos);
            }

            float timeElapsed = 0f;

            while (timeElapsed < hatchDoorsDuration)
            {
                hatchDoorRight.transform.localRotation = Quaternion.Lerp(startRotationRight, targetRotationRight, timeElapsed / hatchDoorsDuration);
                hatchDoorLeft.transform.localRotation = Quaternion.Lerp(startRotationLeft, targetRotationLeft, timeElapsed / hatchDoorsDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // just to make sure
            hatchDoorRight.transform.localRotation = targetRotationRight;
            hatchDoorLeft.transform.localRotation = targetRotationLeft;
        }
        public void InstantHatchDoors(bool open)
        {
            Quaternion targetRotationRight = open ? hatchDoorRightOpenRotation : hatchDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? hatchDoorLeftOpenRotation : hatchDoorsClosedRotation;

            hatchDoorRight.transform.localRotation = targetRotationRight;
            hatchDoorLeft.transform.localRotation = targetRotationLeft;

            targetHatchDoors = open;
            hatchDoorsOpen = open;
        }

        // front dock doors

        public bool dockFrontDoorsOpen = false;
        public bool targetDockFrontDoors = false;
        public float dockFrontDoorsDuration = 0.5f;

        public GameObject dockFrontDoorLeftInt = model.transform.Find("Model/DockDoorFrontLInt").gameObject;
        public GameObject dockFrontDoorRightInt = model.transform.Find("Model/DockDoorFrontRInt").gameObject;
        public GameObject dockFrontDoorLeftExt = model.transform.Find("Model/DockDoorFrontLExt").gameObject;
        public GameObject dockFrontDoorRightExt = model.transform.Find("Model/DockDoorFrontRExt").gameObject;

        public Quaternion dockFrontDoorRightOpenRotation = Quaternion.Euler(0f, 270f, 90f);
        public Quaternion dockFrontDoorLeftOpenRotation = Quaternion.Euler(0f, 90f, 270f);
        public Quaternion dockFrontDoorsClosedRotation = Quaternion.Euler(270f, 0f, 0f);

        public Coroutine dockFrontDoorsCoroutine;

        public void AnimateDockFrontDoors()
        {
            // if dead don't
            if (isScuttled) return;

            if (targetDockFrontDoors != dockFrontDoorsOpen)
            {
                dockFrontDoorsOpen = !dockFrontDoorsOpen;

                if (dockFrontDoorsCoroutine != null)
                {
                    UWE.CoroutineHost.StopCoroutine(dockFrontDoorsCoroutine);
                }
                dockFrontDoorsCoroutine = UWE.CoroutineHost.StartCoroutine(RotateDockFrontDoors(dockFrontDoorsOpen));
            }
        }
        public IEnumerator RotateDockFrontDoors(bool open)
        {
            Quaternion targetRotationRight = open ? dockFrontDoorRightOpenRotation : dockFrontDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? dockFrontDoorLeftOpenRotation : dockFrontDoorsClosedRotation;

            Quaternion startRotationRight = dockFrontDoorRightInt.transform.localRotation;
            Quaternion startRotationLeft = dockFrontDoorLeftInt.transform.localRotation;

            if (this.dockFrontDoorsOpen)
            {
                Transform pos = this.transform.Find("SeamothDock/Seamothtrigger");
                BelugaUtils.PlayFMODSound("docking_doors_open", pos);
            }
            else
            {
                Transform pos = this.transform.Find("SeamothDock/Seamothtrigger");
                BelugaUtils.PlayFMODSound("docking_doors_close", pos);
            }

            float timeElapsed = 0f;

            while (timeElapsed < dockFrontDoorsDuration)
            {
                dockFrontDoorRightInt.transform.localRotation = Quaternion.Lerp(startRotationRight, targetRotationRight, timeElapsed / dockFrontDoorsDuration);
                dockFrontDoorLeftInt.transform.localRotation = Quaternion.Lerp(startRotationLeft, targetRotationLeft, timeElapsed / dockFrontDoorsDuration);
                dockFrontDoorLeftExt.transform.localRotation = Quaternion.Lerp(startRotationLeft, targetRotationLeft, timeElapsed / dockFrontDoorsDuration);
                dockFrontDoorRightExt.transform.localRotation = Quaternion.Lerp(startRotationRight, targetRotationRight, timeElapsed / dockFrontDoorsDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // just to make sure
            dockFrontDoorRightInt.transform.localRotation = targetRotationRight;
            dockFrontDoorRightExt.transform.localRotation = targetRotationRight;
            dockFrontDoorLeftInt.transform.localRotation = targetRotationLeft;
            dockFrontDoorLeftExt.transform.localRotation = targetRotationLeft;
        }
        public void InstantDockFrontDoors(bool open)
        {
            Quaternion targetRotationRight = open ? dockFrontDoorRightOpenRotation : dockFrontDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? dockFrontDoorLeftOpenRotation : dockFrontDoorsClosedRotation;

            dockFrontDoorRightInt.transform.localRotation = targetRotationRight;
            dockFrontDoorLeftInt.transform.localRotation = targetRotationLeft;
            dockFrontDoorRightExt.transform.localRotation = targetRotationRight;
            dockFrontDoorLeftExt.transform.localRotation = targetRotationLeft;

            targetDockFrontDoors = open;
            dockFrontDoorsOpen = open;
        }
        public void setCollidersFrontDockDoors(bool active)
        {
            dockFrontDoorLeftExt.FindChild("Collider").SetActive(active);
            dockFrontDoorRightExt.FindChild("Collider").SetActive(active);
        }

        // back dock doors
        public bool dockBackDoorsOpen = false;
        public bool targetDockBackDoors = false;
        public float dockBackDoorsDuration = 0.5f;

        public GameObject dockBackDoorLeftInt = model.transform.Find("Model/DockDoorBackLInt").gameObject;
        public GameObject dockBackDoorRightInt = model.transform.Find("Model/DockDoorBackRInt").gameObject;
        public GameObject dockBackDoorLeftExt = model.transform.Find("Model/DockDoorBackLExt").gameObject;
        public GameObject dockBackDoorRightExt = model.transform.Find("Model/DockDoorBackRExt").gameObject;

        public Quaternion dockBackDoorRightOpenRotation = Quaternion.Euler(0f, 270f, 90f);
        public Quaternion dockBackDoorLeftOpenRotation = Quaternion.Euler(0f, 90f, 270f);
        public Quaternion dockBackDoorsClosedRotation = Quaternion.Euler(270f, 0f, 0f);

        public Coroutine dockBackDoorsCoroutine;
        public void AnimateDockBackDoors()
        {
            // if dead don't
            if (isScuttled) return;


            if (targetDockBackDoors != dockBackDoorsOpen)
            {
                dockBackDoorsOpen = !dockBackDoorsOpen;

                if (dockBackDoorsCoroutine != null)
                {
                    UWE.CoroutineHost.StopCoroutine(dockBackDoorsCoroutine);
                }
                dockBackDoorsCoroutine = UWE.CoroutineHost.StartCoroutine(RotateDockBackDoors(dockBackDoorsOpen));
            }
        }
        public IEnumerator RotateDockBackDoors(bool open)
        {
            Quaternion targetRotationRight = open ? dockBackDoorRightOpenRotation : dockBackDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? dockBackDoorLeftOpenRotation : dockBackDoorsClosedRotation;

            Quaternion startRotationRight = dockBackDoorRightInt.transform.localRotation;
            Quaternion startRotationLeft = dockBackDoorLeftInt.transform.localRotation;

            if (this.dockBackDoorsOpen)
            {
                Transform pos = this.transform.Find("Prawndock/Prawntrigger");
                BelugaUtils.PlayFMODSound("docking_doors_open", pos);
            }
            else
            {
                Transform pos = this.transform.Find("Prawndock/Prawntrigger");
                BelugaUtils.PlayFMODSound("docking_doors_close", pos);
            }

            float timeElapsed = 0f;

            while (timeElapsed < dockBackDoorsDuration)
            {
                dockBackDoorRightInt.transform.localRotation = Quaternion.Lerp(startRotationRight, targetRotationRight, timeElapsed / dockBackDoorsDuration);
                dockBackDoorLeftInt.transform.localRotation = Quaternion.Lerp(startRotationLeft, targetRotationLeft, timeElapsed / dockBackDoorsDuration);
                dockBackDoorLeftExt.transform.localRotation = Quaternion.Lerp(startRotationLeft, targetRotationLeft, timeElapsed / dockBackDoorsDuration);
                dockBackDoorRightExt.transform.localRotation = Quaternion.Lerp(startRotationRight, targetRotationRight, timeElapsed / dockBackDoorsDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // just to make sure
            dockBackDoorRightInt.transform.localRotation = targetRotationRight;
            dockBackDoorRightExt.transform.localRotation = targetRotationRight;
            dockBackDoorLeftInt.transform.localRotation = targetRotationLeft;
            dockBackDoorLeftExt.transform.localRotation = targetRotationLeft;
        }
        public void InstantDockBackDoors(bool open)
        {
            Quaternion targetRotationRight = open ? dockBackDoorRightOpenRotation : dockBackDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? dockBackDoorLeftOpenRotation : dockBackDoorsClosedRotation;

            dockBackDoorRightInt.transform.localRotation = targetRotationRight;
            dockBackDoorLeftInt.transform.localRotation = targetRotationLeft;
            dockBackDoorRightExt.transform.localRotation = targetRotationRight;
            dockBackDoorLeftExt.transform.localRotation = targetRotationLeft;

            targetDockBackDoors = open;
            dockBackDoorsOpen = open;
        }
        public void setCollidersBackDockDoors(bool active)
        {
            dockBackDoorLeftExt.FindChild("Collider").SetActive(active);
            dockBackDoorRightExt.FindChild("Collider").SetActive(active);
        }

        // engine rotation
        public void RotateEngine(bool rotate)
        {
            GameObject engine = transform.Find("Model/EngineInt").gameObject;

            var engineRotationSpeed = 2000f;

            if (rotate)
            {
                engine.transform.Rotate(new Vector3(0f, engineRotationSpeed * Time.deltaTime, 0f), Space.Self);
            }
        }
    }
}
