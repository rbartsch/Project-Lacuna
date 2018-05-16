using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Lacuna.AstronomicalObjects;

namespace Lacuna.Generators {
    public class PlanetarySystemGenerator {
        public PlanetarySystem Generate() {
            PlanetarySystem planetarySystem = new PlanetarySystem(new NameGenerator().GenerateAstroObjNameRandom(), new Vector2(Rng.Random.Next(100, 741), Rng.Random.Next(100, 581)));
            return planetarySystem;
        }
    }
}