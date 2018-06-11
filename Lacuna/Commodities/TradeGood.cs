using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.Commodities {
    public class TradeGood : ICloneable {
        public string Name { get; private set; }
        public long Quantity { get; private set; }
        public TradeGoodType TType { get; private set; }

        public TradeGood(string name, long quantity, TradeGoodType type) {
            Name = name;
            Quantity = quantity;
            TType = type;
        }

        public void Add() {
            Quantity++;
        }

        public void Remove() {
            if (Quantity > 0) {
                Quantity--;
            }
        }

        public object Clone() {
            return new TradeGood(Name, Quantity, TType);
        }

        public object CloneWithNewQuantity(long quantity) {
            return new TradeGood(Name, quantity, TType);
        }
    }
}