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


            Beluga thisBeluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            if (thisBeluga.targetHatchDoors != thisBeluga.hatchDoorsOpen)
            {
                thisBeluga.hatchDoorsOpen = thisBeluga.targetHatchDoors;

                if (thisBeluga.hatchDoorsCoroutine != null)
                {
                    UWE.CoroutineHost.StopCoroutine(thisBeluga.hatchDoorsCoroutine);
                }
                thisBeluga.hatchDoorsCoroutine = UWE.CoroutineHost.StartCoroutine(thisBeluga.RotateHatchDoors(thisBeluga.hatchDoorsOpen));
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
            Beluga thisBeluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            Quaternion targetRotationRight = open ? thisBeluga.hatchDoorRightOpenRotation : thisBeluga.hatchDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? thisBeluga.hatchDoorLeftOpenRotation : thisBeluga.hatchDoorsClosedRotation;

            thisBeluga.hatchDoorRight.transform.localRotation = targetRotationRight;
            thisBeluga.hatchDoorLeft.transform.localRotation = targetRotationLeft;

            thisBeluga.targetHatchDoors = open;
            thisBeluga.hatchDoorsOpen = open;
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


            Beluga thisBeluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            if (thisBeluga.targetDockFrontDoors != thisBeluga.dockFrontDoorsOpen)
            {
                thisBeluga.dockFrontDoorsOpen = !thisBeluga.dockFrontDoorsOpen;

                if (thisBeluga.dockFrontDoorsCoroutine != null)
                {
                    UWE.CoroutineHost.StopCoroutine(thisBeluga.dockFrontDoorsCoroutine);
                }
                thisBeluga.dockFrontDoorsCoroutine = UWE.CoroutineHost.StartCoroutine(thisBeluga.RotateDockFrontDoors(thisBeluga.dockFrontDoorsOpen));
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
            Beluga thisBeluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            Quaternion targetRotationRight = open ? thisBeluga.dockFrontDoorRightOpenRotation : thisBeluga.dockFrontDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? thisBeluga.dockFrontDoorLeftOpenRotation : thisBeluga.dockFrontDoorsClosedRotation;

            thisBeluga.dockFrontDoorRightInt.transform.localRotation = targetRotationRight;
            thisBeluga.dockFrontDoorLeftInt.transform.localRotation = targetRotationLeft;
            thisBeluga.dockFrontDoorRightExt.transform.localRotation = targetRotationRight;
            thisBeluga.dockFrontDoorLeftExt.transform.localRotation = targetRotationLeft;

            thisBeluga.targetDockFrontDoors = open;
            thisBeluga.dockFrontDoorsOpen = open;
        }

        public void setCollidersFrontDockDoors(bool active)
        {
            Beluga thisBeluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            thisBeluga.dockFrontDoorLeftExt.FindChild("Collider").SetActive(active);
            thisBeluga.dockFrontDoorRightExt.FindChild("Collider").SetActive(active);
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


            Beluga thisBeluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            if (thisBeluga.targetDockBackDoors != thisBeluga.dockBackDoorsOpen)
            {
                thisBeluga.dockBackDoorsOpen = !thisBeluga.dockBackDoorsOpen;

                if (thisBeluga.dockBackDoorsCoroutine != null)
                {
                    UWE.CoroutineHost.StopCoroutine(thisBeluga.dockBackDoorsCoroutine);
                }
                thisBeluga.dockBackDoorsCoroutine = UWE.CoroutineHost.StartCoroutine(thisBeluga.RotateDockBackDoors(thisBeluga.dockBackDoorsOpen));
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
            Beluga thisBeluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            Quaternion targetRotationRight = open ? thisBeluga.dockBackDoorRightOpenRotation : thisBeluga.dockBackDoorsClosedRotation;
            Quaternion targetRotationLeft = open ? thisBeluga.dockBackDoorLeftOpenRotation : thisBeluga.dockBackDoorsClosedRotation;

            thisBeluga.dockBackDoorRightInt.transform.localRotation = targetRotationRight;
            thisBeluga.dockBackDoorLeftInt.transform.localRotation = targetRotationLeft;
            thisBeluga.dockBackDoorRightExt.transform.localRotation = targetRotationRight;
            thisBeluga.dockBackDoorLeftExt.transform.localRotation = targetRotationLeft;

            thisBeluga.targetDockBackDoors = open;
            thisBeluga.dockBackDoorsOpen = open;
        }

        public void setCollidersBackDockDoors(bool active)
        {
            Beluga thisBeluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            thisBeluga.dockBackDoorLeftExt.FindChild("Collider").SetActive(active);
            thisBeluga.dockBackDoorRightExt.FindChild("Collider").SetActive(active);
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
