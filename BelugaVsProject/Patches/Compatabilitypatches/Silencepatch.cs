using CallOfTheVoid.Mono.Creatures.Silence;
using HarmonyLib;

namespace Beluga.Compatabilitypatches
{
    [HarmonyPatch(typeof(SilenceSmallVehicleGrab))]
    class SilencePatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SilenceSmallVehicleGrab.GrabVehicle))]
        public static bool SilenceSmallVehicleGrabGrabVehiclePrefix(Vehicle vehicle)
        {
            return !(vehicle is Beluga);
        }
    }
}
