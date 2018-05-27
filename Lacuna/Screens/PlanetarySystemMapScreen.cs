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
        PlanetarySystem planetarySystem;
        Button backToStarMapButton;

        List<Button> travelButtons = new List<Button>();

        public Camera2D camera2D;

        // ------------------------------------------------------------------------------------------
        public PlanetarySystemMapScreen(Core core) : base("PlanetarySystemMapScreen", core, false) {
            camera2D = new Camera2D(core.GraphicsDevice.Viewport);
            CameraTransform = camera2D.Transform;
            camera2D.CanZoom = false;
            camera2D.MoveCamera(new Vector2(Core.graphics.PreferredBackBufferWidth / 2, -Core.graphics.PreferredBackBufferHeight / 2));
        }

        // ------------------------------------------------------------------------------------------
        public override void Initialize() {
            // Garbage may not be collected here, needs testing
            for(int i = 0; i < travelButtons.Count; i++)
                travelButtons[i] = null;

            for (int i = 0; i < Drawable2Ds.Count; i++)
                Drawable2Ds[i] = null;

            for (int i = 0; i < ScreenSpaceDrawable2Ds.Count; i++)
                ScreenSpaceDrawable2Ds[i] = null;

            travelButtons.Clear();
            Drawable2Ds.Clear();
            ScreenSpaceDrawable2Ds.Clear();
            //GC.Collect();

            systemNameHorizontalDivider = new Sprite("local_map_name_divider", new Vector2(10, 28), Color.White, true);
            systemName = new Text2D("Terminus", "?", new Vector2(10, systemNameHorizontalDivider.Position.Y-22), Color.White, true);
            backToStarMapButton = new Button("button", "Terminus", new Vector2(systemNameHorizontalDivider.Position.X, systemNameHorizontalDivider.Position.Y + 10), "Back To Star Map", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            backToStarMapButton.Click += BackToStarMap;

            base.Initialize();
        }

        public void TravelToAstroObjsGroup(object sender, EventArgs e, List<AstronomicalObject> astroObjsGroup) {
            Screen s = ScreenManager.GetScreen("GameplayScreen");
            ScreenManager.SwitchScreen("GameplayScreen");
            ((GameplayScreen)s).ReadAstronomicalGroup(s, e, astroObjsGroup);
        }

        // ------------------------------------------------------------------------------------------
        public void ReadPlanetarySystem(PlanetarySystem planetarySystem) {
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
                    Text2D starName = new Text2D("Terminus", star.ShortName, new Vector2(starNameDivider.Position.X, starNameDivider.Position.Y - 17), Color.White);
                    prevX += starSprite.Width + 5;
                    prevY = 100;

                    foreach (AstronomicalObject p in planetarySystem.AstronomicalObjects) {
                        if (p is Planet planet) {
                            if (planet.Parent == star) {
                                Sprite horizontalDivider = new Sprite("local_map_horizontal_divider", new Vector2(prevX, prevY + 31), Color.White);
                                prevX += horizontalDivider.Width + 5;

                                travelButtons.Add(new Button("local_map_travel_button", "Terminus", new Vector2(prevX, prevY + 20), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false));
                                List<AstronomicalObject> planetAndMoonGroup = new List<AstronomicalObject>();
                                planetAndMoonGroup.Add(planet);
                                travelButtons.Last().Click += delegate (object s, EventArgs e) {
                                    TravelToAstroObjsGroup(s, e, planetAndMoonGroup);
                                };
                                prevX += travelButtons.Last().Image.Width + 5;

                                Sprite planetSprite = new Sprite(planet.Texture2DPath, new Vector2(prevX, prevY), Color.White);
                                prevY += planetSprite.Height + 20;
                                Sprite planetNameDivider = new Sprite("local_map_object_name_divider", new Vector2(prevX, prevY), Color.White);
                                Text2D planetName = new Text2D("Terminus", planet.ShortName, new Vector2(planetNameDivider.Position.X, planetNameDivider.Position.Y - 17), Color.White);
                                prevX += planetSprite.Width + 5;

                                int tmpPrevX = prevX;
                                prevX -= (planetSprite.Width / 2) + 5;
                                prevY += 5;
                                foreach (AstronomicalObject m in planetarySystem.AstronomicalObjects) {
                                    if (m is Moon moon) {
                                        if (moon.Parent == planet) {
                                            Sprite verticalDivider = new Sprite("local_map_vertical_divider", new Vector2(prevX, prevY), Color.White);
                                            prevY += verticalDivider.Height + 5;

                                            planetAndMoonGroup.Add(moon);

                                            Sprite moonSprite = new Sprite(moon.Texture2DPath, new Vector2(prevX, prevY), Color.White);
                                            prevY += moonSprite.Height + 20;
                                            Sprite moonNameDivider = new Sprite("local_map_object_name_divider", new Vector2(prevX, prevY), Color.White);
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

        // ------------------------------------------------------------------------------------------
        public void BackToStarMap(object sender, EventArgs e) {
            ScreenManager.SwitchScreen("StarMapScreen");
        }

        // ------------------------------------------------------------------------------------------
        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            backToStarMapButton.Update(Mouse.GetState());
            foreach(Button b in travelButtons) {
                b.Update(Mouse.GetState(), camera2D);
            }

            camera2D.Update(Core.GraphicsDevice.Viewport, gameTime, NewKeyState, OldKeyState, Mouse.GetState(), true);
            CameraTransform = camera2D.Transform;

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}