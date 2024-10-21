using Beluga.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga
{
    public class ExplosionVFXManager : MonoBehaviour, ICyclopsReferencer
    {
        public VFXController controller;

        public Vector3 interiorExplosionScale = Vector3.one;
        public Vector3 exteriorExplosionScale = Vector3.one;

        public void Setup()
        {
            controller = gameObject.EnsureComponent<VFXController>();

            controller.emitters = new VFXController.VFXEmitter[2];

            transform.parent.GetComponent<Beluga>().vfxcontroller = controller;
        }

        public void OnCyclopsReferenceFinished(GameObject cyclops)
        {
            VFXController cyclopsController = cyclops.transform.Find("FX/CyclopsExplosionFX").GetComponent<VFXController>();
            controller.emitters[0].fx = cyclopsController.emitters[0].fx;
            controller.emitters[1].fx = cyclopsController.emitters[1].fx;

            controller.emitters[0].fx.transform.localScale = interiorExplosionScale;
            controller.emitters[1].fx.transform.localScale = exteriorExplosionScale;

            controller.emitters[0].spawnOnPlay = true;
            controller.emitters[1].spawnOnPlay = true;

            controller.emitters[0].parentTransform = transform.parent;
            controller.emitters[1].parentTransform = transform.parent;
        }
    }
}
