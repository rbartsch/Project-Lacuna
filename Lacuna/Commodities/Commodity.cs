using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.Commodities {
    public abstract class Commodity {
        public string Name { get; set; }
        public long Quantity { get; set; }
    }
}