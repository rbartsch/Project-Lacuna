using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.Commodities;

namespace Lacuna.StationServices {
    public class Market : IStationService {
        public string Name { get => throw new NotImplementedException(); private set => throw new NotImplementedException(); }

        struct BuySellValue {
            public double buy;
            public double sell;

            public BuySellValue(double buy, double sell) {
                this.buy = buy;
                this.sell = sell;
            }
        }

        Dictionary<TradeGood, BuySellValue> tradeGoodsForSale = new Dictionary<TradeGood, BuySellValue>();

        public Market(string name) {
            Name = name;

            tradeGoodsForSale.Add(TradeGoodList.GetWithName("Military Rations").CloneWithNewQuantity(5) as TradeGood, new BuySellValue(100, 105));
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
