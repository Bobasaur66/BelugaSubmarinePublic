using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;
using VehicleFramework.Engines;
using VehicleFramework.VehicleTypes;

namespace Beluga
{
    partial class BelugaEngine : ModVehicleEngine, IPlayerListener
    {
        // cyclops controls or advanced agility controls
        public bool agilityControls = false;

        public bool speedUpgradeInstalled = false;

        public int engineSpeed = 2;
        public bool engineActive = false;

        // turns by this amount on update so zero at start
        private float turnSpeedX = 0f;
        private float turnSpeedY = 0f;

        private float turnSpeedMaxX = 2f;
        private float turnSpeedMaxY = 2f;

        private float turnSpeedAccelX = 4f;
        private float turnSpeedAccelY = 4f;

        //subtracted from turn speed
        protected float turnSpeedDamping = 2f;


        // engine speeds
        // strafe acceleration of 0 means it can't go side to side like a seamoth
        protected override float FORWARD_TOP_SPEED
        {
            get
            {
                if (!engineActive)
                {
                    return 0;
                }
                if (engineSpeed == 1)
                {
                    if (speedUpgradeInstalled)
                    {
                        return 1000f;
                    }
                    else
                    {
                        return 500f;
                    }
                }
                else if (engineSpeed == 3)
                {
                    if (speedUpgradeInstalled)
                    {
                        return 2000f;
                    }
                    else
                    {
                        return 1500f;
                    }
                }
                else
                {
                    if (speedUpgradeInstalled)
                    {
                        return 1500f;
                    }
                    else
                    {
                        return 1000f;
                    }
                }
            }
        }
        protected override float STRAFE_MAX_SPEED => 0f;
        protected override float REVERSE_TOP_SPEED => FORWARD_TOP_SPEED / 2;
        protected override float VERT_MAX_SPEED => FORWARD_TOP_SPEED;

        protected override float FORWARD_ACCEL => FORWARD_TOP_SPEED / 4f;
        protected override float REVERSE_ACCEL => REVERSE_TOP_SPEED / 4f;
        protected override float STRAFE_ACCEL => STRAFE_MAX_SPEED / 4f;
        protected override float VERT_ACCEL => VERT_MAX_SPEED / 4f;

        public override bool CanMoveAboveWater => false;
        public override bool CanRotateAboveWater => false;


        // agility stuff
        public GameObject canvas;

        public static Vector2 crosshairPos = Vector2.zero;

        public override void Start()
        {
            base.Start();

            canvas = Instantiate(Beluga.crosshairCanvas);
            agilityControls = false;
            canvas.SetActive(false);

            
        }

        void IPlayerListener.OnPilotBegin()
        {
            crosshairPos = Vector2.zero;
            canvas.SetActive(false);
            
            if (!agilityControls)
            {
                BeginFreeLook();
            }
        }

        void IPlayerListener.OnPilotEnd()
        {
            crosshairPos = Vector2.zero;
            canvas.SetActive(false);


           StopFreelook();
            
            


        }

        void IPlayerListener.OnPlayerEntry()
        {
            crosshairPos = Vector2.zero;
            canvas.SetActive(false);
        }

        void IPlayerListener.OnPlayerExit()
        {
            crosshairPos = Vector2.zero;
            canvas.SetActive(false);
        }

        public void Update()
        {
            Vector2 lookDelta = GameInput.GetLookDelta();

            crosshairPos += lookDelta *= new Vector2(1.5f, 1.5f);

            var limit = 250f;

            if (crosshairPos.sqrMagnitude > limit * limit)
            {
                crosshairPos.Normalize();
                crosshairPos *= limit;
            }

            if (canvas == null)
            {
                Logger.Log("Error: Canvas was null for crosshair");
                return;
            }
            else
            {
                if (agilityControls && (mv as Submarine).IsPlayerPiloting())
                {
                    canvas.SetActive(true);
                }
                else
                {
                    canvas.SetActive(false);
                }
            }
            UpdateCallFreelook();
        }

        public void setCrosshairPosition()
        {
            if (canvas.FindChild("Crosshair") == null)
            {
                Logger.Log("Error: Couldn't find child Crosshair");
                return;
            }
            else
            {
                canvas.FindChild("Crosshair").gameObject.GetComponent<RectTransform>().anchoredPosition = crosshairPos;
            }
        }


        // power draw from engines, change scalarFactor to increase or decrease power consumption
        // just copied from vf
        public override void DrainPower(Vector3 moveDirection)
        {
            float scalarFactor = 1f;
            if (engineSpeed == 1)
            {
                 scalarFactor = 0.5f;
            }
            else if (engineSpeed == 3)
            {
                scalarFactor = 4f;
            }
            float basePowerConsumptionPerSecond = moveDirection.x + moveDirection.y + moveDirection.z;
            float upgradeModifier = Mathf.Pow(0.85f, mv.numEfficiencyModules);
            mv.GetComponent<PowerManager>().TrySpendEnergy(scalarFactor * basePowerConsumptionPerSecond * upgradeModifier * Time.deltaTime);
        }

        public override void ControlRotation()
        {
            if (!engineActive)
            {
                return;
            }

            float sensitivityX = 1f;
            float sensitivityY = 1f;

            canvas.SetActive(agilityControls);

            if (agilityControls)
            {
                
                setCrosshairPosition();

                // pitch
                this.rb.AddTorque(-1 * this.mv.transform.right * Time.deltaTime * sensitivityY * crosshairPos.y / 10, ForceMode.Acceleration);

                //yaw
                this.rb.AddTorque(this.mv.transform.up * Time.deltaTime * sensitivityX * crosshairPos.x / 10, ForceMode.Acceleration);
            }
            else
            {
                
                if (GameInput.GetButtonHeld(GameInput.Button.MoveRight))
                {
                    turnSpeedX += turnSpeedAccelX * Time.deltaTime;
                }
                if (GameInput.GetButtonHeld(GameInput.Button.MoveLeft))
                {
                    turnSpeedX -= turnSpeedAccelX * Time.deltaTime;
                }
                turnSpeedX = Mathf.Clamp(turnSpeedX, -turnSpeedMaxX * sensitivityX, turnSpeedMaxX * sensitivityX);

                // drag on turn speed
                if (turnSpeedX > 0)
                {
                    if (turnSpeedX >= turnSpeedDamping * Time.deltaTime)
                    {
                        turnSpeedX -= turnSpeedDamping * Time.deltaTime;
                    }
                    else
                    {
                        turnSpeedX = 0;
                    }
                }
                if (turnSpeedX < 0)
                {
                    if (turnSpeedX <= -turnSpeedDamping * Time.deltaTime)
                    {
                        turnSpeedX += turnSpeedDamping * Time.deltaTime;
                    }
                    else
                    {
                        turnSpeedX = 0;
                    }
                }

                // carry out actual rotation
                rb.AddTorque(mv.transform.up * turnSpeedX * Time.deltaTime, ForceMode.VelocityChange);
                if (!agilityControls)
                {
                    // Gradually stabilize the roll (around the Z-axis)
                    float rollAngle = mv.transform.eulerAngles.z;
                    if (rollAngle > 180) rollAngle -= 360;
                    float rollStabilization = -rollAngle * 0.1f; // Scale factor to control the stabilization strength

                    // Gradually stabilize the pitch (around the X-axis)
                    float pitchAngle = mv.transform.eulerAngles.x;
                    if (pitchAngle > 180) pitchAngle -= 360;
                    float pitchStabilization = -pitchAngle * 0.1f; // Scale factor to control the stabilization strength

                    // Apply stabilizing torque
                    rb.AddTorque(mv.transform.forward * rollStabilization * Time.deltaTime, ForceMode.VelocityChange);
                    rb.AddTorque(mv.transform.right * pitchStabilization * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
        }

        public IEnumerator RollOverTime(float rollAngle, float timeToRoll)
        {
            Quaternion startRoll = mv.transform.rotation;
            Quaternion endRoll = mv.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f, rollAngle));

            float currentTime = 0f;

            gameObject.GetComponent<Beluga>().stabilizeRoll = false;

            while (currentTime < timeToRoll)
            {
                gameObject.transform.localRotation = Quaternion.Lerp(startRoll, endRoll, currentTime / timeToRoll);

                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}