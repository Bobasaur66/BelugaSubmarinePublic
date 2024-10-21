using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beluga.AudioShit
{
    public class BelugaEngineFMODEmitter : FMOD_CustomLoopingEmitter
    {
        public override void Start()
        {
            base.Start();

            followParent = true;

            asset = Nautilus.Utility.AudioUtils.GetFmodAsset("AmbientLoop");
            assetStart = Nautilus.Utility.AudioUtils.GetFmodAsset("PowerUp");
            assetStop = Nautilus.Utility.AudioUtils.GetFmodAsset("PowerDown");
        }

        public bool engineOn = false;

        public void Update()
        {
            if (engineOn)
            {
                if (!playing)
                {
                    Play();
                }
            }
            else
            {
                if (playing)
                {
                    Stop();
                }
            }
        }
    }
}
