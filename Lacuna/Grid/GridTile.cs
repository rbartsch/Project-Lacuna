using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Lacuna.AstronomicalObjects;

namespace Lacuna {
    public class GridTile {
        public Sprite sprite;

        public Vector2 WorldPosition { get => sprite.Position; set => sprite.Position = value; }
        public Point GridPosition { get; set; }
        public bool Occupied { get; set; } = false;
        public bool Passable { get; set; } = true;

        //public dynamic Occupier { get; set; }

        public GridTile(Sprite tileSprite, Point gridPosition) {
            sprite = tileSprite;
            GridPosition = gridPosition;
        }

        public void SetOriginCenter() => sprite.SetOriginCenter();

        //public T GetOccupier<T>() where T: class {
        //    if(Occupier is T) {
        //        return Occupier;
        //    }
        //    else {
        //        return null;
        //    }
        //}
    }
}