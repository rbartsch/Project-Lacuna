using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna
{
    public class TestScreen : Screen {
        private Sprite sprite;

        public TestScreen(Core core, bool initializeOnStartup = true) : base("TestScreen", core, initializeOnStartup) {
        }

        public override void Initialize() {
            sprite = new Sprite("ship_1_forward", new Vector2(0, 0), Color.Red);
            sprite.SetOriginCenter();

            base.Initialize();
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            if (NewKeyState.IsKeyDown(Keys.Space) && OldKeyState.IsKeyUp(Keys.Space)) {
                ScreenManager.SwitchScreen("GameplayScreen");
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            base.Draw(spriteBatch, gameTime);
        }
    }
}
