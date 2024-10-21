using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    public class Belugamanager : MonoBehaviour
    {
        public static bool agilityflag = false;
        public static bool craftflag = false;
        public static Belugamanager main
        {
            get
            {
                return Player.main.gameObject.EnsureComponent<Belugamanager>();
            }
        }
        public List<Beluga> AllBeluga = new List<Beluga>();
        public Beluga FindNearestBeluga(Vector3 mount)
        {
            float ComputeDistance(Beluga cc)
            {
                try
                {
                    return Vector3.Distance(mount, cc.transform.position);
                }
                catch
                {
                    return 9999;
                }
            }
            Beluga nearestContainer = null;
            foreach (Beluga cont in AllBeluga)
            {
                if (cont is null)
                {
                    continue;
                }
                if (nearestContainer == null || (ComputeDistance(cont) < ComputeDistance(nearestContainer)))
                {
                    nearestContainer = cont;
                }
            }
            
            return nearestContainer;
        }
        public Beluga closestBeluga()
        {
            float ComputeDistance(Beluga cc)
            {
                try
                {
                    return Vector3.Distance(Player.main.transform.position, cc.transform.position);
                }
                catch
                {
                    return 9999;
                }
            }
            Beluga nearestContainer = null;
            foreach (Beluga cont in AllBeluga)
            {
                if (cont is null)
                {
                    continue;
                }
                if (nearestContainer == null || (ComputeDistance(cont) < ComputeDistance(nearestContainer)))
                {
                    nearestContainer = cont;
                }
            }

            return nearestContainer;
        }
        public void RegisterBeluga(Beluga cont)
        {
            AllBeluga.Add(cont);
        }
        public void DeregisterBeluga(Beluga cont)
        {
            AllBeluga.Remove(cont);
        }
    }
}
