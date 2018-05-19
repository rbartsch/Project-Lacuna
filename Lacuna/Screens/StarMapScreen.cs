using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Lacuna.ClusterObjects;
using Lacuna.AstronomicalObjects;
using Microsoft.Xna.Framework.Graphics;

namespace Lacuna {
    // Displays the local cluster of stars/planetary systems
    public class StarMapScreen : Screen {
        public List<Button> markers = new List<Button>();
        public Camera2D camera2D;
        Sprite planetarySystemSummaryPanel;
        Text2D planetarySystemSummary;

        public void Test(object s, EventArgs e) {
            Console.WriteLine("Test");
        }

        public void PrintPlanetarySystem(object sender, EventArgs e, PlanetarySystem planetarySystem) {
            int nStars = 0;
            int nPlanets = 0;
            int nMoons = 0;
            Console.WriteLine("-- Planetary System (" + planetarySystem.Name + ") --");
            foreach(AstronomicalObject a in planetarySystem.AstronomicalObjects) {
                if(a is Star star) {
                    Console.WriteLine("[Star]");
                    Console.WriteLine("=> " + star.Name + ":");
                    nStars++;

                    foreach(AstronomicalObject p in planetarySystem.AstronomicalObjects) {
                        if(p is Planet planet) {
                            nPlanets++;
                            if(planet.Parent == star) {
                                Console.WriteLine("     [Planet]");
                                Console.WriteLine("     => " + p.Name + ":");

                                foreach(AstronomicalObject m in planetarySystem.AstronomicalObjects) {
                                    if(m is Moon moon) {
                                        if(moon.Parent == planet) {
                                            nMoons++;
                                            Console.WriteLine("         [Moon]");
                                            Console.WriteLine("         => " + m.Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
          
            Console.WriteLine("----------------------");

            planetarySystemSummary.Text = string.Format("{0}:\n{1}   Stars(s)\n{2}   Planet(s)\n{3}   Moon(s)", planetarySystem.Name, nStars, nPlanets, nMoons);
        }

        public StarMapScreen(Core core, bool initializeOnStartup = true) : base("StarMapScreen", core, initializeOnStartup) {
            camera2D = new Camera2D(core.GraphicsDevice.Viewport);
            CameraTransform = camera2D.Transform;
            camera2D.MoveCamera(new Vector2(Core.graphics.PreferredBackBufferWidth / 2, -Core.graphics.PreferredBackBufferHeight / 2));
        }

        public override void Initialize() {
            BuildMap();
            planetarySystemSummaryPanel = new Sprite("star_map_selected_system_panel", new Vector2(10, 100), Color.White, true, "", 0, null, null, SpriteEffects.None, 0.1f);
            planetarySystemSummary = new Text2D("Terminus", "", new Vector2(16, 127), Color.White, true, "", 0.09f);

            base.Initialize();
        }

        public void BuildMap() {
            foreach(PlanetarySystem p in Persistence.cluster.PlanetarySystems) {
                markers.Add(new Button("star_map_planetary_system_button", "Terminus", p.WorldPosition, p.Name, Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255)));
                //markers.Last().Click += Test;
                markers.Last().Click += delegate (object s, EventArgs e) {
                    PrintPlanetarySystem(s, e, p);
                };
                markers.Last().SetTextBelow();
            }
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            foreach(Button b in markers) {
                b.Update(Mouse.GetState(), camera2D);
            }

            camera2D.Update(Core.GraphicsDevice.Viewport, gameTime, NewKeyState, OldKeyState, Mouse.GetState());
            CameraTransform = camera2D.Transform;

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}