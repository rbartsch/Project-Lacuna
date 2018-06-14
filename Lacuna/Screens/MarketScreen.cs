using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.Trade;
using Lacuna.StationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    public class MarketScreen : Screen {
        private Sprite layout;
        Button backToGameplayButton;
        private Text2DTabInteractable tabulatedTextInteractable;
        private Text2D selectedText;
        private Dictionary<int, KeyValuePair<TradeGood, BuySellValue>> tradeMappedToLayout = new Dictionary<int, KeyValuePair<TradeGood, BuySellValue>>();

        public MarketScreen(Core core) : base("MarketScreen", core, false) {
        }

        public override void Initialize() {
            // Garbage may not be collected here, needs testing
            if (tabulatedTextInteractable != null) {
                for (int i = 0; i < tabulatedTextInteractable.Text2Ds.GetUpperBound(0); i++)
                    for (int j = 0; j < tabulatedTextInteractable.Text2Ds.GetUpperBound(1); j++)
                        tabulatedTextInteractable.Text2Ds[i, j] = null;

                for (int i = 0; i < tabulatedTextInteractable.Buttons.GetUpperBound(0); i++)
                    for (int j = 0; j < tabulatedTextInteractable.Buttons.GetUpperBound(1); j++)
                        tabulatedTextInteractable.Buttons[i, j] = null;
            }

            for (int i = 0; i < Drawable2Ds.Count; i++)
                Drawable2Ds[i] = null;

            for (int i = 0; i < ScreenSpaceDrawable2Ds.Count; i++)
                ScreenSpaceDrawable2Ds[i] = null;


            Drawable2Ds.Clear();
            ScreenSpaceDrawable2Ds.Clear();
            //GC.Collect();

            tradeMappedToLayout.Clear();

            layout = new Sprite("market_grid", new Vector2(Core.graphics.PreferredBackBufferWidth / 2, Core.graphics.PreferredBackBufferHeight / 2), Color.White);
            layout.SetOriginCenter();
            backToGameplayButton = new Button("button", "Terminus", new Vector2(10, 30), "Back", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            backToGameplayButton.Click += ViewGameplay;
            tabulatedTextInteractable = new Text2DTabInteractable(20, 5, new Vector2((layout.Position.X - layout.Width / 2) + 2, (layout.Position.Y - layout.Height / 2) + 34), new Vector2(5,7), new int[] { 254, 189, 126, 126, 126 }, 30, 1, 2);
            tabulatedTextInteractable.Construct("Terminus", true, "");

            selectedText = new Text2D("Terminus", "Selected: ", new Vector2(10, 100), Color.White);
            foreach(ButtonSimple b in tabulatedTextInteractable.Buttons) {
                b.Click += delegate (object s, EventArgs e) {
                    UpdateSelected(s, e, tabulatedTextInteractable.SelectedIndex);
                };
            }

            base.Initialize();
        }

        public void ViewGameplay(object s, EventArgs e) {
            ScreenManager.SwitchScreen("GameplayScreen");
        }

        public void UpdateSelected(object s, EventArgs e, int[] selectedIndex) {
            selectedText.Text = $"Selected: {tabulatedTextInteractable.Text2Ds[selectedIndex[0], selectedIndex[1]].Text}.\nRow: {selectedIndex[1]}\n";
            KeyValuePair<TradeGood, BuySellValue> actual;
            tradeMappedToLayout.TryGetValue(selectedIndex[1], out actual);

            if (!actual.Equals(default(KeyValuePair<TradeGood, BuySellValue>))) {
                selectedText.Text += $"Actual mapped: {tradeMappedToLayout[selectedIndex[1]].Key.Name}";
            }
        }

        public void ReadMarket(Market market) {
            int row = 0;
            foreach(KeyValuePair<TradeGood, BuySellValue> kvp in market.tradeGoodsForSale) {
                tabulatedTextInteractable.Text2Ds[0, row].Text = $"{kvp.Key.Name}";
                tabulatedTextInteractable.Text2Ds[1, row].Text = $"{kvp.Key.TType}";
                tabulatedTextInteractable.Text2Ds[2, row].Text = $"{kvp.Key.Quantity}";
                tabulatedTextInteractable.Text2Ds[3, row].Text = $"{kvp.Value.buy}";
                tabulatedTextInteractable.Text2Ds[4, row].Text = $"{kvp.Value.sell}";

                tradeMappedToLayout.Add(row, kvp);

                row++;
            }
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            backToGameplayButton.Update(Mouse.GetState());
            tabulatedTextInteractable.Update(Mouse.GetState());

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}