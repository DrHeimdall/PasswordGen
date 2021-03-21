using System;
using System.Text;
using System.Collections.Generic;

using static rnd.NumberPhonet;


namespace rnd
{
    class Generator : CalculateString
    {
        private static readonly uint MAX_CHARS = 5000000;
        public static string Alphabet { get => "qwertyuiopasdfghjklzxcvbnm" + "qwertyuiopasdfghjklzxcvbnm".ToUpper(); }
        public static string SafeChars { get => Alphabet + "0123456789"; }
        public static string ComplexChars { get => SafeChars + "!\"£$%^&*()-_=+[]{}#~`¬|\\<>,.?/™"; }

        protected readonly GenOptions Options;
        /// <summary>
        /// Contains only double or long.
        /// Anything else will break the program
        /// </summary>
        protected List<object> Lengths;
        protected Random Rnd;

        public Generator(GenOptions options, Queue<(string, int)> lengths)
        {
            this.Options = options;

            this.Lengths = new List<object>();
            ParseLengths(lengths);

            this.Rnd = new Random(System.Security.Cryptography.RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));
        }


        public string[] GeneratePasswords(out TimeSpan timeTaken)
        {
            // Choose what characters to make the password from
            char[] characterSet = (Options.Human) ? SafeChars.ToCharArray() : ComplexChars.ToCharArray();

            string[] passwords = new string[Lengths.Count];

            DateTime start = DateTime.UtcNow;

            for (int i = 0; i < Lengths.Count; i++)
            {
                // Cast to a long (what it is) then to an int (what it needs to be)
                passwords[i] = MakeString((int)(long)Lengths[i], characterSet);
            }

            DateTime end = DateTime.UtcNow;

            timeTaken = end - start;

            return passwords;
        }

        public string[] GenerateNumbers(out TimeSpan timeTaken)
        {
            string[] numbers;

            DateTime start = DateTime.UtcNow;

            if (Lengths.Count % 2 == 0)
            {
                numbers = new string[Lengths.Count / 2];
                Queue<object> lengthsQueue = new Queue<object>(Lengths);


                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = MakeNumber(lengthsQueue.Dequeue(), lengthsQueue.Dequeue());
                }
            }
            else
            {
                numbers = new string[1] { MakeNumber(Lengths[0], Lengths[1]) };
            }

            DateTime end = DateTime.UtcNow;

            timeTaken = end - start;

            return numbers;
        }

        public string[] GenerateAuto(out TimeSpan time)
        {
            string[] res = (Options.Number) ? GenerateNumbers(out time) : GeneratePasswords(out time);

            return res;
        }

        /// <summary>
        /// Parse the lengths for the generator to use
        /// </summary>
        /// <param name="lens">The user input</param>
        private void ParseLengths(Queue<(string, int)> lens)
        {
            // Expect pairs of numbers (min and max)
            // Or a single number (0 - max)
            // Or where min and max are variations on int.max etc.
            // If either abs(values) are greater than long.max then use double
            if (Options.Number)
            {
                ParseMinMaxPairs(lens);
            }
            // Expect single, ushort numbers
            else
            {
                ParseSingleIntLengths(lens);
            }
        }

        /// <summary>
        /// Parse lengths for "passwords"
        /// </summary>
        /// <param name="lens">The thing to be parsed</param>
        private void ParseSingleIntLengths(Queue<(string, int)> lens)
        {
            while (lens.Count != 0)
            {
                (string, int) val = lens.Dequeue();

                uint len;

                bool fail = !uint.TryParse(val.Item1, out len);

                // Bigger than what we allow
                if (len > MAX_CHARS || len == 0) fail = true;
                if (fail) { BadUshortFeedback(val.Item2); throw new Exception(); }

                // No error has been thrown, so we good
                Lengths.Add((long)len);
            }
        }

        /// <summary>
        /// Parse min-max pairs for random numbers
        /// </summary>
        /// <param name="lens">The data set to be parsed</param>
        private void ParseMinMaxPairs(Queue<(string, int)> lens)
        {
            // If there is just one value, then give it a min of 0 and a 'max' of that value
            if (lens.Count == 1)
            {
                // Must be long not int!
                Lengths.Add(0L);
                Lengths.Add(ParseSingleMinOrMax(lens.Dequeue()));
                return;
            }

            // There aren't pairs, throw an error
            if (lens.Count % 2 != 0) { BadPairsFeedback(); throw new Exception(); }

            // Parse the pairs
            while (lens.Count != 0)
            {
                Lengths.Add(ParseSingleMinOrMax(lens.Dequeue()));
            }
        }

        /// <summary>
        /// Parses a single min or max value
        /// </summary>
        /// <returns>The value. Either long or double</returns>
        private object ParseSingleMinOrMax((string, int) val)
        {
            // Add some special 
            switch (val.Item1.ToLower())
            {
                case "int.max":
                    return (long)int.MaxValue;
                case "int.min":
                    return (long)int.MinValue;
                case "uint.max":
                    return (long)uint.MaxValue;
                case "uint.min":
                    return (long)uint.MinValue;
                case "short.max":
                    return (long)short.MaxValue;
                case "short.min":
                    return (long)short.MinValue;
                case "ushort.max":
                    return (long)ushort.MaxValue;
                case "ushort.min":
                    return (long)ushort.MinValue;
                case "0":
                    return 0L;
                case "1":
                    return 1L;
                default:
                    return ParseNumber(val);
            }
        }

        /// <summary>
        /// Capable of parsing an integer, a decimal or an exponent (xEy type where y is int abs < 308)
        /// </summary>
        /// <param name="val"></param>
        /// <returns>The number!</returns>
        private object ParseNumber((string, int) val)
        {
            // Is an exponent
            if (val.Item1.ToUpper().Contains("E"))
            {
                return ParseExponent(val);
            }
            // Is a power
            if (val.Item1.ToUpper().Contains("^"))
            {
                return ParsePower(val);
            }
            // Is a normal value
            else
            {
                return ParseNormalNumber(val);
            }
        }

        /// <summary>
        /// Parses an exponent number
        /// </summary>
        /// <remarks>
        /// Throws generic on fail
        /// </remarks>
        /// <param name="val">The value to be parsed</param>
        private static double ParseExponent((string, int) val)
        {
            string[] parts = val.Item1.Split("E");

            short exponent = 0;

            // Is it likely bigger than what double can take?
            bool fail = !short.TryParse(parts[1], out exponent);
            if (exponent >= 308 || exponent <= -324) fail = true;
            if (fail) { InvalidExponentFeedback(val.Item2); throw new Exception(); }

            decimal baseNumber = decimal.Zero;

            // Is it badly formatted?
            fail = !decimal.TryParse(parts[0], out baseNumber);
            if (baseNumber >= 10 || baseNumber <= -10) fail = true;
            if (fail) { InvalidBaseFeedback(val.Item2); throw new Exception(); }

            double k = Math.Pow(10, exponent);

            return (double)(baseNumber * (decimal)k);
        }

        /// <summary>
        /// Parses a power
        /// </summary>
        /// <remarks>
        /// Throws generic on fail
        /// </remarks>
        /// <param name="val">The value to be parsed</param>
        private static double ParsePower((string, int) val)
        {
            string[] parts = val.Item1.Split("^");

            // I'm bored and no longer care about granular feedback XD
            // Also, powers are far more complicated than simple exponents
            try
            {
                double baseVal = double.Parse(parts[0]);
                double power = double.Parse(parts[1]);

                return Math.Pow(baseVal, power);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Parses integer and decimal values.
        /// It decides weather or not to use long, double
        /// </summary>
        /// <remarks>
        /// Throws generic on fail
        /// </remarks>
        /// <param name="val">The value to parse</param>
        /// <returns>Either long or double</returns>
        private static object ParseNormalNumber((string, int) val)
        {
            // We assume it's decimal
            if (val.Item1.Contains('.'))
            {
                double output;

                if (!double.TryParse(val.Item1, out output)) { BadNumberFeedback(val.Item2); throw new Exception(); }

                return output;
            }
            // We assume it's integer
            else
            {
                long output;

                if (!long.TryParse(val.Item1, out output)) { BadNumberFeedback(val.Item2); throw new Exception(); }

                return output;
            }
        }

        public void PrintStrings(string[] strings, TimeSpan timeTaken)
        {
            StringBuilder sb = new StringBuilder();

            // If we need to print the time taken, do it
            if (Options.PrintTime && !Options.ForBash) sb.AppendLine($"Calculated in {timeTaken.TotalSeconds} seconds");
            if (Options.PrintTime && Options.ForBash) sb.AppendLine(timeTaken.TotalSeconds.ToString());

            if (Options.ForBash)
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    sb.Append(((i != 0) ? "," : "") + strings[i]);
                }

                Console.Write(sb.ToString());

                return;
            }

            for (int i = 0; i < strings.Length; i++)
            {
                // It's nicer to not have 2 for loops,
                // even if it is a few cycles slower
                if (Options.Number) sb.AppendLine("Number #" + (i + 1) + ": " + strings[i]);
                else sb.AppendLine("Password #" + (i + 1) + ": " + strings[i]);
            }

            // Not write line bc we already have a trailing \n
            Console.Write(sb.ToString());
        }

        protected static void BadUshortFeedback(int argNo)
        {
            Console.WriteLine($"The {LangNumber(argNo + 1)} argument was not a parsable unsigned integer number (min of 1 max of {MAX_CHARS})");
        }

        protected static void InvalidExponentFeedback(int argNo)
        {
            Console.WriteLine($"The exponent in the {LangNumber(argNo + 1)} argument was not valid. It must be an integer between -323 and 307 inclusive");
        }

        protected static void InvalidPowerFeedback(int argNo)
        {
            Console.WriteLine($"The power in the {LangNumber(argNo + 1)} argument was not valid. It must be in the format \"x^y\" where x or y can be integer or decimal");
        }

        protected static void InvalidBaseFeedback(int argNo)
        {
            Console.WriteLine($"The base value in the {LangNumber(argNo + 1)} argument was not valid. It must be a number lower than 10 and greater than -10");
        }

        protected static void BadNumberFeedback(int argNo)
        {
            Console.WriteLine($"The expected number as the {LangNumber(argNo + 1)} argument was not valid");
        }

        protected static void BadPairsFeedback()
        {
            Console.WriteLine("Random number generator mode requires either exactly one number or several pairs (groups of two) of numbers");
        }
    }
}
