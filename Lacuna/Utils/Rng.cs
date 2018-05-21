using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna {
    // TODO: Refactor to normal class away from static
    public static class Rng {
        public static Random Random { get; private set; } = new Random();

        // ------------------------------------------------------------------------------------------
        public static void Initialize() {
            Random = new Random();
        }

        // ------------------------------------------------------------------------------------------
        public static void InitializeWithSeed(int seed) {
            Random = new Random(seed);
        }

        // ------------------------------------------------------------------------------------------
        public static bool Chance(int percentage) {
            if (Random.Next(0, 101) <= percentage)
                return true;
            else
                return false;
        }

        public static class Dice {
            //My implementation of Amit Patel's resource for dice rolling and probabilities http://www.redblobgames.com/articles/probability/damage-rolls.html

            /// <summary>
            /// Basic roll. Scale is from 1 to x
            /// </summary>
            /// <param name="nDie">Amount of dice</param>
            /// <param name="nSide">Sides per die</param>
            /// <param name="constantShift">Optional shift of entire distribution left or right whether it's negative or positive</param>
            /// <returns>int</returns>
            public static int RollSymmetric(int nDie, int nSide, int constantShift = 0) {
                int result = 0;
                for (int i = 0; i < nDie; i++)
                    result += 1 + Random.Next(0, nSide);

                return result + constantShift < 0 ? 0 : result + constantShift; // Prevent from becoming negative on scale, otherwise just return result + constantShift. Only matters depending on desire of constantShift?
            }

            /// <summary>
            /// Roll with distribution variance. Scale is from 0 to x
            /// </summary>
            /// <param name="nDie">Amount of dice</param>
            /// <param name="nSide">Sides per die</param>
            /// <param name="constantShift">Optional shift of entire distribution left or right whether it's negative or positive</param>
            /// <returns>int</returns>
            public static int RollSymmetricDistributedVariance(int nDie, int nSide, int constantShift = 0) {
                int result = 0;
                for (int i = 0; i < nDie; i++)
                    result += Random.Next(0, nSide + 1);

                return result + constantShift < 0 ? 0 : result + constantShift; // Prevent from becoming negative on scale, otherwise just return result + constantShift. Only matters depending on desire of constantShift?
            }

            /// <summary>
            /// Drop the lowest roll. Higher-than-average values more common than lower-than-average values
            /// </summary>
            /// <param name="nDie">Amount of dice</param>
            /// <param name="nSide">Sides per die</param>
            /// <param name="nRoll">How many times to roll</param>
            /// <param name="constantShift">Optional shift of entire distribution left or right whether it's negative or positive</param>
            /// <returns>int</returns>
            public static int RollAsymmetricHigherAvg(int nDie, int nSide, int nRoll, int constantShift = 0) {
                int result = 0;
                int[] rolls = new int[nRoll];

                for (int i = 0; i < nRoll; i++) {
                    rolls[i] = RollSymmetricDistributedVariance(nDie, nSide, constantShift);
                    result += rolls[i];
                }

                return result = result - rolls.Max();
            }

            /// <summary>
            /// Drop the highest roll. Lower-than-average values more common than higher-than-average values
            /// </summary>
            /// <param name="nDie">Amount of dice</param>
            /// <param name="nSide">Sides per die</param>
            /// <param name="nRoll">How many times to roll</param>
            /// <param name="constantShift">Optional shift of entire distribution left or right whether it's negative or positive</param>
            /// <returns>int</returns>
            public static int RollAsymmetricLowerAvg(int nDie, int nSide, int nRoll, int constantShift = 0) {
                int result = 0;
                int[] rolls = new int[nRoll];

                for (int i = 0; i < nRoll; i++) {
                    rolls[i] = RollSymmetricDistributedVariance(nDie, nSide, constantShift);
                    result += rolls[i];
                }

                return result = result - rolls.Max();
            }

            /// <summary>
            /// Roll with a bonus result added x% of the time
            /// </summary>
            /// <param name="percentage">Chance, x# of the time. i.e 30% for critical hit</param>
            /// <param name="nDie">Amount of dice</param>
            /// <param name="nSide">Sides per die</param>
            /// <param name="constantShift">Optional shift of entire distribution left or right whether it's negative or positive</param>
            /// <returns>int</returns>
            public static int RollCritical(int percentage, int nDie, int nSide, int constantShift = 0) {
                int result = RollSymmetricDistributedVariance(nDie, nSide, constantShift);
                if (Random.Next(0, 101) <= percentage)
                    result += RollSymmetricDistributedVariance(nDie, nSide, constantShift);

                return result;
            }
        }
    }
}