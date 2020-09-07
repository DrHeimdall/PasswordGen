using System;
using System.Threading;
using System.Threading.Tasks;


namespace rnd
{
    public static class CalculateString
    {
        public static string MakeString(Random rnd)
        {
            int threads = Environment.ProcessorCount;
            string str = "";

            Task[] tasks = new Task[threads];

            for (int i = 0; i < threads; i++)
            {
                Task.Factory.StartNew(() => {});
            }
        }

        protected static Transaction(int )
        {

        }
    }
}