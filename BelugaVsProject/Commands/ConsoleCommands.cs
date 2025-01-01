using Nautilus.Commands;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga.Commands
{
    public static class ConsoleCommands
    {
        [ConsoleCommand("destroybeluga")]
        public static void DestroyBeluga()
        {
            Vector3 playerPos = Player.main.transform.position;

            GameObject nearestBeluga = Belugamanager.FindNearestBeluga(playerPos).gameObject;

            if (nearestBeluga.GetComponent<Beluga>().isScuttled)
            {
                BelugaUtils.NautilusBasicText("Nearest beluga is already dead", 200f);
            }
            else
            {
                nearestBeluga.GetComponent<LiveMixin>().Kill();

                BelugaUtils.NautilusBasicText("Nearest beluga kaboomed!", 200f);
            }
        }
        

        [ConsoleCommand("tpdeep")]
        public static void TPDeep()
        {
            Vector3 teleportPos = new Vector3(-98, -13, 668);
            Quaternion teleportRot = Quaternion.Euler(new Vector3(0f, 0f, 90f));

            Player.main.transform.SetPositionAndRotation(teleportPos, teleportRot);

            BelugaUtils.NautilusBasicText("1 minute of swimming saved", 200f);
        }

        [ConsoleCommand("belugaload")]
        public static void BelugaLoad()
        {
            Beluga beluga = Belugamanager.FindNearestBeluga(Player.main.transform.position);

            beluga.GetComponent<BelugaDataLoader>().LoadData();
        }
    }
}
