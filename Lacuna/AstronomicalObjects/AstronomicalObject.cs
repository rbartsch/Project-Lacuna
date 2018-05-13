using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna.AstronomicalObjects {
    public abstract class AstronomicalObject {
        public string Name { get; set; }
        public Point GridPosition { get; set; }
        public Sprite Sprite { get; set; }

        private IsoGrid grid;
        private GridTile activeGridTile;

        public AstronomicalObject(string texture2DPath, IsoGrid grid, Point gridPosition) {
            GridPosition = gridPosition;
            this.grid = grid;
            if(grid.OccupyGridTileByPoint(gridPosition, ref activeGridTile)) {
                Sprite = new Sprite(texture2DPath, grid.GetGridTileWorldPosByPoint(gridPosition), Color.White);
            }
        }
    }
}