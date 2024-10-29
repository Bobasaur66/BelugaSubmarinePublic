using Nautilus.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    public class SaveData : SaveDataCache
    {
        // dictionary of saved belugas (prefab id, data)
        public Dictionary<string, BelugaData> belugasSaved;
    }

    public class BelugaData
    {
        public string seamothPrefabIdentifierId;

        public string exosuitPrefabIdentifierId;

        public LightingController.LightingState lightingState;

        public bool shieldActive;

        public bool destroyed;


    }
}


