using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleFramework.VehicleComponents;
using VehicleFramework.VehicleTypes;

namespace Beluga
{
    public partial class Beluga : Submarine
    {
        private void SetupMaterialReactor()
        {
            var reactor = transform.Find("Reactor").gameObject.AddComponent<VehicleFramework.VehicleComponents.MaterialReactor>();
            reactor.Initialize(this, 2, 2, "Beluga Reactor", 0, GetBelugaReactorData());
            reactor.canViewWhitelist = false;
            reactor.interactText = "Beluga Nuclear Reactor";
        }

        public static List<MaterialReactorData> GetBelugaReactorData()
        {
            return new List<MaterialReactorData>
            {
                new MaterialReactorData
                {
                    inputTechType = TechType.ReactorRod,
                    totalEnergy = 20000f,
                    energyPerSecond = 20f,
                    outputTechType = TechType.DepletedReactorRod
                },
                new MaterialReactorData
                {
                    inputTechType = TechType.SeaTreaderPoop,
                    totalEnergy = 10000f,
                    energyPerSecond = 20f,
                    outputTechType = TechType.None
                }

            };
        }


    }

   
}
