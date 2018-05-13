using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna.AstronomicalObjects {
    public class Star : AstronomicalObject {
        public Star(string texture2DPath, IsoGrid grid, Point gridPosition) : base(texture2DPath, grid, gridPosition) {
        }
    }
}