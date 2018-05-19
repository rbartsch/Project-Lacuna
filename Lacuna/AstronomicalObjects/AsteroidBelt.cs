using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lacuna.AstronomicalObjects {
    public class AsteroidBelt : AstronomicalObject {
        AstronomicalObject Parent { get; set; }

        public AsteroidBelt(string name, AstronomicalObject parent) : base(name) {
            Parent = parent;
        }
    }
}