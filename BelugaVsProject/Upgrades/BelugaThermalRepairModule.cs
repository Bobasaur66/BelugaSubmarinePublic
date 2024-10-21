using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleFramework.UpgradeTypes;
using VehicleFramework.Assets;
using VehicleFramework;
using Beluga.Interfaces;
using Beluga.Components;
using Nautilus.Extensions;
using UnityEngine;

namespace Beluga.Upgrades
{
    public class BelugaThermalRepairModule : ModVehicleUpgrade
    {
        public override string ClassId => "BelugaThermalRepairModule";
        public override string DisplayName => "Beluga Thermal Refabrication Module";
        public override string Description => "Repairs submarine when temperature is high enough.";
        public override List<Ingredient> Recipe => new List<Ingredient>()
                {
                    new Ingredient(TechType.JeweledDiskPiece, 10),
                    new Ingredient(TechType.CrashPowder, 10),
                    new Ingredient(TechType.WiringKit, 2),
                    new Ingredient(TechType.ComputerChip, 2)
                };

        public override Atlas.Sprite Icon => SpriteManager.Get(TechType.CyclopsThermalReactorModule);
        public override void OnAdded(AddActionParams param)
        {
            if(param.vehicle as Beluga)
            {
                param.vehicle.gameObject.EnsureComponent<ThermalRepairer>();
            }
            else
            {
                BelugaUtils.NautilusBasicText("This module only works on Beluga class submarines!", 400f);
            }
        }
        public override void OnRemoved(AddActionParams param)
        {
            if (param.vehicle as Beluga)
            {
                ThermalRepairer repairer = param.vehicle.gameObject.GetComponent<ThermalRepairer>();
                if (repairer != null)
                {
                    Component.DestroyImmediate(repairer);
                }
            }
        }
    }
}
