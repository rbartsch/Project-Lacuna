using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    // TODO: Refactor initialize and load
    public static class ScreenManager {
        private static List<Screen> screens = new List<Screen>();
        private static Screen activeScreen;

        public static void AddScreen(Screen screen) {
            Console.WriteLine("Adding screen: " + screen.Name);

            if (screens.Contains(GetScreen(screen.Name))) {
                throw new ArgumentException(string.Format("Screen name must be unique. There is already a screen with the name \"{0}\"", screen.Name));
            }

            screens.Add(screen);
        }

        public static Screen GetActiveScreen() {
            if (activeScreen != null) {
                Console.WriteLine("Getting active screen: " + activeScreen.Name);
                return activeScreen;
            }
            else {
                throw new Exception("There is no ActiveScreen");
            }
        }

        public static Screen GetScreen(string name) {
            return screens.Find(x => string.Equals(name, x.Name));
        }

        public static void SwitchScreen(Screen screen) {
            if (activeScreen != null) {
                activeScreen.Active = false;

                Console.WriteLine("Switching screen to: " + screen.Name + " from " + activeScreen.Name);
            }
            else {
                Console.WriteLine("Switching screen to: " + screen.Name);
            }

            activeScreen = screen;
            activeScreen.Active = true;
            Drawable2DManager.AssignScreenDrawable2DList(activeScreen);
            activeScreen.Switched();
        }

        public static void SwitchScreen(string name) {
            if (activeScreen != null) {
                activeScreen.Active = false;

                Console.WriteLine("Switching screen to: " + name + " from: " + activeScreen.Name);
            }
            else {
                Console.WriteLine("Switching screen to: " + name);
            }

            activeScreen = GetScreen(name);
            activeScreen.Active = true;
            Drawable2DManager.AssignScreenDrawable2DList(activeScreen);
            activeScreen.Switched();
        }

        // Used for when we want to initialize a screen later on the game when first switching
        // and has new assets that must be also loaded before initializing. May be useful for large
        // levels for example that load in new assets not loaded on startup.
        // TODO: can be added back when assetmanager can have a content manager for each screen
        // so that we can unload the content for a screen when not needed any more
        public static void InitializeScreen(string name) {
            LoadContentScreen(GetScreen(name));
            Console.WriteLine("Initializing screen: " + name);
            Screen s = GetScreen(name);
            Drawable2DManager.AssignScreenDrawable2DList(s);
            s.Initialize();
        }

        public static void InitializeScreen(Screen screen) {
            LoadContentScreen(screen);
            Console.WriteLine("Initializing screen: " + screen);
            Drawable2DManager.AssignScreenDrawable2DList(screen);
            screen.Initialize();
        }

        // Load all screen game content
        private static void LoadContentScreen(Screen screen) {
            Console.WriteLine("Loading content for screen: " + screen.Name);
            screen.Load();
        }

        // Update active screen
        public static void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            activeScreen.Update(gameTime, NewKeyState, OldKeyState);
        }

        // Draw active screen
        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            activeScreen.Draw(spriteBatch, gameTime);
        }

        //public static void UnloadScreens() { }
    }
}