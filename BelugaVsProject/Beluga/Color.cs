using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;
using VehicleFramework.VehicleTypes;

namespace Beluga
{


    public partial class Beluga : Submarine


    {
        /*Material MainExterior = MainPatcher.theUltimateBundleOfAssets.LoadAsset("ExteriorMain",typeof(Material)) as Material;
        Material PrimaryAccent = MainPatcher.theUltimateBundleOfAssets.LoadAsset("ExteriorAccentPrimary", typeof(Material)) as Material;
        Material SecondaryAccent = MainPatcher.theUltimateBundleOfAssets.LoadAsset("ExteriorAccentSecondary", typeof(Material)) as Material;*/
        public  GameObject Hull
        {
            get
            {
                return transform.Find("Model/HullMainExt").gameObject;
            }
        }

        public override GameObject ColorPicker
        {
            get
            {
                return transform.Find("ColorPicker").gameObject;
            }
        }
        public override void PaintVehicleSection(string materialName, Color col)
        {
            foreach (Renderer thisRend in GetComponentsInChildren<Renderer>())
            {
            

                //Renderer thisRend = Hull.GetComponent<Renderer>();

                //Logger.Log("Try find Materials");
                //Logger.Log("number mats " + thisRend.materials.Length);
            for (int j = 0; j < thisRend.materials.Length; j++)
                {
                    //Logger.Log("go trough mats: " + thisRend.materials[j].name);
                    //Logger.Log("Print MaterialName: " + materialName);
                Material thisMat = thisRend.materials[j];
                    if (thisMat.name.Contains(materialName))
                    {
                        Logger.Log("In color changing Process");
                        Material[] deseMats = thisRend.materials;
                        deseMats[j].color = col;
                        thisRend.materials = deseMats;

                    }
                }
            }
            foreach (Renderer thisRend in wreckedModel.GetComponentsInChildren<Renderer>())
            {


                //Renderer thisRend = Hull.GetComponent<Renderer>();

                //Logger.Log("Try find Materials");
                //Logger.Log("number mats " + thisRend.materials.Length);
                for (int j = 0; j < thisRend.materials.Length; j++)
                {
                    //Logger.Log("go trough mats: " + thisRend.materials[j].name);
                    //Logger.Log("Print MaterialName: " + materialName);
                    Material thisMat = thisRend.materials[j];
                    if (thisMat.name.Contains(materialName))
                    {
                        Logger.Log("In color changing Process");
                        Material[] deseMats = thisRend.materials;
                        deseMats[j].color = col;
                        thisRend.materials = deseMats;

                    }
                }
            }


        }

        public override void PaintVehicleName(string name, Color nameColor, Color hullColor)
        {
            base.PaintVehicleName(name, nameColor, hullColor);
        }
        public override void PaintNameDefaultStyle(string name)
        {
            base.PaintNameDefaultStyle(name);
        }

    }


}
