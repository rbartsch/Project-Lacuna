﻿using System;
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
        public int minRelativeWidth;
        public int minRelativeHeight;
        float statDrawFrameRate;
        float statUpdateFrameRate;

        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Text2D rateStatistics;

        public void Quit(object s, EventArgs e) {
            Exit();
        }

        public void SetResolution(int width, int height, bool fullScreen, bool vSync) {
            graphics.SynchronizeWithVerticalRetrace = vSync;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = fullScreen;
            graphics.ApplyChanges();
            if (!graphics.IsFullScreen)
                Window.Position = new Point((GraphicsDevice.DisplayMode.Width / 2) - graphics.PreferredBackBufferWidth / 2, (GraphicsDevice.DisplayMode.Height / 2) - graphics.PreferredBackBufferHeight / 2);

            // If to scale with higher resolutions, subtract minimum target resolution from desired resolution, then divide that value by 2, then add that value to the resolution.
            // e.g for width of 682 and a minimum target resolution of 1366, 1920-1366=554, 554/2=277, 682+277=959.
            minRelativeWidth = ((graphics.PreferredBackBufferWidth - minResolutionWidth) / 2);
            minRelativeHeight = ((graphics.PreferredBackBufferHeight - minResolutionHeight) / 2);
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
            graphics.HardwareModeSwitch = false; // Full screen alt-tab bug in 3.6. This is borderless window switch for temp workaround https://github.com/MonoGame/MonoGame/issues/5885
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
            ScreenManager.AddScreen(new GameplayScreen(this));
            ScreenManager.AddScreen(new TestScreen(this));
            ScreenManager.AddScreen(new MainMenuScreen(this));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            AssetManager.GraphicsDevice = GraphicsDevice;
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

            AssetManager.LoadAsset(AssetType.SoundEffect, "PM_CS_beep_classic3_resampled");
            AssetManager.LoadAsset(AssetType.SoundEffect, "PM_CS_beep_action_resampled");

            AssetManager.LoadAsset(AssetType.Song, "PM_CINEMATIC_SOUNDSCAPES__38");

            AssetManager.LoadAsset(AssetType.SpriteFont, "Arial");
            AssetManager.LoadAsset(AssetType.SpriteFont, "SourceCodePro");
            AssetManager.LoadAsset(AssetType.SpriteFont, "Terminus");
            AssetManager.LoadAsset(AssetType.SpriteFont, "TinyUnicode");
            AssetManager.LoadAsset(AssetType.SpriteFont, "Verdana");

            rateStatistics = new Text2D("TinyUnicode", string.Format("Draw {0}; Update {1}", statDrawFrameRate.ToString("0"), statUpdateFrameRate.ToString("0")), new Vector2(1, -3), Color.White);

            //ScreenManager.InitializeScreens();
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
            rateStatistics.Draw(spriteBatch);
            spriteBatch.End();
            rateStatistics.Text = string.Format("project lacuna ver. 01 (dev)\ndraw: {0}; count: {1}\nupdate: {2}\nassets loaded (global): {3}\npress Esc for menu, WASD to move", statDrawFrameRate.ToString("0"), GraphicsDevice.Metrics.DrawCount, statUpdateFrameRate.ToString("0"), AssetManager.TotalLoaded());

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