using System;

namespace SeparationProblem.Extensions
{
    public static class TupleExtensions
    {
        public static string ToStr(this Tuple<string, string> pair)
        {
            return string.Format("{0} {1}", pair.Item1, pair.Item2);
        }
    }
}