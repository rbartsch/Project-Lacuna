using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.Trade;

namespace Lacuna.StationServices {
    public class Market : IStationService {
        public string Name { get; private set; }

        public Dictionary<TradeGood, BuySellValue> tradeGoodsForSale = new Dictionary<TradeGood, BuySellValue>();

        public Market(string name) {
            Name = name;

            SeedMarket();
        }

        public void SeedMarket() {
            foreach(TradeGood t in TradeGoodList.tradeGoods) {
                if (Rng.Chance(70)) {
                    // for now use integers, need to make a random int64 generator
                    TradeGood copy = t.CloneWithNewQuantity(Rng.Random.Next(0, 10000)) as TradeGood;
                    tradeGoodsForSale.Add(copy, new BuySellValue(t.BasePrice.buy + Rng.Random.Next(50, 101), t.BasePrice.sell + Rng.Random.Next(0, 51)));
                }
            }
        }

        // Remove commodities from list if stock depleted? Or leave as-is?
        public void Clean() {

        }

        public void View() {
            throw new NotImplementedException();
        }

        public void Use() {
            throw new NotImplementedException();
        }
    }
}
