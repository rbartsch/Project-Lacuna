using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lacuna.AstronomicalObjects;
using Lacuna.StationServices;
using Lacuna.ClusterObjects;

namespace Lacuna {
    public class GameplayScreen : Screen {
        Sprite uiLayout;
        Button button;
        IsoGrid grid;
        PlayerShip playerShip;
        //NpcShip npcShip;
        Sprite background;
        Button starMapButton;
        Button localMapButton;
        Button visitObj;
        Text2D mainText;

        string currentSystem;
        string lastAction;

        Sprite panel;
        Text2D panelTitle;
        Text2DTab text2DTabular;
        Text2D panelTextDescription;
        Button closePanelButton;
        Button marketButton;

        List<Sprite> lastAstroObjsSprites = new List<Sprite>();
        List<AstronomicalObject> astroObjsGroup = new List<AstronomicalObject>();
        PlanetarySystem planetarySystem;

        public void Test(object s, EventArgs e) {
            Console.WriteLine("Test");
        }

        public void ViewStarMap(object s, EventArgs e) {
            if (!ScreenManager.GetScreen("StarMapScreen").Initialized) {
                ScreenManager.InitializeScreen("StarMapScreen");
            }

            ScreenManager.SwitchScreen("StarMapScreen");
        }

        public void ViewLocalMap(object s, EventArgs e) {
            ScreenManager.SwitchScreen("PlanetarySystemMapScreen");
        }

        public void ViewMarket(object sender, EventArgs e, Market market) {
            Screen s = ScreenManager.GetScreen("MarketScreen");
            s.Initialized = false;
            ScreenManager.InitializeScreen("MarketScreen");
            ((MarketScreen)s).ReadMarket(market);
            ScreenManager.SwitchScreen("MarketScreen");
        }

        public void VisitAstroObjAtPos(object s, EventArgs e) {
            AstronomicalObject astroObj = astroObjsGroup.Find(x => x.GridPosition == playerShip.GridPosition);
            marketButton.ClearSubscriptions();

            if (astroObj == null) {
                return;
            }

            Console.WriteLine(astroObj.FullName);
            lastAction = $"You visited {astroObj.FullName}.";

            string stationName = "None";
            string stationServices = "None";
            string populationAmount = "N/A";
            string astroObjDescription = "Description here...";

            foreach (AstronomicalObject a in planetarySystem.AstronomicalObjects) {
                if (a is Station station && station.Parent == astroObj) {
                    stationName = station.FullName;
                    if (station.Services.Count > 0) {
                        stationServices = "";
                    }
                    foreach (IStationService service in station.Services) {
                        if (service is Market) {
                            stationServices += "Market";
                            marketButton.Click += delegate (object mO, EventArgs mE) {
                                ViewMarket(mO, mE, (Market)service);
                            };
                        }
                    }
                }
            }

            if (astroObj is Planet planet) {
                populationAmount = planet.Population.ToString();

            }
            else if (astroObj is Moon moon) {
                populationAmount = moon.Population.ToString();
            }

            text2DTabular.Construct("Terminus", true, "", 0.08f, 0.09f);
            text2DTabular.Text2Ds[0, 0].Text = "Name:";
            text2DTabular.Text2Ds[1, 0].Text = $"{astroObj.ShortName} / Full: {astroObj.FullName}";
            text2DTabular.Text2Ds[0, 1].Text = "Station:";
            text2DTabular.Text2Ds[1, 1].Text = $"{stationName}";
            text2DTabular.Text2Ds[0, 2].Text = "Station Services:";
            text2DTabular.Text2Ds[1, 2].Text = $"{stationServices}";
            text2DTabular.Text2Ds[0, 3].Text = "Population:";
            text2DTabular.Text2Ds[1, 3].Text = $"{populationAmount}";

            panelTextDescription.Text = $"{astroObjDescription}";

            panelTextDescription.Position = new Vector2(panelTextDescription.Position.X,
                text2DTabular.Text2Ds[text2DTabular.Text2Ds.GetUpperBound(0), text2DTabular.Text2Ds.GetUpperBound(1)].Position.Y +
                text2DTabular.Text2Ds[text2DTabular.Text2Ds.GetUpperBound(0), text2DTabular.Text2Ds.GetUpperBound(1)].MeasureString().Y + 50);

            panel.DoDraw = true;
            panelTitle.DoDraw = true;
            foreach (Text2D textTab in text2DTabular.Text2Ds) {
                if (textTab != null) {
                    textTab.DoDraw = true;
                }
            }
            panelTextDescription.DoDraw = true;
            marketButton.SetActiveStatus(true);
            closePanelButton.SetActiveStatus(true);
        }

        public void ClosePanel(object sender, EventArgs e) {
            panel.DoDraw = false;
            panelTitle.DoDraw = false;
            foreach (Text2D textTab in text2DTabular.Text2Ds) {
                if (textTab != null) {
                    textTab.DoDraw = false;
                }
            }
            panelTextDescription.DoDraw = false;
            marketButton.SetActiveStatus(false);
            closePanelButton.SetActiveStatus(false);
        }

        public void ReadAstronomicalGroup(object s, EventArgs e, string systemName, List<AstronomicalObject> astroObjsGroup, PlanetarySystem planetarySystem) {
            ClosePanel(this, EventArgs.Empty);
            this.planetarySystem = planetarySystem;

            Console.WriteLine("Clearing old astronomical object sprites");
            foreach (Sprite sprite in lastAstroObjsSprites) {
                for (int i = 0; i < Drawable2Ds.Count; i++) {
                    if (sprite == Drawable2Ds[i]) {
                        Drawable2Ds.Remove(Drawable2Ds[i]);
                    }
                }

                for (int i = 0; i < ScreenSpaceDrawable2Ds.Count; i++) {
                    if (sprite == ScreenSpaceDrawable2Ds[i]) {
                        ScreenSpaceDrawable2Ds.Remove(ScreenSpaceDrawable2Ds[i]);
                    }
                }
            }

            for (int i = 0; i < lastAstroObjsSprites.Count; i++)
                lastAstroObjsSprites[i] = null;

            lastAstroObjsSprites.Clear();

            // Clear any tiles that were set to occupied or non-passable from previous planetary system
            foreach(GridTile tile in grid.GridTiles) {
                tile.Occupied = false;
                tile.Passable = true;
            }

            Console.WriteLine("Adding new astronomical object sprites");
            this.astroObjsGroup = astroObjsGroup;
            foreach(AstronomicalObject astroObj in astroObjsGroup) {
                if (grid.OccupyGridTileByPointAsStatic(astroObj.GridPosition, true)) {
                    if(astroObj is Station) {
                        // skip stations for now, we don't display a graphic for them,
                        // they are part of the planet/moons ATM.
                        continue;
                    }
                    lastAstroObjsSprites.Add(new Sprite(astroObj.Texture2DPath, grid.GetGridTileWorldPosByPoint(astroObj.GridPosition), Color.White, false, "", 0, null, null, SpriteEffects.None, 0.13f));
                    lastAstroObjsSprites.Last().SetOriginCenter();
                }
            }

            localMapButton.ClearSubscriptions();
            localMapButton.Click += ViewLocalMap;
            currentSystem = $"You are in the {systemName}.";
        }

        public GameplayScreen(Core core, bool initializeOnStartup = true) : base("GameplayScreen", core, initializeOnStartup) {
        }

        public override void Initialize() {
            uiLayout = new Sprite("ui_layout", new Vector2(Core.graphics.PreferredBackBufferWidth / 2, Core.graphics.PreferredBackBufferHeight / 2), Color.White);

            button = new Button("button", "Terminus", new Vector2(Core.minResolutionRelativeWidth + 117, Core.minResolutionRelativeHeight + 220), "Test", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false);
            button.Click += Test;

            grid = new IsoGrid(new Point(3, 3), new Point(242, 121), new Vector2(682 + Core.minResolutionRelativeWidth, 261 + Core.minResolutionRelativeHeight), "single_blue_grid_tile");
            grid.Construct();

            playerShip = new PlayerShip(new string[] {
                "ship_1_forward",
                "ship_1_backward",
                "ship_1_right",
                "ship_1_left"
            }, grid, new Point(1, 2));

            //npcShip = new NpcShip(new string[] {
            //    "ship_2_forward",
            //    "ship_2_backward",
            //    "ship_2_right",
            //    "ship_2_left"
            //}, grid, new Point(1, 0), playerShip);

            background = new Sprite("background_1", new Vector2(Core.minResolutionRelativeWidth + 306, Core.minResolutionRelativeHeight + 192), Color.White);
            background.LayerDepth = 1.0f;

            uiLayout.SetOriginCenter();

            foreach (GridTile g in grid.GridTiles) {
                g.SetOriginCenter();
            }
            foreach (Text2D t in grid.TextMarkers) {
                t.SetOriginCenter();
            }

            playerShip.Sprite.SetOriginCenter();
            playerShip.SetDirection(ShipMoveDirection.Forward);

            //npcShip.Sprite.SetOriginCenter();
            //npcShip.SetDirection(ShipMoveDirection.Backward);

            starMapButton = new Button("button", "Terminus", new Vector2(Core.minResolutionRelativeWidth + 322, Core.minResolutionRelativeHeight + 16), "Star Map", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false);
            starMapButton.Click += ViewStarMap;
            localMapButton = new Button("button", "Terminus", new Vector2(Core.minResolutionRelativeWidth + 322, Core.minResolutionRelativeHeight + 61), "Local Map", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false);
            visitObj = new Button("button", "Terminus", new Vector2(Core.minResolutionRelativeWidth + 322, Core.minResolutionRelativeHeight + 106), "Visit Object @ Pos.", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false);
            visitObj.Click += VisitAstroObjAtPos;

            mainText = new Text2D("Verdana", "Main Text", new Vector2(Core.minResolutionRelativeWidth + 322, Core.minResolutionRelativeHeight + 582), Color.White);

            panel = new Sprite("panel", new Vector2(Core.graphics.PreferredBackBufferWidth / 2, Core.graphics.PreferredBackBufferHeight / 2), Color.White, true, "", 0, null, null, SpriteEffects.None, 0.1f);
            panel.SetOriginCenter();
            panelTitle = new Text2D("Terminus", "Astronomical Object Information:", new Vector2((panel.Position.X - panel.Width / 2) + 6, (panel.Position.Y - panel.Height / 2) + 4), Color.White, true, "", 0.09f);
            text2DTabular = new Text2DTab(4, 2, new Vector2(panelTitle.Position.X, panelTitle.Position.Y + 30), new int[] { 200, 200 }, 20);
            panelTextDescription = new Text2D("Verdana", "> Description here...", new Vector2((panel.Position.X - panel.Width / 2) + 6, 0), Color.White, true, "", 0.09f);
            closePanelButton = new Button("button", "Terminus", new Vector2(panel.Position.X - 88, (panel.Position.Y + panel.Height / 2) + 5), "Close", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            closePanelButton.ClearSubscriptions();
            closePanelButton.Click += ClosePanel;
            marketButton = new Button("button", "Terminus", new Vector2(panel.Position.X-panel.Width/2+4, panel.Position.Y+panel.Height/2-44), "Market", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true, 0.09f);
            ClosePanel(this, EventArgs.Empty);

            Persistence.Initialize();

            base.Initialize();
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            button.Update(Mouse.GetState());
            starMapButton.Update(Mouse.GetState());
            localMapButton.Update(Mouse.GetState());
            visitObj.Update(Mouse.GetState());
            closePanelButton.Update(Mouse.GetState());
            marketButton.Update(Mouse.GetState());
            playerShip.Update(NewKeyState, OldKeyState);
            mainText.Text = $"{currentSystem}\nLast action: {lastAction}";
            if (NewKeyState.IsKeyDown(Keys.Space) && OldKeyState.IsKeyUp(Keys.Space)) {
                ScreenManager.SwitchScreen("TestScreen");
            }
            if (NewKeyState.IsKeyDown(Keys.Q) && OldKeyState.IsKeyUp(Keys.Q)) {
                //Drawable2DManager.AssignScreenDrawable2DList(this);
                Sprite s = new Sprite("ship_1_backward", new Vector2(200, 0), Color.Green);
                //s.SetOriginCenter();                
            }
            if (NewKeyState.IsKeyDown(Keys.Escape) && OldKeyState.IsKeyUp(Keys.Escape)) {
                ScreenManager.SwitchScreen("MainMenuScreen");
            }
            if (NewKeyState.IsKeyDown(Keys.M) && OldKeyState.IsKeyUp(Keys.M)) {
                if (!ScreenManager.GetScreen("StarMapScreen").Initialized) {
                    ScreenManager.InitializeScreen("StarMapScreen");
                }

                ScreenManager.SwitchScreen("StarMapScreen");
            }
            if(NewKeyState.IsKeyDown(Keys.V) && OldKeyState.IsKeyUp(Keys.V)) {
                VisitAstroObjAtPos(this, EventArgs.Empty);
            }
            if(NewKeyState.IsKeyDown(Keys.N) && OldKeyState.IsKeyUp(Keys.N)) {
                ScreenManager.InitializeScreen("MarketScreen");
                ScreenManager.SwitchScreen("MarketScreen");
            }
        }
    }
}