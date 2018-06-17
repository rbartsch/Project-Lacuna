using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.Trade;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    public class ShipOverviewScreen : Screen {
        private Sprite shipOverviewHorizontalDivider;
        private Text2D shipOverviewName;
        private Button backToGameplayButton;
        private string prevScreenName = "";
        private PlayerShip playerShip;
        private Text2D cargoTitle;
        private Text2DTab cargoHoldTabulated;

        public ShipOverviewScreen(Core core) : base("ShipOverviewScreen", core, false) {
        }

        public override void Initialize() {
            for (int i = 0; i < Drawable2Ds.Count; i++)
                Drawable2Ds[i] = null;

            for (int i = 0; i < ScreenSpaceDrawable2Ds.Count; i++)
                ScreenSpaceDrawable2Ds[i] = null;


            Drawable2Ds.Clear();
            ScreenSpaceDrawable2Ds.Clear();

            shipOverviewHorizontalDivider = new Sprite("long_horizontal_divider", new Vector2(10, 28), Color.White, true);
            shipOverviewName = new Text2D("Terminus", "Ship Overview", new Vector2(10, shipOverviewHorizontalDivider.Position.Y - 22), Color.White, true);
            backToGameplayButton = new Button("button", "Terminus", new Vector2(shipOverviewHorizontalDivider.Position.X, shipOverviewHorizontalDivider.Position.Y + 10), "Back", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            backToGameplayButton.Click += ViewGameplay;

            cargoTitle = new Text2D("Terminus", "Items in ship's cargo hold:", new Vector2(10, 100), Color.White);

            cargoHoldTabulated = new Text2DTab(20, 2, new Vector2(10, cargoTitle.Position.Y + 25), new int[] { 250, 100 }, 20);
            cargoHoldTabulated.Construct("Terminus");

            base.Initialize();
        }

        public override void Switched(string screenSwitchingFromName) {
            prevScreenName = screenSwitchingFromName;
        }

        public void ViewGameplay(object s, EventArgs e) {
            ScreenManager.SwitchScreen(prevScreenName);
        }

        public void ReadPlayerShip(PlayerShip playerShip) {
            this.playerShip = playerShip;

            int row = 0;
            foreach(TradeGood tradeGood in playerShip.CargoHold) {
                cargoHoldTabulated.Text2Ds[0, row].Text = $"{tradeGood.Name}";
                cargoHoldTabulated.Text2Ds[1, row].Text = $"Qty: {tradeGood.Quantity}";
                row++;
            }
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            backToGameplayButton.Update(Mouse.GetState());

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}