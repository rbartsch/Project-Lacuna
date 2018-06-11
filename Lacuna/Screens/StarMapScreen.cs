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
        Button viewLocalPlanetarySystemButton;
        Button backToGameplayButton;
        Text2D title;

        public StarMapScreen(Core core) : base("StarMapScreen", core, false) {
            camera2D = new Camera2D(core.GraphicsDevice.Viewport);
            CameraTransform = camera2D.Transform;
            camera2D.MoveCamera(new Vector2(Core.graphics.PreferredBackBufferWidth / 2, -Core.graphics.PreferredBackBufferHeight / 2));
        }

        public void ViewGameplay(object s, EventArgs e) {
            ScreenManager.SwitchScreen("GameplayScreen");
        }

        public override void Initialize() {
            BuildMap();
            planetarySystemSummaryPanel = new Sprite("star_map_selected_system_panel", new Vector2(10, 80), Color.White, true, "", 0, null, null, SpriteEffects.None, 0.1f);
            planetarySystemSummary = new Text2D("Terminus", "", new Vector2(16, planetarySystemSummaryPanel.Position.Y + 27), Color.White, true, "", 0.09f);
            viewLocalPlanetarySystemButton = new Button("button", "Terminus", new Vector2(22, planetarySystemSummaryPanel.Position.Y + 149), "View Local Map", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true, 0.09f);
            title = new Text2D("Terminus", "Cluster Star Map", new Vector2(10, 5), Color.White, true);
            backToGameplayButton = new Button("button", "Terminus", new Vector2(10, 30), "Back", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            backToGameplayButton.Click += ViewGameplay;

            base.Initialize();
        }

        public void ViewLocalMap(object sender, EventArgs e, PlanetarySystem planetarySystem) {
            Screen s = ScreenManager.GetScreen("PlanetarySystemMapScreen");
            s.Initialized = false;
            ScreenManager.InitializeScreen("PlanetarySystemMapScreen");
            ((PlanetarySystemMapScreen)s).ReadPlanetarySystem(planetarySystem);
            ScreenManager.SwitchScreen("PlanetarySystemMapScreen");
        }

        public void PrintPlanetarySystem(object sender, EventArgs e, PlanetarySystem planetarySystem) {
            int nStars = 0;
            int nPlanets = 0;
            int nMoons = 0;
            int nStations = 0;
            Console.WriteLine("-- Planetary System (" + planetarySystem.Name + ") --");
            foreach (AstronomicalObject a in planetarySystem.AstronomicalObjects) {
                if (a is Star star) {
                    Console.WriteLine("[Star]");
                    Console.WriteLine("=> " + star.FullName + ":");
                    nStars++;

                    foreach (AstronomicalObject p in planetarySystem.AstronomicalObjects) {
                        if (p is Planet planet) {
                            nPlanets++;
                            if (planet.Parent == star) {
                                Console.WriteLine("     [Planet]");
                                Console.WriteLine("     => " + p.FullName + ":");

                                foreach(AstronomicalObject s in planetarySystem.AstronomicalObjects) {
                                    if(s is Station station) {
                                        if (station.Parent == planet) {
                                            nStations++;
                                            Console.WriteLine("         [Station]");
                                            Console.WriteLine("         => " + s.FullName);
                                        }
                                    }
                                }

                                foreach (AstronomicalObject m in planetarySystem.AstronomicalObjects) {
                                    if (m is Moon moon) {
                                        if (moon.Parent == planet) {
                                            nMoons++;
                                            Console.WriteLine("         [Moon]");
                                            Console.WriteLine("         => " + m.FullName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("----------------------");

            planetarySystemSummary.Text = string.Format("{0}:\n{1}   Star(s)\n{2}   Planet(s)\n{3}   Moon(s)\n{4}    Station(s)", planetarySystem.Name, nStars, nPlanets, nMoons, nStations);
            viewLocalPlanetarySystemButton.ClearSubscriptions();
            viewLocalPlanetarySystemButton.Click += delegate (object s, EventArgs ee) {
                ViewLocalMap(s, ee, planetarySystem);
            };
        }

        public void BuildMap() {
            foreach(PlanetarySystem p in Persistence.clusters[0].PlanetarySystems) {
                markers.Add(new Button("star_map_planetary_system_button", "Terminus", p.WorldPosition, p.Name, Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false));
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

            if (NewKeyState.IsKeyDown(Keys.Escape) && OldKeyState.IsKeyDown(Keys.Escape)) {
                ScreenManager.SwitchScreen("MainMenuScreen");
            }

            camera2D.Update(Core.GraphicsDevice.Viewport, gameTime, NewKeyState, OldKeyState, Mouse.GetState());
            CameraTransform = camera2D.Transform;

            viewLocalPlanetarySystemButton.Update(Mouse.GetState());
            backToGameplayButton.Update(Mouse.GetState());

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}