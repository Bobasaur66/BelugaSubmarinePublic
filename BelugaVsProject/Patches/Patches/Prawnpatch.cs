using HarmonyLib;
namespace Beluga.Patches
{
    [HarmonyPatch(typeof(Exosuit))]
    internal class ExosuitPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Exosuit.Start))]
        public static void ExosuitStartPostfix(Exosuit __instance)
        {
            PrawnManager.main.RegisterPrawn(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Exosuit.Update))]
        public static void ExosuitUpdatePostfix(Exosuit __instance)
        {
            if (Belugamanager.FindNearestBeluga(__instance.transform.position) != null)
            {
                if (__instance = Belugamanager.FindNearestBeluga(__instance.transform.position).currentprawn)
                {
                    //__instance.mainAnimator.SetBool("sit", true);
                    //Logger.Log("spammm");
                }
            }
        }
    }
}
