using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna.AstronomicalObjects {
    public class Planet : AstronomicalObject {
        public Star Parent { get; set; }

        public Planet(string name, Star parent) : base(name) {
            Parent = parent;
        }
    }
}