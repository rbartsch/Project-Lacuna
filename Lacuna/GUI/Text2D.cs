using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lacuna {
    public class Text2D : Drawable2D {
        public string Text { get; set; }

        private SpriteFont spriteFont;

        public Text2D(string spriteFontName, string text, Vector2 position, Color color, bool drawInScreenSpace = false, string tag = "", float layerDepth = 0.11f) : base(position, color, drawInScreenSpace, tag, 0f, null, null, SpriteEffects.None, layerDepth) {
            spriteFont = AssetManager.GetAsset(AssetType.SpriteFont, spriteFontName);
            Text = text;
        }

        public Vector2 MeasureString(string text = null) {
            if (text != null) {
                return spriteFont.MeasureString(text);
            }
            else {
                return spriteFont.MeasureString(Text);
            }
        }

        public int GetLineSpacing() {
            return spriteFont.LineSpacing;
        }

        public float GetSpacing() {
            return spriteFont.Spacing;
        }

        public float GetMaxCharacterWidth() {
            List<Vector2> sizes = new List<Vector2>();
            for (int i = 0; i < spriteFont.Characters.Count; i++) {
                sizes.Add(MeasureString(spriteFont.Characters[i].ToString()));
            }

            return sizes.Max(x => x.X);
        }

        // Must be set after spritefont loaded
        public void SetOriginTopLeft() {
            Origin = new Vector2(0, 0);
        }

        // Must be set after spritefont loaded
        public void SetOriginCenter() {
            Origin = new Vector2((float)Math.Round(MeasureString().X / 2), (float)Math.Round(MeasureString().Y / 2));
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (spriteFont != null && Text != null && DoDraw) {
                spriteBatch.DrawString(spriteFont, Text, ParentPosition + Position, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
            }
        }
    }
}