using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.ClusterObjects;
using Lacuna.AstronomicalObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    public class PlanetarySystemMapScreen : Screen {
        Sprite systemNameHorizontalDivider;
        Text2D systemName;
        Text2D legend;
        PlanetarySystem planetarySystem;
        Button backToStarMapButton;

        Sprite panel;
        Text2D panelTitle;
        Text2DTabular text2DTabular;
        Text2D panelTextDescription;
        Button closePanelButton;

        string astroObjDescription;

        List<Button> travelButtons = new List<Button>();
        List<Button> infoButtons = new List<Button>();

        public Camera2D camera2D;

        public PlanetarySystemMapScreen(Core core) : base("PlanetarySystemMapScreen", core, false) {
            camera2D = new Camera2D(core.GraphicsDevice.Viewport);
            CameraTransform = camera2D.Transform;
            camera2D.CanZoom = false;
            camera2D.MoveCamera(new Vector2(Core.graphics.PreferredBackBufferWidth / 2, -Core.graphics.PreferredBackBufferHeight / 2));
        }

        public override void Initialize() {
            // Garbage may not be collected here, needs testing
            for(int i = 0; i < travelButtons.Count; i++)
                travelButtons[i] = null;

            for (int i = 0; i < infoButtons.Count; i++)
                infoButtons[i] = null;

            for (int i = 0; i < Drawable2Ds.Count; i++)
                Drawable2Ds[i] = null;

            for (int i = 0; i < ScreenSpaceDrawable2Ds.Count; i++)
                ScreenSpaceDrawable2Ds[i] = null;

            travelButtons.Clear();
            infoButtons.Clear();
            Drawable2Ds.Clear();
            ScreenSpaceDrawable2Ds.Clear();
            //GC.Collect();

            systemNameHorizontalDivider = new Sprite("local_map_name_divider", new Vector2(10, 28), Color.White, true);
            systemName = new Text2D("Terminus", "?", new Vector2(10, systemNameHorizontalDivider.Position.Y-22), Color.White, true);
            legend = new Text2D("Terminus", "Legend: (S) - Station;", new Vector2(400, systemNameHorizontalDivider.Position.Y - 22), Color.White, true);
            backToStarMapButton = new Button("button", "Terminus", new Vector2(systemNameHorizontalDivider.Position.X, systemNameHorizontalDivider.Position.Y + 10), "Back To Star Map", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            backToStarMapButton.ClearSubscriptions();
            backToStarMapButton.Click += BackToStarMap;

            panel = new Sprite("panel", new Vector2(Core.graphics.PreferredBackBufferWidth / 2, Core.graphics.PreferredBackBufferHeight / 2), Color.White, true, "", 0, null, null, SpriteEffects.None, 0.1f);
            panel.SetOriginCenter();
            panelTitle = new Text2D("Terminus", "Astronomical Object Information:", new Vector2((panel.Position.X - panel.Width / 2) + 6, (panel.Position.Y - panel.Height / 2) + 4), Color.White, true, "", 0.09f);
            text2DTabular = new Text2DTabular(3, 2, new Vector2(panelTitle.Position.X, panelTitle.Position.Y + 30), 200, 20);
            panelTextDescription = new Text2D("Verdana", "> Description here...", new Vector2((panel.Position.X - panel.Width / 2) + 6, 0), Color.White, true, "", 0.09f);
            closePanelButton = new Button("button", "Terminus", new Vector2(panel.Position.X-88, (panel.Position.Y+panel.Height/2)+5), "Close", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            closePanelButton.ClearSubscriptions();
            closePanelButton.Click += ClosePanel;
            ClosePanel(this, EventArgs.Empty);

            base.Initialize();
        }

        public void TravelToAstroObjsGroup(object sender, EventArgs e, List<AstronomicalObject> astroObjsGroup) {
            Screen s = ScreenManager.GetScreen("GameplayScreen");
            ScreenManager.SwitchScreen("GameplayScreen");
            ((GameplayScreen)s).ReadAstronomicalGroup(s, e, systemName.Text, astroObjsGroup);
        }

        public void ViewAstroObjInfo(object sender, EventArgs e, AstronomicalObject astroObj, PlanetarySystem planetarySystem) {
            string stationName = "None";
            string populationAmount = "N/A";

            foreach (AstronomicalObject a in planetarySystem.AstronomicalObjects) {
                if(a is Station station && station.Parent == astroObj) {
                    stationName = station.FullName;
                }
            }

            if (astroObj is Planet planet) {
                populationAmount = planet.Population.ToString();

            }
            else if (astroObj is Moon moon) {
                populationAmount = moon.Population.ToString();
            }

            text2DTabular.Construct("Terminus", true, "", 0.09f);
            text2DTabular.text2Ds[0, 0].Text = "Name:";
            text2DTabular.text2Ds[1, 0].Text = $"{astroObj.ShortName} / Full: {astroObj.FullName}";
            text2DTabular.text2Ds[0, 1].Text = "Station:";
            text2DTabular.text2Ds[1, 1].Text = $"{stationName}";
            text2DTabular.text2Ds[0, 2].Text = "Population:";
            text2DTabular.text2Ds[1, 2].Text = $"{populationAmount}";

            astroObjDescription = $"Description here...";
            panelTextDescription.Text = $"{astroObjDescription}";

            panelTextDescription.Position = new Vector2(panelTextDescription.Position.X, 
                text2DTabular.text2Ds[text2DTabular.text2Ds.GetUpperBound(0), text2DTabular.text2Ds.GetUpperBound(1)].Position.Y + 
                text2DTabular.text2Ds[text2DTabular.text2Ds.GetUpperBound(0), text2DTabular.text2Ds.GetUpperBound(1)].MeasureString().Y + 50);

            panel.DoDraw = true;
            panelTitle.DoDraw = true;
            foreach(Text2D textTab in text2DTabular.text2Ds) {
                if (textTab != null) {
                    textTab.DoDraw = true;
                }
            }
            panelTextDescription.DoDraw = true;
            closePanelButton.SetActiveStatus(true);            
        }

        public void ClosePanel(object sender, EventArgs e) {
            panel.DoDraw = false;
            panelTitle.DoDraw = false;
            foreach (Text2D textTab in text2DTabular.text2Ds) {
                if (textTab != null) {
                    textTab.DoDraw = false;
                }
            }
            panelTextDescription.DoDraw = false;
            closePanelButton.SetActiveStatus(false);
        }

        public void ReadPlanetarySystem(PlanetarySystem planetarySystem) {
            camera2D.Position = new Vector2(Core.graphics.PreferredBackBufferWidth / 2, -Core.graphics.PreferredBackBufferHeight / 2);
            this.planetarySystem = planetarySystem;
            systemName.Text = planetarySystem.Name + " System";

            int prevX = 10;
            int prevY = 100;
            foreach (AstronomicalObject a in planetarySystem.AstronomicalObjects) {
                if (a is Star star) {                    
                    travelButtons.Add(new Button("local_map_travel_button", "Terminus", new Vector2(prevX, prevY + 20), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false));
                    List<AstronomicalObject> starGroup = new List<AstronomicalObject>();
                    starGroup.Add(star);
                    travelButtons.Last().Click += delegate (object s, EventArgs e) {
                        TravelToAstroObjsGroup(s, e, starGroup);
                    };
                    prevX += travelButtons.Last().Image.Width + 5;

                    Sprite starSprite = new Sprite(star.Texture2DPath, new Vector2(prevX, prevY), Color.White);
                    prevY += starSprite.Height + 20;
                    Sprite starNameDivider = new Sprite("local_map_object_name_divider", new Vector2(prevX, prevY), Color.White);
                    infoButtons.Add(new Button("info_button", "Terminus", new Vector2(starNameDivider.Position.X + starNameDivider.Width, starNameDivider.Position.Y), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false));
                    infoButtons.Last().Click += delegate (object s, EventArgs e) {
                        ClosePanel(s, e);
                        ViewAstroObjInfo(s, e, star, planetarySystem);
                    };
                    Text2D starName = new Text2D("Terminus", star.ShortName, new Vector2(starNameDivider.Position.X, starNameDivider.Position.Y - 17), Color.White);
                    prevX += starSprite.Width + 5;
                    prevY = 100;

                    foreach (AstronomicalObject p in planetarySystem.AstronomicalObjects) {
                        if (p is Planet planet) {
                            if (planet.Parent == star) {
                                Sprite horizontalDivider = new Sprite("local_map_horizontal_divider", new Vector2(prevX, prevY + 31), Color.White);
                                prevX += horizontalDivider.Width + 5;

                                travelButtons.Add(new Button("local_map_travel_button", "Terminus", new Vector2(prevX, prevY + 20), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false));
                                List<AstronomicalObject> planetStationAndMoonGroup = new List<AstronomicalObject>();
                                planetStationAndMoonGroup.Add(planet);
                                travelButtons.Last().Click += delegate (object s, EventArgs e) {
                                    TravelToAstroObjsGroup(s, e, planetStationAndMoonGroup);
                                };
                                prevX += travelButtons.Last().Image.Width + 5;

                                Sprite planetSprite = new Sprite(planet.Texture2DPath, new Vector2(prevX, prevY), Color.White);
                                prevY += planetSprite.Height + 20;
                                Sprite planetNameDivider = new Sprite("local_map_object_name_divider", new Vector2(prevX, prevY), Color.White);
                                infoButtons.Add(new Button("info_button", "Terminus", new Vector2(planetNameDivider.Position.X + planetNameDivider.Width, planetNameDivider.Position.Y), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false));
                                infoButtons.Last().Click += delegate (object s, EventArgs e) {
                                    ClosePanel(s, e);
                                    ViewAstroObjInfo(s, e, planet, planetarySystem);
                                };
                                Text2D planetName = new Text2D("Terminus", planet.ShortName, new Vector2(planetNameDivider.Position.X, planetNameDivider.Position.Y - 17), Color.White);
                                foreach(AstronomicalObject planetStation in planetarySystem.AstronomicalObjects) {
                                    if(planetStation is Station station) {
                                        if (station.Parent == planet) {
                                            planetName.Text += " (S)";
                                            planetStationAndMoonGroup.Add(station);
                                        }
                                    }
                                }
                                prevX += planetSprite.Width + 5;

                                int tmpPrevX = prevX;
                                prevX -= (planetSprite.Width / 2) + 5;
                                prevY += 5;
                                foreach (AstronomicalObject m in planetarySystem.AstronomicalObjects) {
                                    if (m is Moon moon) {
                                        if (moon.Parent == planet) {
                                            Sprite verticalDivider = new Sprite("local_map_vertical_divider", new Vector2(prevX, prevY), Color.White);
                                            prevY += verticalDivider.Height + 5;

                                            planetStationAndMoonGroup.Add(moon);

                                            Sprite moonSprite = new Sprite(moon.Texture2DPath, new Vector2(prevX, prevY), Color.White);
                                            prevY += moonSprite.Height + 20;
                                            Sprite moonNameDivider = new Sprite("local_map_object_name_divider", new Vector2(prevX, prevY), Color.White);
                                            infoButtons.Add(new Button("info_button", "Terminus", new Vector2(moonNameDivider.Position.X + moonNameDivider.Width, moonNameDivider.Position.Y), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false));
                                            infoButtons.Last().Click += delegate (object s, EventArgs e) {
                                                ClosePanel(s, e);
                                                ViewAstroObjInfo(s, e, moon, planetarySystem);
                                            };
                                            Text2D moonName = new Text2D("Terminus", moon.ShortName, new Vector2(moonNameDivider.Position.X, moonNameDivider.Position.Y - 17), Color.White);

                                            moonSprite.Position = new Vector2(moonSprite.Position.X - moonSprite.Width / 2, moonSprite.Position.Y);
                                            prevY += 5;
                                        }
                                    }
                                }

                                prevX = tmpPrevX;
                                prevY = 100;
                            }
                        }
                    }
                }
            }
        }

        public void BackToStarMap(object sender, EventArgs e) {
            ScreenManager.SwitchScreen("StarMapScreen");
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            backToStarMapButton.Update(Mouse.GetState());
            closePanelButton.Update(Mouse.GetState());
            foreach(Button b in travelButtons) {
                b.Update(Mouse.GetState(), camera2D);
            }
            foreach(Button b in infoButtons) {
                b.Update(Mouse.GetState(), camera2D);
            }

            camera2D.Update(Core.GraphicsDevice.Viewport, gameTime, NewKeyState, OldKeyState, Mouse.GetState(), true);
            CameraTransform = camera2D.Transform;

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}