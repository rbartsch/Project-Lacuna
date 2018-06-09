using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna.AstronomicalObjects {
    public class Moon : AstronomicalObject {
        public Planet Parent { get; set; }
        public MoonType Type { get; set; }
        public long Population { get; set; }

        public Moon(string name, Planet parent) : base(name) {
            Parent = parent;
        }
    }
}