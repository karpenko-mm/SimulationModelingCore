using System;

namespace Modeling.Core {

    /// <summary>
    /// Random number generation helper
    /// </summary>
    public static class RandomGenerator {

        static readonly Random rnd = new Random(0);

        /// <summary>
        /// Generates a random floating point number in the range [min; max)
        /// </summary>
        public static double NextDouble(double min = 0, double max = 1) {
            return rnd.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Generates a random integer in the range [min; max)
        /// </summary>
        public static int NextInt(int min = 0, int max = Int32.MaxValue) {
            return rnd.Next(min, max);
        }

        /// <summary>
        /// Generate a boolean value with a given probability of a true value
        /// </summary>
        /// <param name = "probabilityForTrue">Probability of true value</param>
        public static bool NextBool(double probabilityForTrue = 0.5) {
            return rnd.NextDouble() < probabilityForTrue;
        }
    }
}
