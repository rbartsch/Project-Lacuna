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
        private Sprite marketNameHorizontalDivider;
        private Text2D marketName;
        private Button backToGameplayButton;
        private Text2DTabInteractable marketTabulatedText;
        private Text2D selectedText;
        private Button buyButton;
        private Button sellButton;
        private Sprite buyingWindow;
        private Sprite sellingWindow;
        private Button plusButton;
        private Button minusButton;
        private Button okButton;
        private Button cancelButton;
        private Text2D buyingSellWindowTitle;
        private Text2DTab buyingSellingTabulatedText;
        private Text2DTab totalTabulatedText;

        private Dictionary<int, KeyValuePair<TradeGood, BuySellValue>> tradeMappedToLayout = new Dictionary<int, KeyValuePair<TradeGood, BuySellValue>>();
        private KeyValuePair<TradeGood, BuySellValue> selected;

        int transactionQuantity = 1;
        string actionType = "";

        public MarketScreen(Core core) : base("MarketScreen", core, false) {
        }

        public override void Initialize() {
            // Garbage may not be collected here, needs testing
            if (marketTabulatedText != null) {
                for (int i = 0; i < marketTabulatedText.Text2Ds.GetUpperBound(0); i++)
                    for (int j = 0; j < marketTabulatedText.Text2Ds.GetUpperBound(1); j++)
                        marketTabulatedText.Text2Ds[i, j] = null;

                for (int i = 0; i < marketTabulatedText.Buttons.GetUpperBound(0); i++)
                    for (int j = 0; j < marketTabulatedText.Buttons.GetUpperBound(1); j++)
                        marketTabulatedText.Buttons[i, j] = null;
            }

            for (int i = 0; i < Drawable2Ds.Count; i++)
                Drawable2Ds[i] = null;

            for (int i = 0; i < ScreenSpaceDrawable2Ds.Count; i++)
                ScreenSpaceDrawable2Ds[i] = null;


            Drawable2Ds.Clear();
            ScreenSpaceDrawable2Ds.Clear();
            //GC.Collect();

            tradeMappedToLayout.Clear();

            layout = new Sprite("market_grid", new Vector2(Core.graphics.PreferredBackBufferWidth / 2+80, Core.graphics.PreferredBackBufferHeight / 2), Color.White);
            layout.SetOriginCenter();
            marketNameHorizontalDivider = new Sprite("long_horizontal_divider", new Vector2(10, 28), Color.White, true);
            marketName = new Text2D("Terminus", "?", new Vector2(10, marketNameHorizontalDivider.Position.Y - 22), Color.White, true);
            backToGameplayButton = new Button("button", "Terminus", new Vector2(marketNameHorizontalDivider.Position.X, marketNameHorizontalDivider.Position.Y + 10), "Back", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            backToGameplayButton.Click += ViewGameplay;
            marketTabulatedText = new Text2DTabInteractable(20, 5, new Vector2((layout.Position.X - layout.Width / 2) + 2, (layout.Position.Y - layout.Height / 2) + 34), new Vector2(5,7), new int[] { 254, 189, 126, 126, 126 }, 30, 1, 2);
            marketTabulatedText.Construct("Terminus", true, "");

            selectedText = new Text2D("Terminus", "Selected: ", new Vector2(layout.Position.X-layout.Width/2, layout.Position.Y+layout.Height/2+10), Color.White);
            foreach(ButtonSimple b in marketTabulatedText.Buttons) {
                b.Click += delegate (object s, EventArgs e) {
                    UpdateSelected(s, e, marketTabulatedText.SelectedIndex);
                };
            }

            buyButton = new Button("buy_button", "Terminus", new Vector2(layout.Position.X-23, layout.Position.Y+layout.Height/2+10), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            buyButton.Click += delegate (object s, EventArgs e) {
                BuySellWindow(s, e, "buy");
            };
            sellButton = new Button("sell_button", "Terminus", new Vector2(layout.Position.X + 22, layout.Position.Y + layout.Height / 2 + 10), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            sellButton.Click += delegate (object s, EventArgs e) {
                BuySellWindow(s, e, "sell");
            };

            buyingWindow = new Sprite("buying_window", new Vector2(10, 100), Color.White);
            buyingWindow.DoDraw = false;
            sellingWindow = new Sprite("selling_window", new Vector2(10, 100), Color.White);
            sellingWindow.DoDraw = false;
            plusButton = new Button("plus_button", "Terminus", new Vector2(314, 129), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true, 0.11f);
            plusButton.Click += delegate (object s, EventArgs e) {
                AdjustTransactionQuantity(s, e, 1);
            };
            plusButton.SetActiveStatus(false);
            minusButton = new Button("minus_button", "Terminus", new Vector2(314, 141), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true, 0.11f);
            minusButton.Click += delegate (object s, EventArgs e) {
                AdjustTransactionQuantity(s, e, -1);
            };
            minusButton.SetActiveStatus(false);
            okButton = new Button("ok_button", "Terminus", new Vector2(110, 205), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            okButton.Click += OkBuySellWindow;
            okButton.SetActiveStatus(false);
            cancelButton = new Button("cancel_button", "Terminus", new Vector2(171, 205), "", Color.White, new Color(53, 82, 120, 255), new Color(22, 81, 221, 255), true);
            cancelButton.Click += CancelBuySellWindow;
            cancelButton.SetActiveStatus(false);
            buyingSellWindowTitle = new Text2D("Terminus", "Title", new Vector2(99, 103), Color.White);
            buyingSellWindowTitle.DoDraw = false;

            buyingSellingTabulatedText = new Text2DTab(2, 2, new Vector2(15, 136), new int[] { 174, 134 }, 21);
            buyingSellingTabulatedText.Construct("Terminus");
            foreach (Text2D textTab in buyingSellingTabulatedText.Text2Ds) {
                if (textTab != null) {
                    textTab.DoDraw = false;
                }
            }

            totalTabulatedText = new Text2DTab(1, 2, new Vector2(15, 179), new int[] { 174, 174 }, 21);
            totalTabulatedText.Construct("Terminus");
            foreach (Text2D textTab in totalTabulatedText.Text2Ds) {
                if (textTab != null) {
                    textTab.DoDraw = false;
                }
            }

            base.Initialize();
        }

        public void UpdateBuySellWindow() {
            if (actionType == "buy") {
                buyingSellingTabulatedText.Text2Ds[0, 0].Text = $"Quantity:";
                buyingSellingTabulatedText.Text2Ds[1, 0].Text = $"{transactionQuantity}";
                buyingSellingTabulatedText.Text2Ds[0, 1].Text = $"Price Per Unit:";
                buyingSellingTabulatedText.Text2Ds[1, 1].Text = $"{selected.Value.buy} Cr";
                totalTabulatedText.Text2Ds[0, 0].Text = $"Total:";
                totalTabulatedText.Text2Ds[1, 0].Text = $".";
            }
            else if (actionType == "sell") {
                buyingSellingTabulatedText.Text2Ds[0, 0].Text = $"Quantity:";
                buyingSellingTabulatedText.Text2Ds[1, 0].Text = $"{transactionQuantity}";
                buyingSellingTabulatedText.Text2Ds[0, 1].Text = $"Price Per Unit:";
                buyingSellingTabulatedText.Text2Ds[1, 1].Text = $"{selected.Value.sell} Cr";
            }
            else {
                throw new ArgumentException("Invalid market action");
            }
        }
        
        public void ToggleBuySellWindow(bool status) {
            buyingWindow.DoDraw = status;
            sellingWindow.DoDraw = status;
            plusButton.SetActiveStatus(status);
            minusButton.SetActiveStatus(status);
            okButton.SetActiveStatus(status);
            cancelButton.SetActiveStatus(status);
            buyingSellWindowTitle.DoDraw = status;
            foreach (Text2D textTab in buyingSellingTabulatedText.Text2Ds) {
                if (textTab != null) {
                    textTab.DoDraw = status;
                }
            }
            foreach (Text2D textTab in totalTabulatedText.Text2Ds) {
                if (textTab != null) {
                    textTab.DoDraw = status;
                }
            }
        }

        public void BuySellWindow(object s, EventArgs e, string actionType) {
            this.actionType = actionType;
            transactionQuantity = 1;

            if (!selected.Equals(default(KeyValuePair<TradeGood, BuySellValue>))) {
                buyingSellWindowTitle.Text = $"{selected.Key.Name}";
            }

            ToggleBuySellWindow(false);
            UpdateBuySellWindow();
            ToggleBuySellWindow(true);

            if (actionType == "buy") {
                sellingWindow.DoDraw = false;
            }
            else if (actionType == "sell") {
                buyingWindow.DoDraw = false;
            }
        }

        public void AdjustTransactionQuantity(object s, EventArgs e, int amount) {
            if (transactionQuantity + amount >= 1 && transactionQuantity + amount <= selected.Key.Quantity) {
                transactionQuantity += amount;
                UpdateBuySellWindow();
            }
        }

        public void OkBuySellWindow(object s, EventArgs e) {
            if (actionType == "buy") {
                selected.Key.Remove((uint)transactionQuantity);
            }
            else if (actionType == "sell") {
                selected.Key.Add((uint)transactionQuantity);
            }
            int row = 0;
            foreach (KeyValuePair<int, KeyValuePair<TradeGood, BuySellValue>> kvp in tradeMappedToLayout) {
                marketTabulatedText.Text2Ds[0, row].Text = $"{kvp.Value.Key.Name}";
                marketTabulatedText.Text2Ds[1, row].Text = $"{kvp.Value.Key.TType}";
                marketTabulatedText.Text2Ds[2, row].Text = $"{kvp.Value.Key.Quantity}";
                marketTabulatedText.Text2Ds[3, row].Text = $"{kvp.Value.Value.buy}";
                marketTabulatedText.Text2Ds[4, row].Text = $"{kvp.Value.Value.sell}";

                row++;
            }

            ToggleBuySellWindow(false);
        }

        public void CancelBuySellWindow(object s, EventArgs e) {
            ToggleBuySellWindow(false);
        }

        public void ViewGameplay(object s, EventArgs e) {
            ScreenManager.SwitchScreen("GameplayScreen");
        }

        public void UpdateSelected(object s, EventArgs e, int[] selectedIndex) {
            tradeMappedToLayout.TryGetValue(selectedIndex[1], out selected);

            if (!selected.Equals(default(KeyValuePair<TradeGood, BuySellValue>))) {
                selectedText.Text = $"Selected: {tradeMappedToLayout[selectedIndex[1]].Key.Name}; Row: {selectedIndex[1]}";
            }
        }

        public void ReadMarket(Market market) {
            marketName.Text = market.Name;
            int row = 0;
            foreach(KeyValuePair<TradeGood, BuySellValue> kvp in market.tradeGoodsForSale) {
                marketTabulatedText.Text2Ds[0, row].Text = $"{kvp.Key.Name}";
                marketTabulatedText.Text2Ds[1, row].Text = $"{kvp.Key.TType}";
                marketTabulatedText.Text2Ds[2, row].Text = $"{kvp.Key.Quantity}";
                marketTabulatedText.Text2Ds[3, row].Text = $"{kvp.Value.buy}";
                marketTabulatedText.Text2Ds[4, row].Text = $"{kvp.Value.sell}";

                tradeMappedToLayout.Add(row, kvp);

                row++;
            }
        }

        public override void Update(GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState) {
            backToGameplayButton.Update(Mouse.GetState());
            marketTabulatedText.Update(Mouse.GetState());
            buyButton.Update(Mouse.GetState());
            sellButton.Update(Mouse.GetState());
            plusButton.Update(Mouse.GetState());
            minusButton.Update(Mouse.GetState());
            okButton.Update(Mouse.GetState());
            cancelButton.Update(Mouse.GetState());

            base.Update(gameTime, NewKeyState, OldKeyState);
        }
    }
}