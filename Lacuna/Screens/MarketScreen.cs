using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    public class MarketScreen : Screen {
        //Sprite layout;
        //Text2DTab tabulatedText;

        //public MarketScreen(Core core) : base("MarketScreen", core, false) {
        //}

        //public override void Initialize() {
        //    layout = new Sprite("market_grid", new Vector2(Core.graphics.PreferredBackBufferWidth / 2, Core.graphics.PreferredBackBufferHeight / 2), Color.White);
        //    layout.SetOriginCenter();
        //    tabulatedText = new Text2DTab(20, 5, new Vector2((layout.Position.X - layout.Width / 2) + 2, (layout.Position.Y - layout.Height / 2) + 34), new int[] { 254, 189, 126, 126, 126 }, 30, 1, 2, true);

        //    tabulatedText.Construct("Terminus", true, "");
        //    tabulatedText.text2Ds[0, 0].Text = "Military Ration";
        //    tabulatedText.text2Ds[1, 0].Text = "Food";
        //    tabulatedText.text2Ds[2, 0].Text = "1000";
        //    tabulatedText.text2Ds[3, 0].Text = "1000";
        //    tabulatedText.text2Ds[4, 0].Text = "1000";
        //    tabulatedText.text2Ds[0, 1].Text = "Nickel";
        //    tabulatedText.text2Ds[1, 1].Text = "Raw Material";
        //    tabulatedText.text2Ds[2, 1].Text = "99 310";
        //    tabulatedText.text2Ds[3, 1].Text = "12";
        //    tabulatedText.text2Ds[4, 1].Text = "11";

        //    base.Initialize();
        //}

        //public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
        //    tabulatedText.Update(Mouse.GetState());

        //    base.Update(gameTime, NewKeyState, OldKeyState);
        //}

        Sprite layout;
        Text2DTabInteractable tabulatedTextInteractable;

        public MarketScreen(Core core) : base("MarketScreen", core, false) {
        }

        public override void Initialize() {
            layout = new Sprite("market_grid", new Vector2(Core.graphics.PreferredBackBufferWidth / 2, Core.graphics.PreferredBackBufferHeight / 2), Color.White);
            layout.SetOriginCenter();
            tabulatedTextInteractable = new Text2DTabInteractable(20, 5, new Vector2((layout.Position.X - layout.Width / 2) + 2, (layout.Position.Y - layout.Height / 2) + 34), new Vector2(5,7), new int[] { 254, 189, 126, 126, 126 }, 30, 1, 2);

            tabulatedTextInteractable.Construct("Terminus", true, "");
            tabulatedTextInteractable.Text2Ds[0, 0].Text = "New Dawn Co. Colony Foods Crate";
            tabulatedTextInteractable.Text2Ds[1, 0].Text = "Food";
            tabulatedTextInteractable.Text2Ds[2, 0].Text = "355 520";
            tabulatedTextInteractable.Text2Ds[3, 0].Text = "2 720";
            tabulatedTextInteractable.Text2Ds[4, 0].Text = "2 150";
            tabulatedTextInteractable.Text2Ds[0, 1].Text = "Compressed Nickel";
            tabulatedTextInteractable.Text2Ds[1, 1].Text = "Raw Material";
            tabulatedTextInteractable.Text2Ds[2, 1].Text = "97 431 810";
            tabulatedTextInteractable.Text2Ds[3, 1].Text = "12";
            tabulatedTextInteractable.Text2Ds[4, 1].Text = "11";
            tabulatedTextInteractable.Text2Ds[0, 2].Text = "Zero-G Scaffolding";
            tabulatedTextInteractable.Text2Ds[1, 2].Text = "Construction";
            tabulatedTextInteractable.Text2Ds[2, 2].Text = "5 991";
            tabulatedTextInteractable.Text2Ds[3, 2].Text = "200";
            tabulatedTextInteractable.Text2Ds[4, 2].Text = "193";

            base.Initialize();
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            tabulatedTextInteractable.Update(Mouse.GetState());

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}