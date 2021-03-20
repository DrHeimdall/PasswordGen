using System;
using System.Collections.Generic;

using static rnd.NumberPhonet;


namespace rnd
{
    public class Program
    {
        static void Main(string[] args)
        {
            // For testing
            args = new string[]{"-n", "2.5^8"};

            if (args.Length == 0) {PrintHelp(); return;}

            Queue<(string, int)> qArgs = GenOptions.ArgsToQueue(args);

            GenOptions opts = new GenOptions(qArgs);

            Generator gen = new Generator(opts, qArgs);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            (TimeSpan, bool) timeTaken;

            PrintStrings(gen.GenerateAuto(out timeTaken), true);
        }

        static void PrintStrings(string[] strings, bool forBash)
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

        static void PrintHelp()
        {
            Console.WriteLine($"rnd [-h] [-t] [-c] len\nPrints a random sequence of characters of length len\nUses \"{Generator.SafeChars}\" with -h and \"{Generator.ComplexChars}\" without\n  -t prints the time taken for just the string to generate in seconds\n  -c prints just the string of characters and nothing else, usefull if using in a bash script\nThe program will print a random string for every length measurement given (e.g. rnd 156 65 23 will print 3 strings of respective length)");
        }
    }
}
