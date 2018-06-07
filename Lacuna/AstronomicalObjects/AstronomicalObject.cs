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
        //public GridTile activeGridTile { get; set; }
        public string Texture2DPath;

        public AstronomicalObject(string fullName) {
            FullName = fullName;
        }
    }
}