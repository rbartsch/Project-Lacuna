using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.ClusterObjects;
using Lacuna.Generators;

namespace Lacuna {
    public static class Persistence {
        public static Cluster cluster;

        public static void StartCluster() {
            cluster = new ClusterGenerator().GenerateCluster();
        }
    }
}