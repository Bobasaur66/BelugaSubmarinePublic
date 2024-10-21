using Beluga.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework.VehicleTypes;

namespace Beluga
{
    public partial class Beluga : Submarine, ICyclopsReferencer
    {
        public bool shielded = false;
        public GameObject Shield
        {
            get
            {
                return transform.Find("BelugaShield").gameObject;
            }
        }
        public void enableshield ()
        {
            Shield.SetActive(true);
            this.liveMixin.shielded = true;
            shielded = true;


        }

        public void disableshield()
        {
            Shield.SetActive(false);
            this.liveMixin.shielded = false;
            shielded = false;


        }

        public void shieldupdate()
        
        {
            if (shielded) {
                this.ConsumeEnergy(2);
                if (this.GetPercentageOfPower() < 1) 
                { 
                    disableshield();
                
                }
            }
            


        }

        public void OnCyclopsReferenceFinished(GameObject cyclops)
        {
            MeshRenderer cyclopsMR = cyclops.transform.Find("FX/x_Cyclops_GlassShield").GetComponent<MeshRenderer>();

            MeshRenderer shieldMR = Shield.GetComponent<MeshRenderer>();

            //shieldMR.material = cyclopsMR.material;
            shieldMR.material = transform.Find("Model/AtlasHolo").GetComponent<MeshRenderer>().material;
        }
    }
}
