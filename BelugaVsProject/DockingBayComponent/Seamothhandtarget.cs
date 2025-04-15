using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static TechStringCache;

namespace Beluga
{
    public class SeaMothhandtarget : HandTarget, IHandTarget
    {
        internal Beluga beluga = null;
        void IHandTarget.OnHandClick(GUIHand hand)
        {
            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                if (beluga.currentSeaMoth != null)
                {
                    beluga.seamothBay.Detach(true);
                }
            }
        }
        void IHandTarget.OnHandHover(GUIHand hand)
        {
            if (beluga.currentSeaMoth != null)
            {
                HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
                string displayString = Language.main.Get("EnterDockedVehicle");
                HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
            }
        }
    }
}
