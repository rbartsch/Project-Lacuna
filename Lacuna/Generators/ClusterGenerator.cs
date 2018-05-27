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
        public int MaxStars { get; private set; } = 1; // Only support drawing local planetary system map for one star at the moment
        public int MaxPlanetsPerStar { get; private set; } = 6;
        public int MaxMoonsPerPlanet { get; private set; } = 3;

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

            // Star(s)
            for(int i = 0; i < nStars; i++) {
                Star star = GenerateStar(planetarySystem, i);
                star.GridPosition = new Point(Rng.Random.Next(0, 3), Rng.Random.Next(0, 3));
                planetarySystem.AstronomicalObjects.Add(star);                

                // Planet(s)
                // Chance to generate planets for a star
                if (Rng.Chance(90)) {
                    int nPlanets = Rng.Random.Next(1, MaxPlanetsPerStar + 1);

                    for (int j = 1; j <= nPlanets; j++) {
                        Planet planet = GeneratePlanet(star, j);
                        planet.GridPosition = new Point(Rng.Random.Next(0, 3), Rng.Random.Next(0, 3));
                        planetarySystem.AstronomicalObjects.Add(planet);
                    }
                }
            }

            // Moon(s)
            for(int i = 0; i < planetarySystem.AstronomicalObjects.Count; i++) {
                if(planetarySystem.AstronomicalObjects[i] is Planet p) {
                    List<Point> availablePositions = new List<Point>();

                    for(int y = 0; y < 3; y++) {
                        for(int x = 0; x < 3; x++) {
                            if(p.GridPosition.X != x || p.GridPosition.Y != y) {
                                availablePositions.Add(new Point(x, y));
                            }
                        }
                    }

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
                            Point moonGridPosition = availablePositions[Rng.Random.Next(0, availablePositions.Count)];
                            moon.GridPosition = moonGridPosition;
                            availablePositions.Remove(moonGridPosition);
                            planetarySystem.AstronomicalObjects.Add(moon);
                        }
                    }
                }
            }

            return planetarySystem;
        }

        // ------------------------------------------------------------------------------------------
        public Star GenerateStar(PlanetarySystem planetarySystem, int nStar) {
            Star star = new Star(planetarySystem.Name);
            star.FullName = star.FullName.Insert(star.FullName.Length, " " + starNumToLetter[nStar]);
            star.ShortName = starNumToLetter[nStar] + ".";
            star.Type = StarType.MainSequence;

            star.Texture2DPath = "star";

            return star;
        }

        // ------------------------------------------------------------------------------------------
        public Planet GeneratePlanet(Star star, int nPlanet) {
            Planet planet = new Planet(star.FullName + " " + RomanNumeral.Encode((uint)nPlanet), star);
            planet.ShortName = RomanNumeral.Encode((uint)nPlanet) + ".";
            planet.Type = (PlanetType)Rng.Random.Next(0, Enum.GetValues(typeof(PlanetType)).Cast<int>().Max() + 1);

            switch (planet.Type) {
                case PlanetType.Gas:
                    planet.Texture2DPath = "gas_giant";
                    break;
                case PlanetType.Icy:
                    planet.Texture2DPath = "planet_ice";
                    break;
                case PlanetType.Oceanic:
                    planet.Texture2DPath = "planet_water";
                    break;
                case PlanetType.Rocky:
                    planet.Texture2DPath = "planet_rocky";
                    break;
                case PlanetType.Terra:
                    planet.Texture2DPath = "planet_terra";
                    break;
                default:
                    throw new Exception("Invalid planet. Don't know which graphic to load");
            }

            return planet;
        }

        // ------------------------------------------------------------------------------------------
        public Moon GenerateMoon(Planet planet, int nMoon) {
            Moon moon = new Moon(planet.FullName + " " + nMoon.ToString(), planet);
            moon.ShortName = nMoon.ToString() + ".";
            moon.Type = (MoonType)Rng.Random.Next(0, Enum.GetValues(typeof(MoonType)).Cast<int>().Max() + 1);

            switch (moon.Type) {
                case MoonType.Icy:
                    moon.Texture2DPath = "moon_ice";
                    break;
                case MoonType.Regolith:
                    moon.Texture2DPath = "moon_regolith";
                    break;
                default:
                    throw new Exception("Invalid moon. DOn't know which graphic to load");
            }

            return moon;
        }
    }
}