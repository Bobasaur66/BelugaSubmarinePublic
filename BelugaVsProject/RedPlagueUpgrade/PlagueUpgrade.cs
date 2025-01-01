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
    public class PlagueUpgrade : ModVehicleUpgrade
    {
        public override string ClassId => "PlagueBeluga";
        public override string DisplayName => "Beluga Plague Core";
        public override string Description => "Be carefull";
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
            if (param.vehicle is Beluga)
            {
                Beluga it = (Beluga)param.vehicle;
                it.Plague.active = true;
                it.Plagued = true;

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
                Beluga it = (Beluga)param.vehicle;
                it.Plague.active = false;
                it.Plagued = false;
            }
        }
    }
}
