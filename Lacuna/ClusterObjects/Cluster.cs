using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.AstronomicalObjects;

namespace Lacuna.ClusterObjects {
    public class Cluster {
        public List<PlanetarySystem> PlanetarySystems { get; set; } = new List<PlanetarySystem>();
    }
}