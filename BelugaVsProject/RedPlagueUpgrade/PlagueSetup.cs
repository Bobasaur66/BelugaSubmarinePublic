using Nautilus.Utility.MaterialModifiers;
using Nautilus.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace Beluga
{
    partial class Beluga
    {
        public GameObject Plague { 
            get
            {

                return transform.Find("Plague").gameObject;


            }
        
        
        
        
        }
        public List<GameObject> Spaghetti
        {
            get
            {
                var list = new List<GameObject>();
                // Use transform.Find to get the parent Transform, and iterate over its children
                Transform parent = transform.Find("Plague/Red");
                if (parent != null)
                {
                    foreach (Transform child in parent)
                    {
                        list.Add(child.gameObject);
                    }
                }
                return list;
            }
        }

        public bool Plagued = false;

        Color redShade = new Color(0.4f, 0f, 0f); // Full red color
        Material ionCubeMaterial = MaterialUtils.IonCubeMaterial;

        public void PlagueSetup()
        {


            foreach (GameObject S in Spaghetti)
            {
                // Get all Renderer components on the current GameObject and its children
                foreach (Renderer thisRend in S.GetComponentsInChildren<Renderer>())
                {
                    if (thisRend != null)
                    {
                        // Ensure a fresh material instance for each renderer
                        
                        Material Blood = new Material(ionCubeMaterial);
                        Blood.color = redShade;
                        Blood.SetColor("_SpecColor", redShade);
                        Blood.SetColor("_DetailsColor", redShade);
                        Blood.SetColor("_SquaresColor", redShade);
                        Blood.SetColor("_BorderColor", redShade);



                        // Apply red color directly to the material after assigning it

                        thisRend.material = Blood;

                        
                    }
                }
            }

            Plague.active = false;

        }
    }
}
