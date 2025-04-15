using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleFramework.UpgradeTypes;

namespace Beluga.Upgrades
{
    internal static class CraftingNames
    {
        internal static List<CraftingNode> GetBelugaCraftingPath()
        {
            return new List<CraftingNode>
            {
                new CraftingNode
                {
                    name = "BelugaUpgrades",
                    displayName = "Beluga Upgrades",
                    icon = VehicleFramework.Assets.StaticAssets.UpgradeIcon
                }
            };
        }
    }
}
