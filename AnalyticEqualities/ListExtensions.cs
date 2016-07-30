using System.Collections.Generic;
using System.Linq;

namespace AnalyticEqualities
{
    public static class ListExtensions
    {
        public static string ToStr(this IEnumerable<long> list)
        {
            return list.Aggregate("", (current, i) => current + (i + " "));
        }

        public static bool ContainsSet(this List<SortedSet<long>> list, SortedSet<long> set)
        {
            return list.Any(item => item.SetEquals(set));
        }
    }
}