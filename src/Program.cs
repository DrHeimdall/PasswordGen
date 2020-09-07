using System;
using System.Collections.Generic;

using static rnd.NumberPhonet;


namespace rnd
{
    public class Program
    {
        private static string Alphabet { get => "qwertyuiopasdfghjklzxcvbnm" + "qwertyuiopasdfghjklzxcvbnm".ToUpper(); }
        private static string SafeChars { get => Alphabet + "0123456789"; }
        private static string ComplexChars { get => SafeChars + "!\"£$%^&*()-_=+[]{}#~`¬|\\<>,.?/™"; }

        static int Main(string[] args)
        {
            // For testing
            // args = new string[2]{"-t", "15000"};

            bool human = false;
            bool forBash = false;
            bool printTime = false;

            List<int> lengths = new List<int>();

            if (s_ProcessArgs(args, ref human, ref forBash, ref printTime, ref lengths) != 0) return 1;

            DateTime start = DateTime.UtcNow;

            char[] chars = (human) ? SafeChars.ToCharArray() : ComplexChars.ToCharArray();
            string[] strings = new string[lengths.Count];

            for (int i = 0; i < lengths.Count; i++)
            {
                strings[i] = CalculateString.s_MakeString(lengths[i], chars);
            }

            TimeSpan duration = (DateTime.UtcNow - start);

            if (printTime && !forBash) Console.WriteLine($"Calculated in {duration.TotalSeconds} seconds");
            if (printTime && forBash) Console.WriteLine(duration.TotalSeconds);

            s_PrintStrings(strings, forBash);

            return 0;
        }

        static void s_PrintStrings(string[] strings, bool forBash)
        {
            if (forBash)
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    Console.Write(((i != 0) ? "," : "") + strings[i]);
                }

                Console.Write("\n");

                return;
            }

            for (int i = 0; i < strings.Length; i++)
            {
                Console.WriteLine("Password #" + (i + 1) + ": " + strings[i]);
            }
        }

        static int s_ProcessArgs(string[] args, ref bool human, ref bool forBash, ref bool printTime, ref List<int> lengths)
        {
            if (args.Length == 0)
            {
                s_PrintHelp();
                return 1;
            }


            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower().Contains('h') && args[i].StartsWith('-')) { human = true; }
                else if (args[i].ToLower().Contains('c') && args[i].StartsWith('-')) { forBash = true; }
                else if (args[i].ToLower().Contains('t') && args[i].StartsWith('-')) { printTime = true; }
                else
                {
                    try
                    {
                        int len = int.Parse(args[i]);
                        lengths.Add(len);
                    }
                    catch
                    {
                        return s_NoParseError(i);
                    }
                }
            }

            if (forBash) { human = true; }

            return 0;
        }

        static void s_PrintHelp()
        {
            Console.WriteLine($"rnd [-h] [-t] [-c] len\nPrints a random sequence of characters of length len\nUses \"{SafeChars}\" with -h and \"{ComplexChars}\" without\n  -t prints the time taken for just the string to generate in seconds\n  -c prints just the string of characters and nothing else, usefull if using in a bash script\nThe program will print a random string for every length measurement given (e.g. rnd 156 65 23 will print 3 strings of respective length)");
        }

        static int s_NoParseError(int argNo)
        {
            Console.WriteLine($"The {LangNumber(argNo + 1)} argument was not a parsable interger number");

            return 1;
        }

        static int s_BadNumberError(int argNo)
        {
            Console.WriteLine($"The {LangNumber(argNo + 1)} argument was an invalid number.\nThe length of the string must be above 1");

            return 1;
        }
    }
}
