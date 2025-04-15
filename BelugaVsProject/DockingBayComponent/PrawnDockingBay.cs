using UnityEngine;

namespace Beluga.DockingBayComponent
{
    internal class PrawnDockingBay : BelugaDockingBay
    {
        public override Transform PlayerExitLocation => transform.Find("Prawndock/DockexitPrawn");
        public override Transform DockTrigger => transform.Find("Prawndock/Prawntrigger");
        public override Transform GetDockedPosition(Vehicle dockedVehicle)
        {
            return transform.Find("Prawndock/prawndocked");
        }
        public override bool IsTargetValid(Vehicle thisPossibleTarget)
        {
            return thisPossibleTarget is Exosuit;
        }
    }
}
