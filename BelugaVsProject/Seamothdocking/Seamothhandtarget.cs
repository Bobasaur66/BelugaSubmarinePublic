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


        void IHandTarget.OnHandClick(GUIHand hand)
        {
            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                Beluga closest = Belugamanager.FindNearestBeluga(this.transform.position);
                SeaMoth container = closest.currentSeaMoth;
                if (container != null)
                {
                    closest.PlayerExit();
                    // don't do the detach method
                    // just see if the player exits correctly at this point
                    closest.TryDetachSeamoth();

                }
            }

        }
        void IHandTarget.OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("EnterDockedVehicle");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);
            

        }
    }
}
