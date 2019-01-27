using System;

namespace DynamicConnections
{
    public static class Helper
    {
        /// <summary>
        /// Randommize the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        public static void Randomize<T>(T[] items)
        {
            Random rnd = new Random();

            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rnd.Next(i, items.Length);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }

        /// <summary>
        /// Returns a random int variable
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomInt(int min, int max)
        {
            Random rnd = new Random();

            return rnd.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a random float variable
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double RandomFloat(double min, double max)
        {
            Random rnd = new Random();

            return (min + rnd.NextDouble() * (min - max));
        }

    }
}
