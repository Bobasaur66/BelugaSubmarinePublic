using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


// copied from the seal submarine - thanks guys!

namespace Beluga
{
    public static class CyclopsReferenceManager
    {
        public static GameObject CyclopsReference { get; private set; }

        private static bool _loaded;

        // call this whenever you are about to access CyclopsReference, thanks and goodbye
        public static IEnumerator EnsureCyclopsReferenceExists()
        {
            if (CyclopsReference != null)
            {
                yield break;
            }

            _loaded = false;

            yield return new WaitUntil(() => LightmappedPrefabs.main);

            LightmappedPrefabs.main.RequestScenePrefab("Cyclops", new LightmappedPrefabs.OnPrefabLoaded(OnSubPrefabLoaded));

            yield return new WaitUntil(() => _loaded);
        }

        private static void OnSubPrefabLoaded(GameObject prefab)
        {
            CyclopsReference = prefab;
            _loaded = true;
            Logger.Log("Cyclops reference loaded!");
        }
    }
}
