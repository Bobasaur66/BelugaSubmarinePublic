using HarmonyLib;
namespace Beluga.Patches
{
    [HarmonyPatch(typeof(SeaMoth))]
    internal class SeamothPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(SeaMoth.Start))]
        public static void Postfix(SeaMoth __instance)
        {
            SeaMothmanager.main.RegisterSeaMoth(__instance);
        }
    }
}
