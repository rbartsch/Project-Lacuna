/* Linear conversion, convert from one scale range to another maintaining ratio
 * e.g Match -8.0f..-4.0f to 0.0f..5.0f
 * 0	= (((-8)-(-8)) / ((-4)-(-8))) * ((5)-(0)) + (0)
 * 1.25 = (((-7)-(-8)) / ((-4)-(-8))) * ((5)-(0)) + (0)
 * 2.5	= (((-6)-(-8)) / ((-4)-(-8))) * ((5)-(0)) + (0)
 * 3.75 = (((-5)-(-8)) / ((-4)-(-8))) * ((5)-(0)) + (0)
 * 5.0	= (((-4)-(-8)) / ((-4)-(-8))) * ((5)-(0)) + (0)
 */

namespace Lacuna {
    public static class LinearConversion {
        public static float Float(float oldValue, float oldMin, float oldMax, float newMin, float newMax) {
            return (((oldValue) - (oldMin)) / ((oldMax) - (oldMin))) * ((newMax) - (newMin)) + (newMin);
        }

        public static int Integer(int oldValue, int oldMin, int oldMax, int newMin, int newMax) {
            return (((oldValue) - (oldMin)) / ((oldMax) - (oldMin))) * ((newMax) - (newMin)) + (newMin);
        }
    }
}