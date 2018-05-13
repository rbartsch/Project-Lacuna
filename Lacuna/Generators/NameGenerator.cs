using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.Generators {
    public class NameGenerator {
        // STAR, PLANET, AND MOON NAME PAIRING GENERATION
        // Notes:
        // - Star has a name and uses alphabet letter if more than one, planet derives name from star name and uses roman numerals, 
        // moon derives from planet name and uses arabic numerals
        // - [Star] Aberulug a, [Planet] Aberulug a -> I, [Moon] Aberulug a -> I -> 1
        private readonly string[] pair0 = { "", "Ab", "Be", "Ch", "Di", "Ej", "Fl", "Gn", "Ho", "Ip", "Ju", "Kv", "Lw", "My" };
        private readonly string[] pair1 = { "a", "e", "i", "o", "u" };
        private readonly string[] pair2 = { "", "au", "bu", "cu", "du", "eu", "fu", "gu", "hu", "iu", "ju", "ku", "lu", "mu", "nu", "ou", "pu", "qu", "ru", "su", "tu", "uu", "vu", "wu", "xu", "yu", "zu" };
        private readonly string[] pair3 = { "", "aud", "bud", "cud", "dud", "eud", "fud", "gud", "hud", "iud", "jud", "kud", "lud", "mud", "nud", "oud", "pud", "qud", "rud", "sud", "tud", "uud", "vud", "wud", "xud", "yud", "zud" };
        private readonly string[] pair4 = { "k", "t", "ak", "aut", "ol", "ar", "lug", "wuk" };

        public string GeneratelAstronomicalObjectName() {
            StringBuilder sb = new StringBuilder();
            sb.Append(pair0[Rng.Random.Next(0, pair0.Length)]);
            sb.Append(pair1[Rng.Random.Next(0, pair1.Length)]);
            sb.Append(pair2[Rng.Random.Next(0, pair2.Length)]);
            sb.Append(pair3[Rng.Random.Next(0, pair3.Length)]);
            sb.Append(pair4[Rng.Random.Next(0, pair4.Length)]);
            sb.Replace(sb[0].ToString(), sb[0].ToString().ToUpper(), 0, 1);
            return sb.ToString();
        }
    }
}