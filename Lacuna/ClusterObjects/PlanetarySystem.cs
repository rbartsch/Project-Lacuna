using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Lacuna.AstronomicalObjects;

namespace Lacuna.ClusterObjects {
    public class PlanetarySystem {
        public string Name { get; set; }
        public Vector2 WorldPosition { get; set; }
        public List<AstronomicalObject> AstronomicalObjects { get; set; } = new List<AstronomicalObject>();

        public PlanetarySystem(string name, Vector2 worldPos) {
            Name = name;
            WorldPosition = worldPos;
        }
    }
}