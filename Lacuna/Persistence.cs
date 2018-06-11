using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.ClusterObjects;
using Lacuna.Generators;
using Lacuna.Commodities;

namespace Lacuna {
    public static class Persistence {
        public static List<Cluster> clusters = new List<Cluster>();

        public static void Initialize() {
            TradeGoodList.Populate();
            GenerateClusters();
        }

        public static void GenerateClusters() {
            clusters.Add(new ClusterGenerator().GenerateCluster());
        }
    }
}