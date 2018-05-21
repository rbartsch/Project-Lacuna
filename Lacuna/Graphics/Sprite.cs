using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lacuna {
    public class Sprite : Drawable2D {
        private Texture2D texture2D;

        public int Width { get => texture2D.Width; }
        public int Height { get => texture2D.Height; }
        public Rectangle Area { get => new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }

        // ------------------------------------------------------------------------------------------
        public Sprite(string texture2DName, Vector2 position, Color color, bool drawInScreenSpace = false, string tag = "", float rotation = 0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0.12f) : base(position, color, drawInScreenSpace, tag, rotation, origin, scale, spriteEffects, layerDepth) {
            texture2D = AssetManager.GetAsset(AssetType.Texture2D, texture2DName);
        }

        // Must be set after texture2D loaded
        // ------------------------------------------------------------------------------------------
        public void SetOriginTopLeft() {
            Origin = new Vector2(0, 0);
        }

        // Must be set after texture2D loaded
        // ------------------------------------------------------------------------------------------
        public void SetOriginCenter() {
            Origin = new Vector2(Width / 2, Height / 2);
        }

        // https://stackoverflow.com/questions/8372041/how-to-check-if-a-texture2d-is-transparent
        // Use this to check if a texture2d has a an area that is transparent
        // ------------------------------------------------------------------------------------------
        bool IsRegionTransparent(Texture2D texture, Rectangle r) {
            int size = r.Width * r.Height;
            Color[] buffer = new Color[size];
            texture.GetData(0, r, buffer, 0, size);
            return buffer.All(c => c == Color.Transparent);
        }

        // ------------------------------------------------------------------------------------------
        public override void Draw(SpriteBatch spriteBatch) {
            if (texture2D != null && DoDraw) {
                spriteBatch.Draw(texture2D, ParentPosition + Position, null, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
            }
        }
    }
}