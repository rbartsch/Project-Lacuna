using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna {
    public class ShipMoveCompleteEventArgs : EventArgs {
        public Point GridPosition { get; set; }
        public ShipMoveDirection Direction { get; set; }

        public ShipMoveCompleteEventArgs(Point gridPosition, ShipMoveDirection direction) {
            GridPosition = gridPosition;
            Direction = direction;
        }
    }
}