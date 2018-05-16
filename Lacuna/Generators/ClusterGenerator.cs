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
        public int MaxStars { get; private set; } = 1;
        public int MaxPlanetsPerStar { get; private set; } = 6;
        public int MaxMoonsPerPlanet { get; private set; } = 3;

        public ClusterGenerator() {

        }

        public ClusterGenerator(int nPlanetarySystems, int maxStars, int maxPlanetsPerStar, int maxMoonsPerPlanet) {
            NPlanetarySystems = nPlanetarySystems;
            MaxStars = maxStars;
            MaxPlanetsPerStar = maxPlanetsPerStar;
            MaxMoonsPerPlanet = maxMoonsPerPlanet;
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