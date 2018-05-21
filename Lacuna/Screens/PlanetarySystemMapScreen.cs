using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.ClusterObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    public class PlanetarySystemMapScreen : Screen {
        Sprite systemNameHorizontalDivider;
        Text2D systemName;
        PlanetarySystem planetarySystem;
        Button backToStarMapButton;

        // ------------------------------------------------------------------------------------------
        public PlanetarySystemMapScreen(Core core) : base("PlanetarySystemMapScreen", core, false) {
        }

        // ------------------------------------------------------------------------------------------
        public override void Initialize() {
            // Garbage may not be collected here, needs testing
            for (int i = 0; i < Drawable2Ds.Count; i++)
                Drawable2Ds[i] = null;

            for (int i = 0; i < ScreenSpaceDrawable2Ds.Count; i++)
                ScreenSpaceDrawable2Ds[i] = null;

            Drawable2Ds.Clear();
            ScreenSpaceDrawable2Ds.Clear();
            //GC.Collect();

            systemNameHorizontalDivider = new Sprite("local_map_name_divider", new Vector2(10, 28), Color.White);
            systemName = new Text2D("Terminus", "?", new Vector2(10, systemNameHorizontalDivider.Position.Y-22), Color.White);
            backToStarMapButton = new Button("button", "Terminus", new Vector2(systemNameHorizontalDivider.Position.X, systemNameHorizontalDivider.Position.Y + 10), "Back To Star Map", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false);
            backToStarMapButton.Click += BackToStarMap;

            base.Initialize();
        }

        // ------------------------------------------------------------------------------------------
        public void ReadPlanetarySystem(PlanetarySystem planetarySystem) {
            this.planetarySystem = planetarySystem;
            systemName.Text = planetarySystem.Name + " System";
        }

        // ------------------------------------------------------------------------------------------
        public void BackToStarMap(object sender, EventArgs e) {
            ScreenManager.SwitchScreen("StarMapScreen");
        }

        // ------------------------------------------------------------------------------------------
        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            backToStarMapButton.Update(Mouse.GetState());

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}