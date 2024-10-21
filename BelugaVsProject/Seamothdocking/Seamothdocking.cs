using Beluga.AudioShit;
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
    partial class Beluga


    {
        public Transform seamothdocked
        {
            get
            {
                return transform.Find("SeamothDock/seamothdocked");
            }
        }
        public Transform DockexitSeamoth

        {
            get
            {
                return transform.Find("SeamothDock/DockexitSeamoth");
            }
        }
        public Transform Seamothtrigger
        {
            get
            {
                return transform.Find("SeamothDock/Seamothtrigger");
            }
        }
        public bool seamothload = true;

        public void seamothdockupdate()
        {
            /*if (seamothload)
            {
                TryAttachSeamoth();
                    seamothload = false;
            }*/

            if (!IsPowered())
            {
                return;
            }
            if (currentSeaMoth == null)
            {
                if (Player.main.currentSub == null)
                {
                    TryAttachSeamoth();
                }
            }
            else
            {

                currentSeaMoth.transform.position = seamothdocked.position;
                currentSeaMoth.transform.rotation = seamothdocked.rotation;
                currentSeaMoth.liveMixin.shielded = true;
                //bloederroboter.collisionModel.SetActive(false);
                currentSeaMoth.useRigidbody.isKinematic = true;
                currentSeaMoth.crushDamage.enabled = false;
                //container.UpdateCollidersForDocking(true);
                currentSeaMoth.collisionModel.SetActive(false);
                currentSeaMoth.UpdateCollidersForDocking(true);
                currentSeaMoth.toggleLights.SetLightsActive(false);
                if (!detachflag2)
                {
                    targetDockFrontDoors = false;
                }


            }




        }
        public SeaMoth currentSeaMoth;
        public SeaMoth quickStep;

        public bool detachflag2 = false;


        public void TryAttachSeamoth()
        {


            if (currentSeaMoth != null)
            {
                return;
            }

            SeaMoth container = SeaMothmanager.main.FindNearestSeaMoth(seamothdocked.transform.position);
            if (container == null) { return; }

            if (Vector3.Distance(Seamothtrigger.position, container.transform.position) > 6 && detachflag2 == true)
            {

                detachflag2 = false;

                GetComponent<TetherSource>().isLive = true;

            }



            if (detachflag2 == true)
            {
                return;

            }

            if (!ValidateAttachment(container))
            {

                return;
            }

            AttachContainer(container);


        }
        public bool ValidateAttachment(SeaMoth container)
        {

            if (container is null)
            {
                return false;
            }

            if (Vector3.Distance(seamothdocked.position, container.transform.position) < 20 && detachflag2 == false)
            {
                targetDockFrontDoors = true;
            }
            if (Vector3.Distance(seamothdocked.position, container.transform.position) > 20 && detachflag2 == false)
            {
                targetDockFrontDoors = false;

            }

            if (Vector3.Distance(Seamothtrigger.position, container.transform.position) < 5 && detachflag2 == false)
            {
                return true;
            }


            return false;



        }
        private bool hasStarted2 = false;
        public void AttachContainer(SeaMoth container)
        {


            //container.transform.localPosition = seamothdocked.position;
            //container.transform.localRotation = seamothdocked.rotation;

            container.liveMixin.shielded = true;
            //bloederroboter.collisionModel.SetActive(false);
            container.useRigidbody.isKinematic = true;
            container.crushDamage.enabled = false;
            //container.UpdateCollidersForDocking(true);
            container.collisionModel.SetActive(false);
            container.UpdateCollidersForDocking(true);
            container.toggleLights.SetLightsActive(false);
            quickStep = container;
            
            if (!hasStarted2)
            {
                hasStarted2 = true;
                StartCoroutine(Seamothdocking(container, Seamothtrigger, seamothdocked, 2.5f, 1.6f));
            }
            






        }
        public void AttachcontainerwithoutAnimation(SeaMoth container) 
        {
            container.liveMixin.shielded = true;
            //bloederroboter.collisionModel.SetActive(false);
            container.useRigidbody.isKinematic = true;
            container.crushDamage.enabled = false;
            //container.UpdateCollidersForDocking(true);
            container.collisionModel.SetActive(false);
            container.UpdateCollidersForDocking(true);
            container.toggleLights.SetLightsActive(false);
            container.transform.position = seamothdocked.transform.position;
            currentSeaMoth = container;

        }

        public IEnumerator Seamothdocking(Vehicle objectToMove, Transform firstTarget, Transform secondTarget, float duration, float duration2)
        {


            // Move and rotate to the first target
            yield return StartCoroutine(MoveAndRotate(objectToMove, firstTarget, duration));

            // After reaching the first target, move and rotate to the second target
            yield return StartCoroutine(MoveAndRotate(objectToMove, secondTarget, duration2));

            // Once the second movement is done, log that the operation is finished

            
                currentSeaMoth = quickStep;

                hasStarted2 = false;
                if (Player.main.inSeamoth)
                {
                    Player.main.rigidBody.velocity = Vector3.zero;

                    Player.main.ToNormalMode(false);
                    Player.main.rigidBody.angularVelocity = Vector3.zero;
                    Player.main.ExitLockedMode(false, false);
                    Player.main.SetPosition(DockexitSeamoth.position);
                    Player.main.ExitSittingMode();
                    InstantDockBackDoors(false);
                    InstantHatchDoors(false);
                    
                    PlayerEntry();
                    Player.main.SetPosition(DockexitSeamoth.position);
                    TeleportPlayer(DockexitSeamoth.position);
                }
            
        }
        SeaMoth savedseamoth = null;

        
        public void TryDetachSeamoth()
        {
            if (currentSeaMoth == null)
            {
                return;
            }
            /*string msg = "Faileduifhae9oiwfhieuoshfoierhio";
            Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
            message.ShowMessage(msg, 200 * Time.deltaTime);*/
            /*currentMount.transform.SetParent(null);
            LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(currentMount);*/
            GetComponent<TetherSource>().isLive = false;
            InstantDockFrontDoors(true);
            InstantHatchDoors(false);
            savedseamoth = currentSeaMoth; currentSeaMoth = null;
            detachflag2 = true;
            Logger.Log("1");
            //currentMount.collisionModel.SetActive(true);
            Logger.Log("2");
            
            Logger.Log("3");
            savedseamoth.liveMixin.shielded = true;
            Logger.Log("4");
            savedseamoth.useRigidbody.velocity = Vector3.zero;
            Logger.Log("5");
            
            
            savedseamoth.useRigidbody.isKinematic = false;
            Logger.Log("6");
            
            
            Logger.Log("8");

            //UWE.CoroutineHost.StartCoroutine(EnterThenAddForceSeamoth(new Vector3(0f, -1000f, 0f)));
            UWE.CoroutineHost.StartCoroutine(EnterThenAddForceSeamoth(new Vector3(0f, -10f, 0f)));
        }

        public IEnumerator EnterThenAddForceSeamoth(Vector3 force)
        {
            seamothHatchDoor.transform.localRotation = Quaternion.Euler(40f, 180f, 180f);



            savedseamoth.EnterVehicle(Player.main, true, true);

            yield return new WaitForSeconds(2f);
            StartCoroutine(MoveAndRotate(savedseamoth, Seamothtrigger, 2f));
            yield return new WaitForSeconds(0.5f);

            BelugaUtils.PlayFMODSound("undock", savedseamoth.transform);
            savedseamoth.crushDamage.enabled = true;
            Logger.Log("7");
            savedseamoth.collisionModel.SetActive(true);
            savedseamoth.UpdateCollidersForDocking(false);
            setCollidersFrontDockDoors(false);
            savedseamoth.transform.position = Seamothtrigger.transform.position;
            savedseamoth.useRigidbody.AddRelativeForce(force, ForceMode.VelocityChange);
            setCollidersFrontDockDoors(true);

            seamothHatchDoor.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);

            currentSeaMoth = null;

            yield return null;
        }


        public void ShowAttachmentStatusSeamoth()
        {
            SeaMoth container = SeaMothmanager.main.FindNearestSeaMoth(seamothdocked.transform.position);
            if (container is null)
            {
                /*string msg = "Failed";
                Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
                message.ShowMessage(msg, 2 * Time.deltaTime);*/
                return;
            }
            float distance = Vector3.Distance(container.transform.position, seamothdocked.position);
            if (distance > 10000)
            {
                return;
            }
            string distanceString = distance.ToString();
            if (distance > 5)
            {
                /*string msg = "Container is " + distanceString + " meters away.";
                Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
                message.ShowMessage(msg, 2 * Time.deltaTime);*/
            }
            else if (!ValidateAttachment(container))
            {

            }


        }


        public void TryDetachSeamothWithoutPlayer()
        {
            if (currentSeaMoth == null)
            {
                return;
            }
            /*string msg = "Faileduifhae9oiwfhieuoshfoierhio";
            Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
            message.ShowMessage(msg, 200 * Time.deltaTime);*/
            /*currentMount.transform.SetParent(null);
            LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(currentMount);*/
            GetComponent<TetherSource>().isLive = false;
            InstantDockFrontDoors(true);
            InstantHatchDoors(false);
            savedseamoth = currentSeaMoth; currentSeaMoth = null;
            detachflag2 = true;
            Logger.Log("1");
            //currentMount.collisionModel.SetActive(true);
            Logger.Log("2");

            Logger.Log("3");
            savedseamoth.liveMixin.shielded = true;
            Logger.Log("4");
            savedseamoth.useRigidbody.velocity = Vector3.zero;
            Logger.Log("5");
            StartCoroutine(MoveAndRotate(savedseamoth, Seamothtrigger, 0.7f));
            
            savedseamoth.useRigidbody.isKinematic = false;
            Logger.Log("6");
            savedseamoth.crushDamage.enabled = true;
            Logger.Log("7");
            savedseamoth.collisionModel.SetActive(true);
            savedseamoth.UpdateCollidersForDocking(false);

            Logger.Log("8");


            seamothHatchDoor.transform.localRotation = Quaternion.Euler(40f, 180f, 180f);

            BelugaUtils.PlayFMODSound("undock", savedseamoth.transform);

            setCollidersFrontDockDoors(false);
            savedseamoth.transform.position = Seamothtrigger.transform.position;
            Vector3 force = new Vector3(0f, -30f, 0f);
            savedseamoth.useRigidbody.AddRelativeForce(force, ForceMode.VelocityChange);
            setCollidersFrontDockDoors(true);

            seamothHatchDoor.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);

            currentSeaMoth = null;
        }
    }
}
