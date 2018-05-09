using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna {
    public struct EngagementVector {
        public ShipSide AttackerSide { get; set; }
        public ShipSide TargetSide { get; set; }

        // ------------------------------------------------------------------------------------------
        public EngagementVector(ShipSide attackerSide, ShipSide targetSide) {
            AttackerSide = attackerSide;
            TargetSide = targetSide;
        }
    }
}