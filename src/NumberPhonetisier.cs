namespace rnd
{
    public static class NumberPhonet
    {
        /// <summary>
        /// Returns a number formatted like 13th, 3rd, -9582nd etc.
        /// </summary>
        /// <example>
        /// LangNumber(50); returns "50th"
        /// LangNumber(-52) returns "-52nd"
        /// </example>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string LangNumber(int number)
        {
            int no = (number < 0) ? number * -1 : number;

            if (no > 3 && no <= 20) return $"{no}th";

            int lastNumber = no % 10;

            if (lastNumber == 1) return $"{no}st";

            if (lastNumber == 2) return $"{no}nd";

            if (lastNumber == 3) return $"{no}rd";

            return $"{no}th";
        }
    }
}