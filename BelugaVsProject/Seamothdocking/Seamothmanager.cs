using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    public class SeaMothmanager : MonoBehaviour
    {
        public static SeaMothmanager main
        {
            get
            {
                return Player.main.gameObject.EnsureComponent<SeaMothmanager>();
            }
        }
        public List<SeaMoth> AllSeaMoth = new List<SeaMoth>();
        public SeaMoth FindNearestSeaMoth(Vector3 mount)
        {
            float ComputeDistance(SeaMoth cc)
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
            SeaMoth nearestContainer = null;
            foreach (SeaMoth cont in AllSeaMoth)
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
        public void RegisterSeaMoth(SeaMoth cont)
        {
            AllSeaMoth.Add(cont);
        }
        public void DeregisterSeaMoth(SeaMoth cont)
        {
            AllSeaMoth.Remove(cont);
        }
    }
}
