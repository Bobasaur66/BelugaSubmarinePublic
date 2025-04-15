using UnityEngine;
using VehicleFramework.VehicleComponents;
using System.Collections;

namespace Beluga.DockingBayComponent
{
    internal abstract class BelugaDockingBay : ModVehicleDockingBay
    {
        private Beluga beluga => GetComponent<Beluga>();

        protected override IEnumerator DoUndockingAnimations()
        {
            yield return new WaitForSeconds(2f);
            BelugaUtils.PlayFMODSound("undock", currentDockedVehicle.transform);
            beluga.setCollidersFrontDockDoors(false);
            yield return UWE.CoroutineHost.StartCoroutine(base.DoUndockingAnimations());
            beluga.setCollidersFrontDockDoors(true);
        }
        protected override void HandleDockDoors(TechType dockedVehicle, bool open)
        {
            if (dockedVehicle == TechType.Seamoth)
            {
                beluga.targetDockFrontDoors = open;
            }
            else if (dockedVehicle == TechType.Exosuit)
            {
                beluga.targetDockBackDoors = open;
            }
        }
        public override void UndockAction(Vehicle dockedVehicle)
        {
            if(dockedVehicle is Exosuit)
            {
                dockedVehicle.useRigidbody.AddRelativeForce(Vector3.down * 2f, ForceMode.VelocityChange);
            }
            else if (dockedVehicle is SeaMoth)
            {
                dockedVehicle.useRigidbody.AddRelativeForce(Vector3.down * 20f, ForceMode.VelocityChange);
            }
            else
            {
                dockedVehicle.useRigidbody.AddRelativeForce(Vector3.down * 20f, ForceMode.VelocityChange);
            }
        }
    }
}
