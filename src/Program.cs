using System;
using System.Linq;
using System.Collections.Generic;

using static rnd.NumberPhonet;


namespace rnd
{
    public class Program
    {
        static void Main(string[] args)
        {
            // For testing
            args = new string[]{"-n", "2.5^8", "-h"};

            if (args.Length == 0 || args.Contains("-h") || args.Contains("help")) { PrintHelp(); return; }

            Generator gen;

            try
            {
                // Sort the arguments
                Queue<(string, int)> qArgs = GenOptions.ArgsToQueue(args);
                // Parse the arguments
                GenOptions opts = new GenOptions(qArgs);
                // Parse the length(s)
                gen = new Generator(opts, qArgs);
            }
            catch
            {
                Console.WriteLine("Program Failed");
                Console.WriteLine("Run without args or with -h to get help");
                
                return;
            }



            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            (TimeSpan, bool) timeTaken;

            gen.PrintStrings(gen.GenerateAuto(out timeTaken));
        }

        static void PrintHelp()
        {
            Console.WriteLine($"rnd [-h] [-t] [-c] [-n] len\nPrints a random sequence of characters of length len\nUses \"{Generator.SafeChars}\" with -h and \"{Generator.ComplexChars}\" without\n  -t prints the time taken for just the string to generate in seconds\n  -c prints just the string of characters and nothing else, usefull if using in a bash script\nThe program will print a random string for every length measurement given (e.g. rnd 156 65 23 will print 3 strings of respective length)\n  -n Puts the program in number mode. Len can be a single value or any number of multiples of two (i.e. 2 or 2 4 4 8) with each representing a min and max for a random generator. Numbers can be integer (processed as 64 bit int) or they can be powers (e.g. 2^8 3^0.3333) (processed as 64 bit fp) or exponents (e.g. 5E8 2.64E80) (processed as 64 bit fp)");
        }
    }
}
