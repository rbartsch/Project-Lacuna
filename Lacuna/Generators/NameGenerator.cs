using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.Generators {
    public class NameGenerator {
        // V
        private readonly string[] vowels = { "a", "e", "i", "o", "u" };
        // VD
        private readonly string[] vowelDigraphs = { "ai", "au", "aw", "ay", "ea", "ee", "ei", "eu", "ew", "ey", "ie", "oi", "oo", "ou", "ow", "oy" };
        // C
        private readonly string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
        // CD - usually for beginning words
        private readonly string[] consonantDigraphs = { "bl", "br", "ch", "ck", "cl", "cr", "dr", "fl", "fr", "gh", "gl", "gr", "ng", "ph", "pl", "pr", "qu", "sc", "sh", "sk", "sl", "sm", "sn", "sp", "st", "sw", "th",
            "tr", "tw", "wh", "wr" };
        // CT - usually for beginning words
        private readonly string[] consonantTrigraphs = { "nth", "sch", "scr", "shr", "spl", "spr", "squ", "str", "thr" };
        // GLN - usually used as first word      
        private readonly string[] greekLetterNames = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta", "Iota", "Kappa", "Lambda", "Mu", "Nu", "Xi", "Omicron", "Pi", "Rho", "Sigma", "Tau", "Upsilon",
            "Phi", "Chi", "Psi", "Omega" };
        // MN - usually used as last word
        private readonly string[] miscNames = { "Major", "Minor" };
        // FA - usually use after consonant letter
        private readonly string[] flairAffixes = { "er", "ury", "ion", "opa", "asi", "esi", "isi", "osi", "usi", "as", "es", "is", "os", "us", "on", "ov", "uv", "ald", "eld", "ild", "old", "uld", "alt", "elt", "ilt",
            "olt", "ult", "alv", "elv", "ilv", "olv", "ulv", "agh", "egh", "igh", "ogh", "ugh", "ayt", "eyt", "iyt", "oyt", "uyt", "and", "end", "ind", "ond", "und" };
        // S
        private readonly string[] symbols = { "@", "#", "-" };

        // Format must be comma-delimited. Format is executed from left to right order
        // Can include spaces, i.e GLN, ,C,V,C,V could yield Alpha Tera
        // If an incorrect format is used, "?" will be added in place
        public string GenerateAstroObjName(string format = "C,V,C,V") {
            string[] formatType = format.Split(',');

            StringBuilder sb = new StringBuilder();

            foreach (string s in formatType) {
                switch (s) {
                    case "V":
                        sb.Append(vowels[Rng.Random.Next(0, vowels.Length)]);
                        break;

                    case "VD":
                        sb.Append(vowelDigraphs[Rng.Random.Next(0, vowelDigraphs.Length)]);
                        break;

                    case "C":
                        sb.Append(consonants[Rng.Random.Next(0, consonants.Length)]);
                        break;

                    case "CD":
                        sb.Append(consonantDigraphs[Rng.Random.Next(0, consonantDigraphs.Length)]);
                        break;

                    case "CT":
                        sb.Append(consonantTrigraphs[Rng.Random.Next(0, consonantTrigraphs.Length)]);
                        break;

                    case "GLN":
                        sb.Append(greekLetterNames[Rng.Random.Next(0, greekLetterNames.Length)]);
                        break;

                    case "MN":
                        sb.Append(miscNames[Rng.Random.Next(0, miscNames.Length)]);
                        break;

                    case "FA":
                        sb.Append(flairAffixes[Rng.Random.Next(0, flairAffixes.Length)]);
                        break;

                    case "S":
                        sb.Append(symbols[Rng.Random.Next(0, symbols.Length)]);
                        break;

                    case " ":
                        sb.Append(" ");
                        break;

                    default:
                        sb.Append("?");
                        break;
                }
            }

            string result = sb.ToString();
            result = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(result.ToLower());
            return result;
        }

        public string GenerateAstroObjNameRandom() {
            string[] formats = {
                "C,V,C",
                "GLN, ,C,V,C",
                "C,V,C, ,MN",
                "GLN, ,C,V,C, ,MN",

                "C,V,C,FA",
                "GLN, ,C,V,C,FA",
                "C,V,C,FA, ,MN",
                "GLN, ,C,V,C,FA, ,MN",

                "C,V,C,V",
                "GLN, ,C,V,C,V",
                "C,V,C,V, ,MN",
                "GLN, ,C,V,C,V, ,MN",

                "C,V,C,V,FA",
                "GLN, ,C,V,C,V,FA",
                "C,V,C,V,FA, ,MN",
                "GLN, ,C,V,C,V,FA, ,MN",

                "V,C,V",
                "GLN, ,V,C,V",
                "V,C,V, ,MN",
                "GLN, ,V,C,V, ,MN",

                "V,C,V,FA",
                "GLN, ,V,C,V,FA",
                "V,C,V,FA, ,MN",
                "GLN, ,V,C,V,FA, ,MN",

                "V,C,V,C",
                "GLN, ,V,C,V,C",
                "V,C,V,C, ,MN",
                "GLN, ,V,C,V,C, ,MN",

                "V,C,V,C,FA",
                "GLN, ,V,C,V,C,FA",
                "V,C,V,C,FA, ,MN",
                "GLN, ,V,C,V,C,FA, ,MN",

                "CD,VD,C",
                "GLN, ,CD,VD,C",
                "CD,VD,C, ,MN",
                "GLN, ,CD,VD,C, ,MN",

                "CD,VD,C,FA",
                "GLN, ,CD,VD,C,FA",
                "CD,VD,C,FA, ,MN",
                "GLN, ,CD,VD,C,FA, ,MN",

                "CD,VD,C,V",
                "GLN, ,CD,VD,C,V",
                "CD,VD,C,V, ,MN",
                "GLN, ,CD,VD,C,V, ,MN",

                "CD,VD,C,V,FA",
                "GLN, ,CD,VD,C,V,FA",
                "CD,VD,C,V,FA, ,MN",
                "GLN, ,CD,VD,C,V,FA, ,MN",

                "VD,CD,V",
                "GLN, ,VD,CD,V",
                "VD,CD,V, ,MN",
                "GLN, ,VD,CD,V, ,MN",

                "VD,CD,V,FA",
                "GLN, ,VD,CD,V,FA",
                "VD,CD,V,FA, ,MN",
                "GLN, ,VD,CD,V,FA, ,MN",

                "VD,CD,V,C",
                "GLN, ,VD,CD,V,C",
                "VD,CD,V,C, ,MN",
                "GLN, ,VD,CD,V,C, ,MN",

                "VD,CD,V,C,FA",
                "GLN, ,VD,CD,V,C,FA",
                "VD,CD,V,C,FA, ,MN",
                "GLN, ,VD,CD,V,C,FA, ,MN",

                "CT,VD,CD",
                "GLN, ,CT,VD,CD",
                "CT,VD,CD, ,MN",
                "GLN, ,CT,VD,CD, ,MN",

                "CT,VD,CD,FA",
                "GLN, ,CT,VD,CD,FA",
                "CT,VD,CD,FA, ,MN",
                "GLN, ,CT,VD,CD,FA, ,MN",

                //"CT,VD,CD,V,C",
                //"GLN, ,CT,VD,CD,V,C",
                //"CT,VD,CD,V,C, ,MN",
                //"GLN, ,CT,VD,CD,V,C, ,MN",

                //"CT,VD,CD,V,C,FA",
                //"GLN, ,CT,VD,CD,V,C,FA",
                //"CT,VD,CD,V,C,FA, ,MN",
                //"GLN, ,CT,VD,CD,V,C,FA, ,MN",
            };

            string[] formatResult;

            // GLN
            if (Rng.Chance(10)) {
                formatResult = formats.Where(x => x.StartsWith("GLN") && !x.EndsWith("MN")).ToArray();
            }
            // MN
            else if (Rng.Chance(10)) {
                formatResult = formats.Where(x => x.EndsWith("MN") && !x.StartsWith("GLN")).ToArray();
            }
            // GLN and MN
            else if (Rng.Chance(5)) {
                formatResult = formats.Where(x => x.StartsWith("GLN") && x.EndsWith("MN")).ToArray();
            }
            else {
                formatResult = formats.Where(x => !x.StartsWith("GLN") && !x.EndsWith("MN")).ToArray();
            }

            string selectedFormat = formatResult[Rng.Random.Next(0, formatResult.Length)];
            return GenerateAstroObjName(selectedFormat);
        }
    }
}