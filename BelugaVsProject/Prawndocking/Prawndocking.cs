using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
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
        public Transform prawndocked
        {
            get
            {
                return transform.Find("Prawndock/prawndocked");
            }
        }
        public Transform DockexitPrawn
        {
            get
            {
                return transform.Find("Prawndock/DockexitPrawn");
            }
        }
        public Transform Prawntrigger
        {
            get
            {
                return transform.Find("Prawndock/Prawntrigger");
            }
        }

        public bool Prawnload = true;
        public int counter = 0;

        public void prawndockupdate() 
        {
            /*if (Prawnload)
            {
                TryAttachPrawn();
                Prawnload = false;
            }*/
            
            if (!IsPowered())
            {
                return;
            }
            if (currentprawn == null)
            {
                if (Player.main.currentSub == null)
                {
                    TryAttachPrawn();
                }
            }
            else 
            {

                currentprawn.transform.position = prawndocked.position;
                currentprawn.transform.rotation = prawndocked.rotation;
                currentprawn.liveMixin.shielded = true;
                //bloederroboter.collisionModel.SetActive(false);
                currentprawn.useRigidbody.isKinematic = true;
                currentprawn.crushDamage.enabled = false;
                currentprawn.UpdateCollidersForDocking(true);
                if (!detachflag)
                {
                    targetDockBackDoors = false;
                }

            }
            



        }
        public Exosuit quickstep;
        public Exosuit currentprawn;
        

        public bool detachflag = false;
        public int rotationcount = 0;

        public void TryAttachPrawn()
        {
            
            if (currentprawn != null)
            {
                return;
            }
            
            Exosuit container = PrawnManager.main.FindNearestPrawn(prawndocked.transform.position);
            
            if (container == null) { return; }
            
            if (Vector3.Distance(Prawntrigger.position, container.transform.position) > 6 && detachflag == true)
            {
                detachflag = false;
                GetComponent<TetherSource>().isLive = true;

            }
            
            if (detachflag == true) {
                return;
            
            }
            
            if (!ValidateAttachment(container))
            {

                return;
            }
            
            AttachContainer(container);


        }
        public bool ValidateAttachment(Exosuit container)
        {
            if (container is null)
            {
                return false;
            }
            
            if (Vector3.Distance(prawndocked.position, container.transform.position) < 20 && detachflag == false)
            {
                targetDockBackDoors = true;
            }
            if (Vector3.Distance(prawndocked.position, container.transform.position) > 20 && detachflag == false)
            {
                targetDockBackDoors = false;

            }
            
            if (Vector3.Distance(Prawntrigger.position, container.transform.position) < 5 && detachflag == false)
            {
                return true;
            }
            

            return false;



        }
        private bool hasStarted = false;
        public void AttachContainer(Exosuit Prawn)
        {

            
            //Prawn.transform.localPosition = prawndocked.position;
            //Prawn.transform.localRotation = prawndocked.rotation;

            Prawn.liveMixin.shielded = true;
            //bloederroboter.collisionModel.SetActive(false);
            Prawn.useRigidbody.isKinematic = true;
            Prawn.crushDamage.enabled = false;
            Prawn.UpdateCollidersForDocking(true);
            
            
            

            quickstep = Prawn;

            if (!hasStarted)
            {
                hasStarted = true;
                StartCoroutine(Prawndocking(Prawn, Prawntrigger, prawndocked, 2.5f, 1.7f));
            }
            







        }
        Exosuit savedprawn = null;
        public void TryDetachPrawn()
        {
            if (currentprawn == null)
            {
                return;
            }
            /*string msg = "Faileduifhae9oiwfhieuoshfoierhio";
            Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
            message.ShowMessage(msg, 200 * Time.deltaTime);*/
            /*currentMount.transform.SetParent(null);
            LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(currentMount);*/
            GetComponent<TetherSource>().isLive = false;
            InstantDockBackDoors(true);
            InstantHatchDoors(false);
            savedprawn = currentprawn; currentprawn = null;
            detachflag = true;
            Logger.Log("1");
            //currentMount.collisionModel.SetActive(true);
            Logger.Log("2");
            savedprawn.rotationDirty = true;
            Logger.Log("3");
            savedprawn.liveMixin.shielded = true;
            Logger.Log("4");
            savedprawn.useRigidbody.velocity = Vector3.zero;
            Logger.Log("5");
            savedprawn.EnterVehicle(Player.main, true, true);
            StartCoroutine(MoveAndRotate(savedprawn, Prawntrigger, 2f));
            savedprawn.useRigidbody.isKinematic = false;
            Logger.Log("6");
            savedprawn.crushDamage.enabled = true;
            Logger.Log("7");
            savedprawn.UpdateCollidersForDocking(true);

            UWE.CoroutineHost.StartCoroutine(EnterThenAddForceExosuit(new Vector3(0f, -10f, 0f)));
        }
        public IEnumerator MoveAndRotate(Vehicle objectToMove, Transform firstTarget, float duration)
        {
            // Get starting position and rotation
            Vector3 startPosition = objectToMove.transform.position;
            Quaternion startRotation = objectToMove.transform.rotation;

            // Get target position and rotation
            Vector3 midPosition = firstTarget.position;
            Quaternion midRotation = firstTarget.rotation;

            float elapsedTime = 0f;

            // Move and rotate over the given duration
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                // Interpolate position and rotation
                objectToMove.transform.position = Vector3.Lerp(startPosition, midPosition, elapsedTime / duration);
                objectToMove.transform.rotation = Quaternion.Slerp(startRotation, midRotation, elapsedTime / duration);

                yield return null; // Wait for the next frame
            }

            // Ensure the final position and rotation are exactly the target's
            objectToMove.transform.position = midPosition;
            objectToMove.transform.rotation = midRotation;


        }
        public IEnumerator Prawndocking(Vehicle objectToMove, Transform firstTarget, Transform secondTarget, float duration, float duration2)
        {
            

            // Move and rotate to the first target
            yield return StartCoroutine(MoveAndRotate(objectToMove, firstTarget, duration));

            // After reaching the first target, move and rotate to the second target
            yield return StartCoroutine(MoveAndRotate(objectToMove, secondTarget, duration2));

            // Once the second movement is done, log that the operation is finished

            
                currentprawn = quickstep;

            hasStarted = false;
                if (Player.main.inExosuit)
                {
                    Player.main.rigidBody.velocity = Vector3.zero;

                    Player.main.ToNormalMode(false);
                    Player.main.rigidBody.angularVelocity = Vector3.zero;
                    Player.main.ExitLockedMode(false, false);
                    Player.main.SetPosition(DockexitPrawn.position);
                    Player.main.ExitSittingMode();
                   
                Logger.Log("blah");
                    PlayerEntry();
                    Player.main.SetPosition(DockexitPrawn.position);
                    TeleportPlayer(DockexitPrawn.position);
                }
            
        }


        public IEnumerator EnterThenAddForceExosuit(Vector3 force)
        {

            savedprawn.EnterVehicle(Player.main, true, true);
            
            yield return new WaitForSeconds(2f);
            StartCoroutine(MoveAndRotate(savedprawn, Prawntrigger, 2f));
            yield return new WaitForSeconds(0.5f);
            BelugaUtils.PlayFMODSound("undock", savedprawn.transform);

            setCollidersBackDockDoors(false);
            savedprawn.UpdateCollidersForDocking(false);
            Logger.Log("8");
            
            savedprawn.useRigidbody.AddRelativeForce(force, ForceMode.VelocityChange);
            setCollidersBackDockDoors(true);

            currentprawn = null;

            yield return null;
        }


        public void ShowAttachmentStatus()
        {
            Exosuit container = PrawnManager.main.FindNearestPrawn(prawndocked.transform.position);
            if (container is null)
            {
                /*string msg = "Failed";
                Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
                message.ShowMessage(msg, 2 * Time.deltaTime);*/
                return;
            }
            float distance = Vector3.Distance(container.transform.position, prawndocked.position);
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


        public void TryDetachPrawnWithoutPlayer()
        {
            if (currentprawn == null)
            {
                return;
            }
            /*string msg = "Faileduifhae9oiwfhieuoshfoierhio";
            Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
            message.ShowMessage(msg, 200 * Time.deltaTime);*/
            /*currentMount.transform.SetParent(null);
            LargeWorldStreamer.main.cellManager.RegisterGlobalEntity(currentMount);*/
            GetComponent<TetherSource>().isLive = false;
            InstantDockBackDoors(true);
            InstantHatchDoors(false);
            savedseamoth = currentSeaMoth; currentSeaMoth = null;
            detachflag = true;
            Logger.Log("1");
            //currentMount.collisionModel.SetActive(true);
            Logger.Log("2");
            savedprawn.rotationDirty = true;
            Logger.Log("3");
            savedprawn.liveMixin.shielded = true;
            Logger.Log("4");
            savedprawn.useRigidbody.velocity = Vector3.zero;
            Logger.Log("5");
            StartCoroutine(MoveAndRotate(savedseamoth, Seamothtrigger, 0.7f));
            savedprawn.useRigidbody.isKinematic = false;
            Logger.Log("6");
            savedprawn.crushDamage.enabled = true;
            Logger.Log("7");


            BelugaUtils.PlayFMODSound("undock", savedprawn.transform);

            setCollidersBackDockDoors(false);
            savedprawn.UpdateCollidersForDocking(false);
            Logger.Log("8");
            savedprawn.transform.position = Prawntrigger.transform.position;
            Vector3 force = new Vector3(0f, -30f, 0f);
            savedprawn.useRigidbody.AddRelativeForce(force, ForceMode.VelocityChange);
            setCollidersBackDockDoors(true);

            currentprawn = null;
        }
    }
}
