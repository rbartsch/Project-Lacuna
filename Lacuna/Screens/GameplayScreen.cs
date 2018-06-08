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

        List<Sprite> lastAstroObjsSprites = new List<Sprite>();
        List<AstronomicalObject> astroObjsGroup = new List<AstronomicalObject>();

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

        public void VisitAstroObjAtPos(object s, EventArgs e) {
            AstronomicalObject astroObj = astroObjsGroup.Find(x => x.GridPosition == playerShip.GridPosition);

            if (astroObj != null) {
                Console.WriteLine(astroObj.FullName);
                lastAction = $"You visited {astroObj.FullName}.";
            }
        }

        public void ReadAstronomicalGroup(object s, EventArgs e, string systemName, List<AstronomicalObject> astroObjsGroup) {
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
            visitObj = new Button("button", "Terminus", new Vector2(Core.minResolutionRelativeWidth + 322, Core.minResolutionRelativeHeight + 106), "Visit Object @ Position", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false);
            visitObj.Click += VisitAstroObjAtPos;

            mainText = new Text2D("Verdana", "Main Text", new Vector2(Core.minResolutionRelativeWidth + 322, Core.minResolutionRelativeHeight + 582), Color.White);

            Persistence.GenerateClusters();

            base.Initialize();
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            button.Update(Mouse.GetState());
            starMapButton.Update(Mouse.GetState());
            localMapButton.Update(Mouse.GetState());
            visitObj.Update(Mouse.GetState());
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
        }
    }
}