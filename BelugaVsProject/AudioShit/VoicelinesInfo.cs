using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beluga.AudioShit
{
    internal static class VoicelinesInfo
    {
        internal static Dictionary<string, float> infoDic = new Dictionary<string, float>
        {
            // round UP to the nearest one tenth (.1)
            {"AbandonShip", 23.5f},
            {"AgilityOff", 2f},
            {"AgilityOn", 2.1f},
            {"Autolevel", 1.6f},
            {"CreatureAttack", 9f},
            {"DamageCritical", 4.4f},
            {"DamageModerate", 2.1f},
            {"DepthClose", 3.2f},
            {"DepthMaximum", 3.5f},
            {"DetectLeviathan", 7f},
            {"DetectLifeform", 5f},
            {"EnginePowerDown", 1.9f},
            {"EnginePowerUp", 1.7f},
            {"PowerCritical", 2.1f},
            {"PowerDepleting", 1.7f},
            {"PowerLow", 1.9f},
            {"PowerZero", 4.2f},
            {"WelcomeAboard", 3.2f}
        };
    }
}
