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

        
        void IHandTarget.OnHandClick(GUIHand hand)
        {
            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                Beluga closest = Belugamanager.main.FindNearestBeluga(this.transform.position);
                Exosuit container = closest.currentprawn;
                if (container != null)
                {
                    closest.PlayerExit();
                    // don't do the detach method
                    // just see if the player exits correctly at this point
                    closest.TryDetachPrawn();

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
