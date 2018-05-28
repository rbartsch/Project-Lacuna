using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna.AstronomicalObjects {
    public class Star : AstronomicalObject {
        public StarType Type { get; set; }

        public Star(string name) : base(name) {
        }
    }
}