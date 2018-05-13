using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Lacuna {
    public abstract class Drawable2D {
        public Vector2 ParentPosition { get; set; } = Vector2.Zero;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Color Color { get; set; }
        public string Tag { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public float LayerDepth { get; set; }
        public bool DoDraw { get; set; } = true;

        // ------------------------------------------------------------------------------------------
        public Drawable2D(Vector2 position, Color color, string tag = "", float rotation = 0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0.0f) {
            Position = position;
            Color = color;
            Tag = tag;
            Rotation = rotation;
            Origin = origin ?? Vector2.Zero;
            Scale = scale ?? Vector2.One;
            SpriteEffects = spriteEffects;
            LayerDepth = layerDepth;
            if (Drawable2DManager.IsUnassignedScreenDrawable2DList()) {
                ConsoleColor originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: A Drawable2D is being created without being added to a screen's internal Drawable2D list.\n\t If this is intended make sure to manually call its draw method.");
                Console.ForegroundColor = originalColor;
            }
            else {
                Drawable2DManager.Add(this);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}