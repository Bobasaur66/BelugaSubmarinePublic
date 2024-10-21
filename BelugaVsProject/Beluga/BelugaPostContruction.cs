using Beluga.AudioShit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;

namespace Beluga
{
    public partial class Beluga
    {
        public override void SubConstructionComplete()
        {
            base.SubConstructionComplete();

            if (!Belugamanager.craftflag)
            {
                Belugamanager.craftflag = true;
                gameObject.GetComponent<BelugaVoicelineManager>().AddVoicelineToQueue("FirstCraft");
            }
        }
    }
}
