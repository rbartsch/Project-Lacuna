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
        Song song;
        Sprite menuTitle;

        // ------------------------------------------------------------------------------------------
        public MainMenuScreen(Core core, bool initializeOnStartup = true) : base("MainMenuScreen", core, initializeOnStartup) {
        }

        // ------------------------------------------------------------------------------------------
        public override void Initialize() {
            menuTitle = new Sprite("menu_title", new Vector2(8, 8), Color.White);
            startButton = new Button("button", "Terminus", new Vector2((menuTitle.Width/2)-88, menuTitle.Height+18), "Start", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false);
            startButton.Click += StartGame;
            exitButton = new Button("button", "Terminus", new Vector2(startButton.Area.X, startButton.Area.Y+startButton.Image.Height+1), "Exit", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), false);
            exitButton.Click += delegate(object s, EventArgs e) {
                Core.Quit(s, e);
            };

            song = AssetManager.GetAsset(AssetType.Song, "PM_FN_Music_SeamlessLoop_1C_resampled");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        // ------------------------------------------------------------------------------------------
        public void StartGame(object s, EventArgs e) {
            if (!ScreenManager.GetScreen("GameplayScreen").Initialized) {
                ScreenManager.InitializeScreen("GameplayScreen");
                MediaPlayer.Stop();
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
