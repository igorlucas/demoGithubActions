using System;

namespace TestProject
{
    public class PrimeService
    {
        public bool IsPrime(int candidate)
        {
            if (candidate <= 1) return false;
            if (candidate == 2) return true;
            if (candidate % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(candidate));

            for (int i = 3; i <= boundary; i += 2)
                if (candidate % i == 0)
                    return false;

            return true;
        }
    }
}