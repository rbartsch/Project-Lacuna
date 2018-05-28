using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lacuna {
    public class MultiSprite : Drawable2D {
        private Texture2D activeTexture2D;
        private Texture2D[] texture2Ds;

        public int Width { get => activeTexture2D.Width; }
        public int Height { get => activeTexture2D.Height; }
        public Rectangle Area { get => new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }

        public MultiSprite(string[] texture2DNames, Vector2 position, Color color, bool drawInScreenSpace = false, string tag = "", float rotation = 0, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0.12f) : base(position, color, drawInScreenSpace, tag, rotation, origin, scale, spriteEffects, layerDepth) {
            texture2Ds = new Texture2D[texture2DNames.Length];
            for(int i = 0; i < texture2Ds.Length; i++) {
                if(texture2DNames[i] != null) {
                    texture2Ds[i] = AssetManager.GetAsset(AssetType.Texture2D, texture2DNames[i]);
                }
            }

            activeTexture2D = texture2Ds[0];
        }

        public void SwitchSprite(string texture2DPath) {
            foreach(Texture2D t in texture2Ds) {
                if(t.Name == texture2DPath) {
                    activeTexture2D = t;
                }
            }
        }

        public void SwitchSprite(int index) {
            if ((index < 0) || (index > texture2Ds.Length)) {
                throw new ArgumentException("Wrong index position for sprite");
            }

            activeTexture2D = texture2Ds[index];
        }

        // Must be set after texture2D loaded
        public void SetOriginTopLeft() {
            Origin = new Vector2(0, 0);
        }

        // Must be set after texture2D loaded
        public void SetOriginCenter() {
            Origin = new Vector2(Width / 2, Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if(activeTexture2D != null && DoDraw) {
                spriteBatch.Draw(activeTexture2D, ParentPosition + Position, null, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
            }
        }
    }
}