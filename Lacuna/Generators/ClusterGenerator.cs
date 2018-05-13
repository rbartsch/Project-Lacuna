using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Lacuna.AstronomicalObjects;

namespace Lacuna.Generators {
    public class ClusterGenerator {
        public int NPlanetarySystems { get; private set; } = 10;
        public int MaxStarsPerSystem { get; private set; } = 1;
        public int MaxPlanetsPerSystem { get; private set; } = 6;
        public int MaxMoonsPerSystem { get; private set; } = 12;

        public ClusterGenerator() {

        }

        public ClusterGenerator(int nPlanetarySystems, int maxStarsPerSystem, int maxPlanetsPerSystem, int maxMoonsPerSystem) {
            NPlanetarySystems = nPlanetarySystems;
            MaxStarsPerSystem = maxStarsPerSystem;
            MaxPlanetsPerSystem = maxPlanetsPerSystem;
            MaxMoonsPerSystem = maxMoonsPerSystem;
        }

        public Cluster Generate() {
            Console.WriteLine("Generating Cluster");
            Cluster cluster = new Cluster();

            for (int i = 0; i < NPlanetarySystems; i++) {
                cluster.PlanetarySystems.Add(new PlanetarySystemGenerator().Generate());
            }

            return cluster;
        }
    }
}