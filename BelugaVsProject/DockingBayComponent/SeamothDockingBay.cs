using UnityEngine;

namespace Beluga.DockingBayComponent
{
    internal class SeamothDockingBay : BelugaDockingBay
    {
        public override Transform PlayerExitLocation => transform.Find("SeamothDock/DockexitSeamoth");
        public override Transform DockTrigger => transform.Find("SeamothDock/Seamothtrigger");
        public override Transform GetDockedPosition(Vehicle dockedVehicle)
        {
            return transform.Find("SeamothDock/seamothdocked");
        }
        public override bool IsTargetValid(Vehicle thisPossibleTarget)
        {
            return thisPossibleTarget is SeaMoth;
        }
    }
}
