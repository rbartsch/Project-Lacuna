using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.Commodities {
    public static class TradeGoodList {
        private static List<TradeGood> tradeGoods = new List<TradeGood>();

        public static void Populate() {
            tradeGoods.Add(new TradeGood("Military Ration", 1, TradeGoodType.Food));
            tradeGoods.Add(new TradeGood("Colony Foods Crate Type-A", 1, TradeGoodType.Food));
            tradeGoods.Add(new TradeGood("Colony Foods Crate Type-B", 1, TradeGoodType.Food));
            tradeGoods.Add(new TradeGood("Antibiotics", 1, TradeGoodType.Medicine));
            tradeGoods.Add(new TradeGood("Analgesics", 1, TradeGoodType.Medicine));
            tradeGoods.Add(new TradeGood("Antiseptics", 1, TradeGoodType.Medicine));
            tradeGoods.Add(new TradeGood("Medical Equipment", 1, TradeGoodType.Medicine));
            tradeGoods.Add(new TradeGood("Pistol", 1, TradeGoodType.Weapons));
            tradeGoods.Add(new TradeGood("Assault Rifle", 1, TradeGoodType.Weapons));
            tradeGoods.Add(new TradeGood("Nickel", 1, TradeGoodType.RawMaterials));
            tradeGoods.Add(new TradeGood("Fuel Cell", 1, TradeGoodType.Energy));
        }

        public static TradeGood GetWithName(string name) {
            return tradeGoods.Find(x => x.Name == name);
        }

        public static List<TradeGood> GetOfType(TradeGoodType type) {
            return tradeGoods.FindAll(x => x.TType == type);
        }
    }
}
