using System;
using static rnd.NumberPhonet;


namespace rnd
{
    public class Program
    {
        private static string SafeChars = "qwertyuiopasdfghjklzxcvbnm0123456789";
        private static string ComplexChars { get => SafeChars + "!\"£$%^&*()-_=+[]{}#~`¬|\\<>,.?/"; }

        static int Main(string[] args)
        {
            Random rnd = new Random();
            bool simple = false;
            int len = -1;

            if (args.Length == 0) PrintHelp();


            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower().Contains('s'))
                {
                    simple = true;
                }
                else
                {
                    try
                    {

                    }
                    catch
                    {
                        NoParseError(i);
                    }
                }
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine($"rnd [-s] len\nPrints a random sequence of characters of length len.\nUses \"{SafeChars}\" with -s and \"{ComplexChars}\" without");
        }

        static int NoParseError(int argNo)
        {
            Console.WriteLine($"The {LangNumber(argNo + 1)} argument was not a parsable interger number");

            return 1;
        }

        static int BadNumberError(int argNo)
        {
            Console.WriteLine($"The {LangNumber(argNo + 1)} argument was an invalid number.\nThe length of the string must be above 1");

            return 1;
        }
    }
}
