using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna.AstronomicalObjects {
    public abstract class AstronomicalObject {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public Point GridPosition { get; set; }
        public string Texture2DPath;

        private IsoGrid grid;
        private GridTile activeGridTile;        

        // ------------------------------------------------------------------------------------------
        public AstronomicalObject(string fullName) {
            FullName = fullName;
        }

        // ------------------------------------------------------------------------------------------
        public bool CanAssignWorldPosition(IsoGrid grid, Point gridPosition) {
            GridPosition = gridPosition;
            this.grid = grid;
            activeGridTile = grid.GetGridTileByPoint(gridPosition);
            if (grid.OccupyGridTileByPoint(gridPosition, ref activeGridTile, true)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}