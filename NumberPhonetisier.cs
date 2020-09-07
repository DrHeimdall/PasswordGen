namespace rnd
{
    public static class NumberPhonet
    {
        public static string LangNumber(int number)
        {
            if (number < 0) number *= -1;

            if (number > 3 && number <= 20) return $"{number}th";

            int lastNumber = number % 10;

            if (lastNumber == 1) return $"{number}st";

            if (lastNumber == 2) return $"{number}nd";

            if (lastNumber == 3) return $"{number}rd";

            return $"{number}th";
        }
    }
}