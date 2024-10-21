using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using VehicleFramework;
using VehicleFramework.VehicleParts;
using VehicleFramework.VehicleTypes;

namespace Beluga.Components
{
    public class LadderTop : HandTarget, IHandTarget
    {
        public Transform Bottom
        {
            get
            {
                return transform.parent.Find("Bottom");
            }
        }
        public void OnHandClick(GUIHand hand)
        {
            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                if (Bottom != null)
                {
                    Beluga.TeleportPlayer(Bottom.position);

                    //BelugaUtils.PlayFMODSound("ladderDown", Player.main.transform);
                }
                else
                {
                    string msg = "Error: Null reference on transform Bottom";
                    Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
                    message.ShowMessage(msg, 200 * Time.deltaTime);
                }

            }
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("LadderClimb");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);

        }
    }

    public class LadderBottom : HandTarget, IHandTarget
    {
        public Transform Top
        {
            get
            {
                return transform.parent.Find("Top");
            }
        }


        public void OnHandClick(GUIHand hand)
        {
            if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
            {
                if (Top != null)
                {
                    
                    Beluga.TeleportPlayer(Top.position);

                    //BelugaUtils.PlayFMODSound("ladderUp", Player.main.transform);
                }
                else
                {
                    string msg = "Error: Null reference on transform Top";
                    Nautilus.Utility.BasicText message = new Nautilus.Utility.BasicText(500, 0);
                    message.ShowMessage(msg, 200 * Time.deltaTime);
                }

            }
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            string displayString = Language.main.Get("LadderClimb");
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, displayString);

        }
    }




}
