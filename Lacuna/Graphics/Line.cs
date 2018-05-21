using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lacuna {
    public class Line : Drawable2D {
        private Texture2D texture2D;
        private Vector2 edge;

        public Vector2 EndPosition { get; set; }
        public int Width { get; set; }

        // ------------------------------------------------------------------------------------------
        public Line(Vector2 startPosition, Vector2 endPosition, int width, Color color, bool drawInScreenSpace, string tag = "", float layerDepth = 0.11f) : base(startPosition, color, drawInScreenSpace, tag, 0f, null, null, SpriteEffects.None, layerDepth) {
            EndPosition = endPosition;
            Width = width;

            edge = endPosition - startPosition;
            Rotation = (float)Math.Atan2(edge.Y, edge.X);

            texture2D = new Texture2D(Core.graphics.GraphicsDevice, 1, 1);
            texture2D.SetData(new[] { color });
        }

        // ------------------------------------------------------------------------------------------
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture2D, new Rectangle((int)Position.X, (int)Position.Y, (int)edge.Length(), Width), null, Color, Rotation, Origin, SpriteEffects.None, LayerDepth);
        }
    }
}