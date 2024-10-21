using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga.Components
{
    public class ThermalRepairer : MonoBehaviour
    {
        public float repairScalar = 50f;

        LiveMixin liveMixin
        {
            get
            {
                return gameObject.GetComponent<LiveMixin>();
            }
        }

        public void Update()
        {
            float temp = BelugaUtils.GetTemperature(transform.position);

            temp = Mathf.Clamp(temp, 0f, 100f);

            float amount = temp / 100 * repairScalar * Time.deltaTime;

            liveMixin.AddHealth(amount);
        }
    }
}
