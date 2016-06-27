using System;

namespace SeparationProblem.Extensions
{
    public static class TupleExtensions
    {
        public static string ToStr(this Tuple<string, string> pair)
        {
            return string.Format("{0} {1}", pair.Item1, pair.Item2);
        }

        public static Tuple<string, string> AddPrefixAndSuffix(this Tuple<string, string> pair, string prefix, string suffix)
        {
            return new Tuple<string, string>(prefix + pair.Item1 + suffix, prefix + pair.Item2 + suffix);
        }
    }
}