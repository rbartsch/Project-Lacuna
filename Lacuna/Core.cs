using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Core : Game {
        public readonly int minResolutionWidth = 1366;
        public readonly int minResolutionHeight = 768;
        public int minResolutionRelativeWidth;
        public int minResolutionRelativeHeight;
        float statDrawFrameRate;
        float statUpdateFrameRate;

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Text2D info;


        public void Quit(object s, EventArgs e) {
            Exit();
        }


        public void SetResolution(int width, int height, bool fullScreen, bool vSync) {
            graphics.SynchronizeWithVerticalRetrace = vSync;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = fullScreen;
            graphics.ApplyChanges();
            if (!graphics.IsFullScreen) {
                Window.Position = new Point((GraphicsDevice.DisplayMode.Width / 2) - graphics.PreferredBackBufferWidth / 2,
                    (GraphicsDevice.DisplayMode.Height / 2) - graphics.PreferredBackBufferHeight / 2);
            }

            // If to scale with higher resolutions, subtract minimum target resolution from desired resolution, then divide that value by 2, then add that value to the resolution.
            // e.g for width of 682 and a minimum target resolution of 1366, 1920-1366=554, 554/2=277, 682+277=959.
            minResolutionRelativeWidth = ((graphics.PreferredBackBufferWidth - minResolutionWidth) / 2);
            minResolutionRelativeHeight = ((graphics.PreferredBackBufferHeight - minResolutionHeight) / 2);
        }


        private static Cursor GetCursor(string cursorName) {
            var buffer = Resource1.ResourceManager.GetObject(cursorName) as byte[];

            using (var m = new MemoryStream(buffer)) {
                return new Cursor(m);
            }
        }


        public Core() {
            Form form = (Form)Control.FromHandle(Window.Handle);
            form.Cursor = GetCursor("cursor");
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            // Full screen alt-tab bug in 3.6. 
            // This is borderless window switch for temp workaround https://github.com/MonoGame/MonoGame/issues/5885
            graphics.HardwareModeSwitch = false;
            SetResolution(1366, 768, false, true);
            //SetResolution(1920, 1080, true, true);
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            Rng.Initialize();

            ScreenManager.AddScreen(new GameplayScreen(this));
            ScreenManager.AddScreen(new TestScreen(this));
            ScreenManager.AddScreen(new MainMenuScreen(this));
            ScreenManager.AddScreen(new StarMapScreen(this));
            ScreenManager.AddScreen(new PlanetarySystemMapScreen(this));

            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            AssetManager.Content = Content;

            AssetManager.LoadAsset(AssetType.Texture2D, "button");
            AssetManager.LoadAsset(AssetType.Texture2D, "ui_layout");
            AssetManager.LoadAsset(AssetType.Texture2D, "single_blue_grid_tile");
            AssetManager.LoadAsset(AssetType.Texture2D, "ship_1_forward");
            AssetManager.LoadAsset(AssetType.Texture2D, "ship_1_backward");
            AssetManager.LoadAsset(AssetType.Texture2D, "ship_1_right");
            AssetManager.LoadAsset(AssetType.Texture2D, "ship_1_left");
            AssetManager.LoadAsset(AssetType.Texture2D, "planet_rocky");
            AssetManager.LoadAsset(AssetType.Texture2D, "planet_terra");
            AssetManager.LoadAsset(AssetType.Texture2D, "planet_ice");
            AssetManager.LoadAsset(AssetType.Texture2D, "planet_water");
            AssetManager.LoadAsset(AssetType.Texture2D, "star");
            AssetManager.LoadAsset(AssetType.Texture2D, "moon_ice");
            AssetManager.LoadAsset(AssetType.Texture2D, "moon_regolith");
            AssetManager.LoadAsset(AssetType.Texture2D, "gas_giant");
            AssetManager.LoadAsset(AssetType.Texture2D, "ship_2_forward");
            AssetManager.LoadAsset(AssetType.Texture2D, "ship_2_backward");
            AssetManager.LoadAsset(AssetType.Texture2D, "ship_2_right");
            AssetManager.LoadAsset(AssetType.Texture2D, "ship_2_left");
            AssetManager.LoadAsset(AssetType.Texture2D, "background_1");
            AssetManager.LoadAsset(AssetType.Texture2D, "background_2");
            AssetManager.LoadAsset(AssetType.Texture2D, "background_3");
            AssetManager.LoadAsset(AssetType.Texture2D, "background_4");
            AssetManager.LoadAsset(AssetType.Texture2D, "background_5");
            AssetManager.LoadAsset(AssetType.Texture2D, "star_map_planetary_system_button");
            AssetManager.LoadAsset(AssetType.Texture2D, "local_map_grid_tile");
            AssetManager.LoadAsset(AssetType.Texture2D, "star_map_selected_system_panel");
            AssetManager.LoadAsset(AssetType.Texture2D, "local_map_horizontal_divider");
            AssetManager.LoadAsset(AssetType.Texture2D, "local_map_name_divider");
            AssetManager.LoadAsset(AssetType.Texture2D, "local_map_object_name_divider");
            AssetManager.LoadAsset(AssetType.Texture2D, "local_map_travel_button");
            AssetManager.LoadAsset(AssetType.Texture2D, "local_map_vertical_divider");
            AssetManager.LoadAsset(AssetType.Texture2D, "camera_pan_arrow");
            AssetManager.LoadAsset(AssetType.Texture2D, "menu_title");

            AssetManager.LoadAsset(AssetType.SoundEffect, "PM_CS_beep_classic3_resampled");
            AssetManager.LoadAsset(AssetType.SoundEffect, "PM_CS_beep_action_resampled");

            AssetManager.LoadAsset(AssetType.Song, "PM_CINEMATIC_SOUNDSCAPES__38");
            AssetManager.LoadAsset(AssetType.Song, "PM_FN_Music_SeamlessLoop_1C_resampled");

            AssetManager.LoadAsset(AssetType.SpriteFont, "Arial");
            AssetManager.LoadAsset(AssetType.SpriteFont, "SourceCodePro");
            AssetManager.LoadAsset(AssetType.SpriteFont, "Terminus");
            AssetManager.LoadAsset(AssetType.SpriteFont, "TinyUnicode");
            AssetManager.LoadAsset(AssetType.SpriteFont, "Verdana");

            info = new Text2D("TinyUnicode", string.Format(""), new Vector2(1, graphics.PreferredBackBufferHeight - 98), Color.White);

            ScreenManager.InitializeScreen("MainMenuScreen");
            ScreenManager.InitializeScreen("TestScreen");
            ScreenManager.SwitchScreen("MainMenuScreen");
        }


        KeyboardState OldKeyState;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            statUpdateFrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState NewKeyState = Keyboard.GetState();

            ScreenManager.Update(gameTime, NewKeyState, OldKeyState);

            OldKeyState = NewKeyState;

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            statDrawFrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.Black);

            ScreenManager.Draw(spriteBatch, gameTime);

            // Update statistics text
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, null);
            info.Draw(spriteBatch);
            spriteBatch.End();
            info.Text = string.Format("project lacuna ver. {0} (dev)\n-\ndraw: {1}; count: {2}\nupdate: {3}\nassets loaded (global): {4}\n-" +
                "\npress Esc for menu. WASD to move and M to view star map while in gameplay screen",
                Application.ProductVersion, statDrawFrameRate.ToString("0"), GraphicsDevice.Metrics.DrawCount, statUpdateFrameRate.ToString("0"), AssetManager.TotalLoaded());

            base.Draw(gameTime);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
    }
}