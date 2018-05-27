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
    // TODO: We should notify a screen when we switch back, useful for example to replay the mainmenu music when switching back and forth to it
    public abstract class Screen {
        // Maybe we should have a new contentmanager per screen? then we can unload each screen if needed
        public ContentManager Content;
        // Drawables are handled per screen, and their objects should always be created there
        public List<Drawable2D> Drawable2Ds = new List<Drawable2D>();
        public List<Drawable2D> ScreenSpaceDrawable2Ds = new List<Drawable2D>();
        public string Name { get; set; }
        public bool InitializeOnStartup { get; set; }
        public bool Initialized { get; set; }
        public bool LoadedContent { get; set; }
        public bool Active { get; set; }
        public Core Core { get; set; }
        // Needs to be updated in child class screen's update with the camera used there
        public Matrix CameraTransform { get; set; } = Matrix.Identity;

        // ------------------------------------------------------------------------------------------
        public Screen(string name, Core core, bool initializeOnStartup) {
            Name = name;
            Core = core;
            InitializeOnStartup = initializeOnStartup;
        }

        // For screens if we are loading in new content for screen not at startup then load is called
        // before initialize, we load with AssetManager here
        // ------------------------------------------------------------------------------------------
        public virtual void Load() {
            LoadedContent = true;
        }

        // ------------------------------------------------------------------------------------------
        //public virtual void Unload() {
        //}

        // ------------------------------------------------------------------------------------------
        public virtual void Initialize() {
            Initialized = true;
        }

        // ------------------------------------------------------------------------------------------
        public virtual void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) { }

        // ------------------------------------------------------------------------------------------
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            // If layer depth is used for sorting you MUST use BackToFront or FrontToBack sort mode!
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, CameraTransform);
            foreach (Drawable2D d in Drawable2Ds) {
                d.Draw(spriteBatch);
            }
            spriteBatch.End();

            // Screen space drawable2Ds only
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, null);
            foreach (Drawable2D d in ScreenSpaceDrawable2Ds) {
                d.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}