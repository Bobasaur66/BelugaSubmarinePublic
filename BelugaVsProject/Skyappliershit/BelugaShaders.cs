using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Nautilus;
using Nautilus.Utility;
using mset;
using Beluga.Interfaces;
using static Nautilus.Utility.MaterialUtils;
using Nautilus.Utility.MaterialModifiers;

namespace Beluga
{
    public partial class Beluga
    {
        public static void ApplyMarmosetUBERShader(GameObject model)
        {
            //MaterialUtils.ApplySNShaders(model, 10f, 1f, 1f);

            GameObject gameObject = model;
            float shininess = 10f;
            float specularIntensity = 1f;
            float glowStrength = 1f;

            var renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                for (var j = 0; j < renderers[i].materials.Length; j++)
                {
                    var material = renderers[i].materials[j];

                    var matNameLower = material.name.ToLower();
                    bool transparent = matNameLower.Contains("transparent");
                    bool alphaClip = matNameLower.Contains("cutout");

                    var materialType = MaterialType.Opaque;
                    if (transparent)
                        materialType = MaterialType.Transparent;
                    else if (alphaClip)
                        materialType = MaterialType.Cutout;

                    bool blockShaderConversion = false;

                    // stop volumetric lights getting marmo
                    if (renderers[i].gameObject.name == "VolumetricLight")
                    {
                        blockShaderConversion = true;
                    }

                    if (!blockShaderConversion)
                    {
                        ApplyUBERShader(material, shininess, specularIntensity, glowStrength, materialType);
                    }
                }
            }
        }

        public static class BelugaSkyApplierAdder
        {
            public static void AddSkyApplierComponents(GameObject prefab)
            {
                // remove any existing skyappliers
                foreach (SkyApplier skyApplier in prefab.GetComponents<SkyApplier>())
                {
                    Component.DestroyImmediate(skyApplier);
                }
                // remove any existing lighting controllers
                foreach (LightingController lightingController in prefab.GetComponents<LightingController>())
                {
                    Component.DestroyImmediate(lightingController);
                }

                BelugaSkyApplierManager prefabSAManager = prefab.EnsureComponent<BelugaSkyApplierManager>();
                prefabSAManager.lightingController = prefab.EnsureComponent<LightingController>();
                prefabSAManager.lightingController.skies = new LightingController.MultiStatesSky[2];

                prefabSAManager.exteriorSkyApplier = prefab.AddComponent<SkyApplier>();
                prefabSAManager.interiorSkyApplier = prefab.AddComponent<SkyApplier>();
                prefabSAManager.windowSkyApplier = prefab.AddComponent<SkyApplier>();

                prefabSAManager.exteriorSkyApplier.anchorSky = Skies.Auto;
                prefabSAManager.interiorSkyApplier.anchorSky = Skies.BaseInterior;
                prefabSAManager.windowSkyApplier.anchorSky = Skies.BaseGlass;

                prefabSAManager.exteriorSkyApplier.dynamic = true;
                prefabSAManager.interiorSkyApplier.dynamic = false;
                prefabSAManager.windowSkyApplier.dynamic = false;

                prefabSAManager.exteriorSkyApplier.emissiveFromPower = true;
                prefabSAManager.interiorSkyApplier.emissiveFromPower = true;
                prefabSAManager.windowSkyApplier.emissiveFromPower = true;

                prefabSAManager.lightingController.state = LightingController.LightingState.Damaged;
                prefabSAManager.lightingController.fadeDuration = 1f;
                prefabSAManager.lightingController.emissiveController.intensities = new[] { 1f, 1f, 0f };
            }
        }

        public class BelugaSkyApplierManager : MonoBehaviour, ICyclopsReferencer
        {
            [SerializeField] public SkyApplier exteriorSkyApplier;
            [SerializeField] public SkyApplier interiorSkyApplier;
            [SerializeField] public SkyApplier windowSkyApplier;
            [SerializeField] public float[] lightBrightnessMultipliers = new float[] { 1, 0.5f, 0f };

            [SerializeField] public LightingController lightingController;

            private List<Renderer> _interiorRenderers = new List<Renderer>();
            private List<Renderer> _exteriorRenderers = new List<Renderer>();
            private List<Renderer> _windowRenderers = new List<Renderer>();

            public void SetSkyApplierRenderers(GameObject parent)
            {
                if (parent.FindChild("Model") == null)
                {
                    Debug.LogError("Failed to get model gameobject for skyappliers");
                    return;
                }
                if (parent.FindChild("Wrecked") != null)
                {
                    GameObject wreckedModel = parent.transform.Find("Wrecked/Model").gameObject;

                    foreach (Transform child in wreckedModel.transform)
                    {
                        child.gameObject.EnsureComponent<SubExteriorObjectTag>();
                        _exteriorRenderers.Add(child.GetComponent<Renderer>());
                    }
                }
                else
                {
                    Debug.LogError("Failed to get wrecked model gameobject for skyappliers");
                }

                foreach (Transform child in parent.FindChild("Model").transform)
                {
                    string name = child.name;

                    if (name.Contains("Int"))
                    {
                        child.gameObject.EnsureComponent<SubInteriorObjectTag>();
                        _interiorRenderers.Add(child.GetComponent<Renderer>());
                    }
                    else if (name.Contains("Ext"))
                    {
                        child.gameObject.EnsureComponent<SubExteriorObjectTag>();
                        _exteriorRenderers.Add(child.GetComponent<Renderer>());
                    }
                    else if (name.Contains("Canopy"))
                    {
                        child.gameObject.EnsureComponent<SubWindowTag>();
                        _windowRenderers.Add(child.GetComponent<Renderer>());
                    }
                }
                exteriorSkyApplier.renderers = _exteriorRenderers.ToArray();
                interiorSkyApplier.renderers = _interiorRenderers.ToArray();
                windowSkyApplier.renderers = _windowRenderers.ToArray();
            }

            public void OnCyclopsReferenceFinished(GameObject cyclops)
            {
                var skyBaseGlass = Instantiate(cyclops.transform.Find("SkyBaseGlass"), transform).GetComponent<Sky>();
                var skyBaseInterior = Instantiate(cyclops.transform.Find("SkyBaseInterior"), transform).GetComponent<Sky>();

                interiorSkyApplier.applySky = skyBaseInterior;
                windowSkyApplier.applySky = skyBaseGlass;

                lightingController.skies[0].sky = skyBaseGlass;
                lightingController.skies[1].sky = skyBaseInterior;

                var lights = gameObject.GetComponentsInChildren<Light>(true)
                    .Where((l) => l.gameObject.GetComponent<ExcludeLightFromController>() == null)
                    .ToArray();

                lightingController.lights = new MultiStatesLight[lights.Length];
                for (int i = 0; i < lights.Length; i++)
                {
                    var intensity = lights[i].intensity;
                    lightingController.lights[i] = new MultiStatesLight()
                    {
                        light = lights[i],
                        intensities = new[]
                        {
                        intensity * lightBrightnessMultipliers[0],
                        intensity * lightBrightnessMultipliers[1],
                        intensity * lightBrightnessMultipliers[2]
                    },
                    };
                }

                /*var sub = gameObject.GetComponent<SubRoot>();
                sub.interiorSky = skyBaseInterior;
                sub.glassSky = skyBaseGlass;
                sub.lightControl = lightingController;*/
            }
        }
    }
}

