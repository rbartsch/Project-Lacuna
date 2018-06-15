using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.Trade;

namespace Lacuna.Trade {
    public class TradeGood : ICloneable {
        public string Name { get; private set; }
        public long Quantity { get; private set; }
        public TradeGoodType TType { get; private set; }
        public BuySellValue BasePrice { get; private set; }

        public TradeGood(string name, long quantity, TradeGoodType type, BuySellValue basePrice) {
            Name = name;
            Quantity = quantity;
            TType = type;
            BasePrice = basePrice;
        }

        public void Add(uint amount) {
            Quantity += amount;
        }

        public void Remove(uint amount) {
            if (Quantity - amount >= 0) {
                Quantity -= amount;
            }
        }

        public object Clone() {
            return new TradeGood(Name, Quantity, TType, BasePrice);
        }

        public object CloneWithNewQuantity(long quantity) {
            return new TradeGood(Name, quantity, TType, BasePrice);
        }
    }
}