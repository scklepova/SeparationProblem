using System.Collections.Generic;
using System.Linq;

namespace AnalyticEqualities
{
    public static class MathHelper
    {
        public static long LeastCommonMultiple(SortedSet<long> ints)
        {
            const long long1 = 1;
            return ints.Aggregate(long1, LeastCommonMultiple);
        }

        public static long LeastCommonMultiple(long a, long b)
        {
            return a*b/GreatestCommonDenominator(a, b);
        }

        private static long GreatestCommonDenominator(long a, long b)
        {
            while (a%b > 0)
            {
                var c = b;
                b = a%b;
                a = c;
            }
            return b;
        }
    }
}