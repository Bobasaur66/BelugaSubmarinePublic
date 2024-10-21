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
        // Beluga, current seamoth, current prawn, lighting state, shield state, engine on/off, engine speed, destroyed true/false
        public List<Tuple<Beluga, SeaMoth, Exosuit, LightingController.LightingState, bool, bool>> belugaSaveData;
    }

    


}


