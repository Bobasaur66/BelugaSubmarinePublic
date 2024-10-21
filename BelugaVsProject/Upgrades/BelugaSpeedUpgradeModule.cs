using Beluga.Interfaces;
using Nautilus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;
using VehicleFramework.UpgradeTypes;
using VehicleFramework.Assets;

namespace Beluga.Upgrades
{
    public class BelugaSpeedUpgradeModule : ModVehicleUpgrade
    {
        public override string ClassId => "BelugaSpeedUpgradeModule";
        public override string DisplayName => "Beluga Ion Reactor Upgrade Module";
        public override string Description => "Increases maximum speed of Beluga on all engine modes.";
        public override List<Ingredient> Recipe => new List<Ingredient>()
                {
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(TechType.Kyanite, 8),
                    new Ingredient(TechType.AdvancedWiringKit, 3),
                    new Ingredient(TechType.PrecursorIonCrystal, 4),
                    new Ingredient(TechType.PrecursorIonBattery, 2)
                };

        public override Atlas.Sprite Icon => SpriteManager.Get(TechType.PowerUpgradeModule);
        public override void OnAdded(AddActionParams param)
        {
            if (param.vehicle as Beluga)
            {
                param.vehicle.gameObject.GetComponent<BelugaEngine>().speedUpgradeInstalled = true;
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
                param.vehicle.gameObject.GetComponent<BelugaEngine>().speedUpgradeInstalled = false;
            }
        }
    }
}
