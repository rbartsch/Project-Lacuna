using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.Commodities;
using Lacuna.Trade;

namespace Lacuna.StationServices {
    public class Market : IStationService {
        public string Name { get; private set; }

        Dictionary<TradeGood, BuySellValue> tradeGoodsForSale = new Dictionary<TradeGood, BuySellValue>();

        public Market(string name) {
            Name = name;

            TradeGood t = TradeGoodList.GetWithName("Military Ration").CloneWithNewQuantity(5) as TradeGood;
            // for now use ints, need to make a random int64 generator
            tradeGoodsForSale.Add(t, new BuySellValue(t.BasePrice.buy + Rng.Random.Next(0, 51), t.BasePrice.sell + Rng.Random.Next(50, 101)));
        }

        public void SeedMarket() {
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
