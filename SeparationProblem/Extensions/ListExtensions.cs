using System;
using System.Collections.Generic;
using System.Linq;

namespace SeparationProblem.Extensions
{
    public static class ListExtensions
    {
        public static string PathToStr(this List<string> path)
        {
            var ans = path[0];
            for (var i = 1; i < path.Count; i++)
                ans += path[i].Last();

            return ans;
        }

        public static string PermutationToString(this List<int> permutation)
        {
            return permutation.Aggregate("", (current, i) => current + (i + " "));
        }

        public static bool HasSameEquality(this List<Tuple<string, string>> equalities, Tuple<string, string> equality)
        {
            foreach (var tuple in equalities)
            {
                if (tuple.Item1.IsEqualStringFor(equality.Item1) && tuple.Item2.IsEqualStringFor(equality.Item2) ||
                    tuple.Item1.IsEqualStringFor(equality.Item2) && tuple.Item2.IsEqualStringFor(equality.Item1))
                    return true;
            }
            return false;
        }

        public static int CountStartingFrom(this List<string> list, string toFind, int position)
        {
            var c = 0;
            for(var i = position; i < list.Count; i++)
                if (list[i] == toFind)
                    c++;
            return c;
        }
    }
}