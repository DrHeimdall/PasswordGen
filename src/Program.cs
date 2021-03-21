using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;


namespace rnd
{
    public class Program
    {
        static void Main(string[] args)
        {
            // For testing
            // args = new string[]{"-h", "30"};

            if (args.Length == 0 || args.Contains("help")) { PrintHelp(); return; }

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
                Console.WriteLine("Run without args or with arg \"help\" to get help");

                return;
            }

            // Get the time taken to compute the "password(s)" or random numbers
            TimeSpan timeTaken;

            gen.PrintStrings(gen.GenerateAuto(out timeTaken), timeTaken);
        }

        static void PrintHelp()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("rnd [help] [-h] [-t] [-c] [-n] <<len...> or <max> or <min max...>>\n");

            sb.AppendLine("Prints random sequence(s) of characters of length len");
            sb.AppendLine("Or if in number mode prints a random number between");
            sb.AppendLine("min and max inclusive. If a single number is given, min is 0\n");
            sb.AppendLine("Numbers can be integer or decimal, or in the format of");
            sb.AppendLine("  - x^y x and y can be decimal or integer. Returns a decimal");
            sb.AppendLine("  - xEy x can be either but y is integer. Returns a decimal");
            sb.AppendLine("  - If either min or max is decimal the result will be too\n");
            sb.AppendLine("Arguments:");
            sb.AppendLine("  -h Prints human characters (easily typed ones)");
            sb.AppendLine($"    -h uses:");
            sb.AppendLine($"    {Generator.SafeChars}");
            sb.AppendLine($"    default uses:");
            sb.AppendLine($"    {Generator.ComplexChars}\n");
            sb.AppendLine("  -t Prints the amount of time taken to fulfil the request");
            sb.AppendLine("  -c Prints in bash mode with a user-unfriendly output");
            sb.AppendLine("  -n Puts the generator in number mode");
            sb.AppendLine("    -h is ignored in number mode, obviously");

            Console.Write(sb.ToString());
        }
    }
}
