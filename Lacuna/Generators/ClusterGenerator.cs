using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Lacuna.AstronomicalObjects;
using Lacuna.ClusterObjects;

namespace Lacuna.Generators {
    public class ClusterGenerator {
        public int NPlanetarySystems { get; private set; } = 20;
        public int MaxStars { get; private set; } = 1;
        public int MaxPlanetsPerStar { get; private set; } = 6;
        public int MaxMoonsPerPlanet { get; private set; } = 6;

        private static readonly string[] starNumToLetter = {
            "a","b","c","d","e","f","g","h","i","j","k","l","m",
            "n","o","p","q","r","s","t","v","w","x","y","z",
        };

        // ------------------------------------------------------------------------------------------
        public ClusterGenerator() {

        }

        // ------------------------------------------------------------------------------------------
        public ClusterGenerator(int nPlanetarySystems, int maxStars, int maxPlanetsPerStar, int maxMoonsPerPlanet) {
            NPlanetarySystems = nPlanetarySystems;
            MaxStars = maxStars;
            MaxPlanetsPerStar = maxPlanetsPerStar;
            MaxMoonsPerPlanet = maxMoonsPerPlanet;
        }

        // ------------------------------------------------------------------------------------------
        public Cluster GenerateCluster() {
            Console.WriteLine("Generating Cluster");
            Cluster cluster = new Cluster();

            for (int i = 0; i < NPlanetarySystems; i++) {
                cluster.PlanetarySystems.Add(GeneratePlanetarySystem());
            }

            return cluster;
        }

        // ------------------------------------------------------------------------------------------
        public PlanetarySystem GeneratePlanetarySystem() {
            int nStars = Rng.Random.Next(1, MaxStars + 1);

            PlanetarySystem planetarySystem = new PlanetarySystem(new NameGenerator().GenerateAstroObjNameRandom(), new Vector2(Rng.Random.Next(0, 1367), Rng.Random.Next(0, 769)));

            for(int i = 0; i < nStars; i++) {
                Star star = GenerateStar(planetarySystem, i);
                planetarySystem.AstronomicalObjects.Add(star);

                // Chance to generate planets for a star
                if (Rng.Chance(80)) {
                    int nPlanets = Rng.Random.Next(1, MaxPlanetsPerStar + 1);

                    for (int j = 1; j <= nPlanets; j++) {
                        Planet planet = GeneratePlanet(star, j);
                        planetarySystem.AstronomicalObjects.Add(planet);
                    }
                }
            }

            for(int i = 0; i < planetarySystem.AstronomicalObjects.Count; i++) {
                if(planetarySystem.AstronomicalObjects[i] is Planet p) {
                    // Chance to generate moons for planet
                    if (Rng.Chance(40)) {
                        // If passed, at least one moon
                        int nMoons = 1;

                        // 60% chance of 2 moons
                        if (Rng.Chance(60)) {
                            nMoons = 2;
                        }

                        // 40% chance of 3 or more
                        if (Rng.Chance(40)) {
                            nMoons = Rng.Random.Next(3, MaxMoonsPerPlanet + 1);
                        }

                        for (int j = 1; j <= nMoons; j++) {
                            Moon moon = GenerateMoon(p, j);
                            planetarySystem.AstronomicalObjects.Add(moon);
                        }
                    }
                }
            }


            return planetarySystem;
        }

        public Star GenerateStar(PlanetarySystem planetarySystem, int nStar) {
            Star star = new Star(planetarySystem.Name);
            star.Name = star.Name.Insert(star.Name.Length, " " + starNumToLetter[nStar]);
            return star;
        }

        public Planet GeneratePlanet(Star star, int nPlanet) {
            Planet planet = new Planet(star.Name + " " + RomanNumeral.Encode((uint)nPlanet), star);
            return planet;
        }

        public Moon GenerateMoon(Planet planet, int nMoon) {
            Moon moon = new Moon(planet.Name + " " + nMoon.ToString(), planet);
            return moon;
        }
    }
}