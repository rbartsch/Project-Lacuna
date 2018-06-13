using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.Trade;

namespace Lacuna.Commodities {
    public static class TradeGoodList {
        public static List<TradeGood> tradeGoods;

        public static void Populate() {
            tradeGoods = new List<TradeGood>();
            tradeGoods.Add(new TradeGood("Military Ration", 1, TradeGoodType.Food, new BuySellValue(90, 100)));
            tradeGoods.Add(new TradeGood("Colony Foods Crate Type-A", 1, TradeGoodType.Food, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Colony Foods Crate Type-B", 1, TradeGoodType.Food, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Antibiotics", 1, TradeGoodType.Medicine, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Analgesics", 1, TradeGoodType.Medicine, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Antiseptics", 1, TradeGoodType.Medicine, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Medical Equipment", 1, TradeGoodType.Medicine, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Pistol", 1, TradeGoodType.Weapons, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Assault Rifle", 1, TradeGoodType.Weapons, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Nickel", 1, TradeGoodType.RawMaterials, new BuySellValue()));
            tradeGoods.Add(new TradeGood("Fuel Cell", 1, TradeGoodType.Energy, new BuySellValue()));
        }

        public static TradeGood GetWithName(string name) {
            TradeGood t = tradeGoods.Find(x => x.Name == name);
            if(t == null) {
                throw new Exception("Invalid TradeGood name");
            }
            else {
                return t;
            }
        }

        public static List<TradeGood> GetOfType(TradeGoodType type) {
            return tradeGoods.FindAll(x => x.TType == type);
        }
    }
}
