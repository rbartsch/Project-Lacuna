using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacuna.StationServices;

namespace Lacuna.AstronomicalObjects
{
    public class Station : AstronomicalObject {
        public AstronomicalObject Parent { get; set; }
        public List<IStationService> Services { get; set; }

        public Station(string fullName, AstronomicalObject parent) : base(fullName) {
            Parent = parent;
        }
    }
}
