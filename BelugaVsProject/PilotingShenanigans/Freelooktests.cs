

using System;
using UnityEngine;
using VehicleFramework;
using VehicleFramework.Engines;
using VehicleFramework.VehicleTypes;
using Nautilus.Utility;

namespace Beluga
{
    partial class BelugaEngine : ModVehicleEngine, IPlayerListener
    {
        private Player _player;
        
        private MainCameraControl _mcc;
        
        private Player player
        {
            get
            {
                bool flag = this._player == null;
                if (flag)
                {
                    this._player = base.GetComponent<Player>();
                }
                return this._player;
            }
        }
        private MainCameraControl mcc
        {
            get
            {
                
                
                


                    this._mcc = MainCameraControl.main;
                
                return this._mcc;
            }
        }

        private float xVelocity = 0.0f;
        private float yVelocity = 0.0f;
        const float smoothTime = 0.25f;

        public bool isFreeLooking = false;
        private bool wasFreelyPilotingLastFrame = false;
        private Quaternion savedCameraRotation;

        public void UpdateCallFreelook()
        {
            if (isFreeLooking)
            {
                UpdateFreelook();
            }
        }

        public void UpdateFreelook()
        {
            if (isFreeLooking)
            {
                float deadzone = 20f / 100f;
                bool triggerState = (Input.GetAxisRaw("ControllerAxis3") > deadzone) || (Input.GetAxisRaw("ControllerAxis3") < -deadzone);

                ExecuteFreeLook(Belugamanager.closestBeluga());

                if (triggerState && !wasFreelyPilotingLastFrame)
                {
                    StopFreelook();
                }

                wasFreelyPilotingLastFrame = triggerState;
            }
        }

        private void ExecuteFreeLook(Vehicle vehicle)
        {
            //Logger.Log("1");
            OxygenManager oxygenMgr = Player.main.oxygenMgr;
            //Logger.Log("2");
            oxygenMgr.AddOxygen(Time.deltaTime);
            //Logger.Log("3");
            MoveCamera();
            //Logger.Log("4");

           
        }

        public void BeginFreeLook()
        {
            if (!isFreeLooking)
            {
                
                isFreeLooking = true;
                savedCameraRotation = mcc.transform.localRotation;
                mcc.cinematicMode = true;
                mcc.rotationX = mcc.camRotationX;
                mcc.rotationY = mcc.camRotationY;
                mcc.transform.Find("camOffset/pdaCamPivot").localRotation = Quaternion.identity;
            }
        }

        public void StopFreelook()
        {
            if (isFreeLooking)
            {
                isFreeLooking = false;
                mcc.cinematicMode = false;
                mcc.transform.localRotation = savedCameraRotation;
                MainCamera.camera.transform.localEulerAngles = savedCameraRotation.eulerAngles;
            }
        }
        public Transform Camerastatesave = null;
        private void MoveCamera()
        {
            Vector2 myLookDelta = GameInput.GetLookDelta();
            if (myLookDelta == Vector2.zero)
            {
                myLookDelta.x -= GameInput.GetAnalogValueForButton(GameInput.Button.LookLeft);
                myLookDelta.x += GameInput.GetAnalogValueForButton(GameInput.Button.LookRight);
                myLookDelta.y += GameInput.GetAnalogValueForButton(GameInput.Button.LookUp);
                myLookDelta.y -= GameInput.GetAnalogValueForButton(GameInput.Button.LookDown);
            }
            mcc.rotationX += myLookDelta.x;
            mcc.rotationY += myLookDelta.y;
            mcc.rotationX = Mathf.Clamp(mcc.rotationX, -100, 100);
            mcc.rotationY = Mathf.Clamp(mcc.rotationY, -80, 80);
            
            MainCamera.camera.transform.localEulerAngles = new Vector3(-mcc.rotationY, mcc.rotationX, 0f);
        }
    }
}


