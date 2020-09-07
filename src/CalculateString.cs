using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace rnd
{
    public class CalculateString
    {
        /// <summary>
        /// If the length is larger than or equal to this, the program will use all cpu threads to calculate the string
        /// </summary>
        public static int multiThreadedLength = 2;

        public static string s_MakeString(int length, char[] characterSet)
        {
            if (length >= multiThreadedLength) return sm_MakeThreadedString(length, characterSet);

            return sm_MakeString(length, characterSet);
        }

        /// <summary>
        /// Make a random string using one thread
        /// </summary>
        /// <param name="length">Length of string</param>
        /// <param name="characterSet">Characters to make the string from</param>
        /// <returns>Random string</returns>
        protected static string sm_MakeString(int length, char[] characterSet)
        {
            List<char> str = new List<char>();

            sm_Transaction(ref str, characterSet, length, RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));

            return new string(str.ToArray());
        }

        /// <summary>
        /// Make a random string using all cpu threads
        /// </summary>
        /// <param name="length">Length of string</param>
        /// <param name="characterSet">Characters to make the string from</param>
        /// <returns>Random string</returns>
        protected static string sm_MakeThreadedString(int length, char[] characterSet)
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
                    sm_Transaction(ref str[p], characterSet, (p + 1 == threads) ? lastThreadLength : lenPerThread, RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));
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
        /// Adds random info to a List<char> using a given seed for a System.Random
        /// </summary>
        /// <param name="str">The List<char> to add too</param>
        /// <param name="charSet">The character set to add to str</param>
        /// <param name="len">Number of items to add to str</param>
        /// <param name="seed">The seed to be given to the System.Random (please calc this using crypto)</param>
        protected static void sm_Transaction(ref List<char> str, char[] charSet, int len, int seed)
        {
            Random rnd = new Random(seed);

            for (int i = 0; i < len; i++)
            {
                str.Add(charSet[rnd.Next(charSet.Length)]);
            }
        }
    }
}