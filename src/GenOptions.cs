using System.Collections.Generic;
using System.Text.RegularExpressions;

using static rnd.NumberPhonet;


namespace rnd
{
    class GenOptions
    {
        private static Regex argRegex = new Regex(@"-\D");
        
        public bool Human;
        public bool Number;
        public bool Newline;
        public bool ForBash;
        public bool PrintTime;

        /// <summary>
        /// Gets the options from the args queue and stores them
        /// </summary>
        /// <param name="args">The args, sorted to have the '-' ones first</param>
        public GenOptions(Queue<(string, int)> args)
        {
            ProcessArgs(args);
        }

        /// <summary>
        /// Converts the args arr to queue.
        /// Gets the options from the args queue and stores them
        /// </summary>
        public GenOptions(string[] opts) : this(ArgsToQueue(opts)) { }

        public static Queue<(string, int)> ArgsToQueue(string[] args)
        {
            // Convert args to a list of tuples containing their
            // original indexes and the arg
            List<(string arg, int index)> sortedArgs = new List<(string, int)>();
            for (int i = 0; i < args.Length; i++)
            {
                sortedArgs.Add((args[i], i));
            }

            // Sort the arguments with - first
            sortedArgs.Sort((a, b) =>
            {
                int aVal = (argRegex.IsMatch(a.arg) ? -a.index : a.index);
                int bVal = (argRegex.IsMatch(b.arg) ? -b.index : b.index);

                return aVal - bVal;
            });

            // Then convert to a FIFO queue
            return new Queue<(string, int)>(sortedArgs);
        }

        private void ProcessArgs(Queue<(string arg, int)> args)
        {
            // Iterate over all of the arguments until we get to
            // the first non '-' argument
            while (args.Count != 0)
            {
                // If we're at the end of the '-' arguments, skip to the next loop
                if (!argRegex.IsMatch(args.Peek().arg)) break;

                // Dequeue the arg we're dealing with
                (string arg, int) arg = args.Dequeue();

                // Process it
                switch (arg.arg.ToLower()[1])
                {
                    case 'h':
                        this.Human = true; break;
                    case 'n':
                        this.Number = true; break;
                    case 'l':
                        this.Newline = true; break;
                    case 'c':
                        this.ForBash = true; goto case 'h'; // Make the forBash always flag human
                    case 't':
                        this.PrintTime = true; break;
                    default:
                        UnrecognisedArgWarning(arg); break;
                }
            }
        }

        protected static void UnrecognisedArgWarning((string arg, int index) arg)
        {
            System.Console.WriteLine($"WARN: The {LangNumber(arg.index + 1)} arg, '{arg.arg}' wasn't recognised.");
        }
    }
}
