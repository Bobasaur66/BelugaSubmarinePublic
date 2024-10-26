using Nautilus.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    public class SavingShit : SaveDataCache
    {
        // beluga prefab identifier id, 
        public Dictionary<string, BelugaSaveData> belugasSaved;
    }

    public class BelugaSaveData
    {
        public string seamothPrefabIdentifierId;

        public string exosuitPrefabIdentifierId;

        public LightingController.LightingState lightingState;

        public bool shieldActive;

        public bool destroyed;


    }
}


