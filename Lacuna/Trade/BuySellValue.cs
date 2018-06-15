using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.Trade {
    public class BuySellValue {
        public long buy;
        public long sell;

        public BuySellValue(long buy = 0, long sell = 0) {
            this.buy = buy;
            this.sell = sell;
        }
    }
}