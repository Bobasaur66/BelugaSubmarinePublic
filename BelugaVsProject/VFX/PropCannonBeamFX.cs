using System.Collections;
using UnityEngine;

namespace Beluga
{
    // mostly from the seal sub - thanks guys!
    public class PropCannonBeamFX : MonoBehaviour
    {
        public static float VectorRandomRange = 0.35f;
        public static float centerOffset = 0f;



        private static GameObject _beamFXPrefab;
        private bool _coroutineRunning = false;

        private GameObject _beamFX;
        private Transform _target;
        private Vector3 _noise;

        public void SetGravityBeam(Transform transform)
        {
            _target = transform;

            _noise = Vector3.zero;
            //_noise = new Vector3(Random.Range(-VectorRandomRange, VectorRandomRange), Random.Range(-VectorRandomRange, VectorRandomRange), Random.Range(-VectorRandomRange, VectorRandomRange));
        }

        public void Update()
        {
            if (!_beamFX) return;

            _beamFX.SetActive(_target);

            if (!_target) return;

            var origin = GetOriginPosition();
            var originVector = GetOriginVector();

            foreach (var beam in _beamFX.GetComponentsInChildren<VFXElectricLine>(true))
            {
                beam.target = GetTargetPosition();
                beam.origin = origin;

                //Make the beams come from just slightly different angles

                beam.originVector = originVector + _noise;
            }
        }

        public Vector3 GetTargetPosition()
        {
            return _target.position - GetOriginVector() * centerOffset;
        }

        public Vector3 GetOriginVector()
        {
            return (_target.position - transform.position).normalized;
        }
        public static Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
        {
            lineDir.Normalize();
            var v = pnt - linePnt;
            var d = Vector3.Dot(v, lineDir);
            return linePnt + lineDir * d;
        }

        public Vector3 GetOriginPosition()
        {
            return transform.position;
        }
        public IEnumerator Start()
        {
            if (_beamFXPrefab)
            {
                _beamFX = Instantiate(_beamFXPrefab, transform.position, transform.rotation, transform);
                yield break;
            }

            while (_coroutineRunning) yield return null;
            if (_beamFXPrefab)
            {
                _beamFX = Instantiate(_beamFXPrefab, transform.position, transform.rotation, transform);
                yield break;
            }
            _coroutineRunning = true;

            var task = CraftData.GetPrefabForTechTypeAsync(TechType.PropulsionCannon);
            yield return task;
            _beamFXPrefab = task.GetResult().FindChild("xPropulsionCannon_Beams");


            _beamFX = Instantiate(_beamFXPrefab, transform.position, transform.rotation, transform);
            _coroutineRunning = false;
        }
    }
}