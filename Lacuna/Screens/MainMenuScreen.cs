using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lacuna
{
    public class MainMenuScreen : Screen {
        Button startButton;
        Button exitButton;

        // ------------------------------------------------------------------------------------------
        public MainMenuScreen(Core core, bool initializeOnStartup = true) : base("MainMenuScreen", core, initializeOnStartup) {
        }

        // ------------------------------------------------------------------------------------------
        public override void Initialize() {
            startButton = new Button("button", "Terminus", new Vector2(1, 120), "Start", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255));
            startButton.Click += StartGame;
            exitButton = new Button("button", "Terminus", new Vector2(1, 161), "Exit", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255));
            exitButton.Click += delegate(object s, EventArgs e) {
                Core.Quit(s, e);
            };

            base.Initialize();
        }

        // ------------------------------------------------------------------------------------------
        public void StartGame(object s, EventArgs e) {
            if (!ScreenManager.GetScreen("GameplayScreen").Initialized) {
                ScreenManager.InitializeScreen("GameplayScreen");
                startButton.Text = "Resume";
            }

            ScreenManager.SwitchScreen("GameplayScreen");

        }

        // ------------------------------------------------------------------------------------------
        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            startButton.Update(Mouse.GetState());
            exitButton.Update(Mouse.GetState());
            base.Update(gameTime, NewKeyState, OldKeyState);
        }

        // ------------------------------------------------------------------------------------------
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            base.Draw(spriteBatch, gameTime);
        }
    }
}
