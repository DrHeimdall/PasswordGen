using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace rnd
{
    public class CalculateString
    {
        #region String Generation

        /// <summary>
        /// If the length is larger than or equal to this, the program will use all cpu threads to calculate the string
        /// </summary>
        public static int multiThreadedLength = 15000;

        public static string MakeString(int length, char[] characterSet)
        {
            if (length >= multiThreadedLength) return MakeThreadedString(length, characterSet);

            return MakeNonThreadedString(length, characterSet);
        }

        /// <summary>
        /// Make a random string using one thread
        /// </summary>
        /// <param name="length">Length of string</param>
        /// <param name="characterSet">Characters to make the string from</param>
        /// <returns>Random string</returns>
        protected static string MakeNonThreadedString(int length, char[] characterSet)
        {
            List<char> str = new List<char>();

            Transaction(ref str, characterSet, length, RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));

            return new string(str.ToArray());
        }

        /// <summary>
        /// Make a random string using all cpu threads
        /// </summary>
        /// <param name="length">Length of string</param>
        /// <param name="characterSet">Characters to make the string from</param>
        /// <returns>Random string</returns>
        protected static string MakeThreadedString(int length, char[] characterSet)
        {
            int threads = Environment.ProcessorCount;
            int lenPerThread = length / threads;
            int lastThreadLength = length - ((threads - 1) * lenPerThread);

            List<char>[] str = new List<char>[threads];

            Task[] tasks = new Task[threads];

            for (int i = 0; i < threads; i++)
            {
                int p = i;

                str[i] = new List<char>();

                tasks[i] = Task.Factory.StartNew(() =>
                {
                    Transaction(ref str[p], characterSet, (p + 1 == threads) ? lastThreadLength : lenPerThread, RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));
                });
            }

            Task.WaitAll(tasks);

            List<char> finalStr = new List<char>();

            // Consoledate information
            for (int i = 0; i < str.Length; i++)
            {
                finalStr.AddRange(str[i]);
            }

            return new string(finalStr.ToArray());
        }

        /// <summary>
        /// Adds random info to a List(char) using a given seed for a System.Random
        /// </summary>
        /// <param name="str">The List(char) to add too</param>
        /// <param name="charSet">The character set to add to str</param>
        /// <param name="len">Number of items to add to str</param>
        /// <param name="seed">The seed to be given to the System.Random (please calc this using crypto)</param>
        protected static void Transaction(ref List<char> str, char[] charSet, int len, int seed)
        {
            Random rnd = new Random(seed);

            for (int i = 0; i < len; i++)
            {
                str.Add(charSet[rnd.Next(charSet.Length)]);
            }
        }

        #endregion

        #region Random Number Generation

        /// <summary>
        /// Generates a random number
        /// </summary>
        /// <param name="min">Must be either double or long</param>
        /// <param name="max">Must be either double or long</param>
        /// <returns>The random number</returns>
        public static string MakeNumber(object min, object max)
        {
            Random rnd = new Random(RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));

            if (min is long && max is long)
            {
                return RndLong((long)min, (long)max, rnd);
            }
            else if (min is long && max is double)
            {
                return RndDouble((double)(long)min, (double)max, rnd);
            }
            else if (min is double && max is long)
            {
                return RndDouble((double)min, (double)(long)max, rnd);
            }
            else if (min is double && max is double)
            {
                return RndDouble((double)min, (double)max, rnd);
            }

            return "";
        }

        // From https://stackoverflow.com/questions/6651554/random-number-in-long-range-is-this-the-way?answertab=active#6651661
        // Flawed, returns a 63 bit number.
        /// <summary>
        /// Lies! This is a 63 bit random number function.
        /// Returns a 63 bit number and converts the params to 63 bit if they are bigger than 64.
        /// </summary>
        public static string RndLong(long min, long max, Random rnd)
        {
            if (min > max)
            {
                long swap = min; min = max; max = swap;
            }

            long maxVal = long.MaxValue / 2;

            if (min < -maxVal)
            {
                min = -maxVal;
            }

            if (max > maxVal)
            {
                max = maxVal;
            }

            byte[] bytes = new byte[8];
            rnd.NextBytes(bytes);
            long rand = BitConverter.ToInt64(bytes);

            return (Math.Abs(rand % ((max - min) + ((max != long.MaxValue) ? 1 : 0))) + min).ToString();
        }

        /// <summary>
        /// This does not at all work for edge cases
        /// </summary>
        public static string RndDouble(double min, double max, Random rnd)
        {
            if (min > max)
            {
                double swap = min; min = max; max = swap;
            }

            return (rnd.NextDouble() * (max - min) + min).ToString();
        }

        #endregion
    }
}
