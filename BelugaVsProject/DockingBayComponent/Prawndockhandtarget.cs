using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    public class Prawnhandtarget : HandTarget, IHandTarget
    {
        internal Beluga beluga = null;
        void IHandTarget.OnHandClick(GUIHand hand)
        {
            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                if (beluga.currentprawn != null)
                {
                    beluga.prawnBay.Detach(true);
                }
            }
        }
        void IHandTarget.OnHandHover(GUIHand hand)
        {
            if (beluga.currentprawn != null)
            {
                HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
                string displayString = Language.main.Get("EnterDockedVehicle");
                HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
            }
        }
    }
}
