using System.Collections.Generic;
using System.Linq;
using SeparationProblem.Extensions;

namespace SeparationProblem
{
    public static class WordsFactory
    {
        public static List<string> GetAllWordsOfLength(int n)
        {
            var arr = new int[n];
            for (var i = 0; i < n; i++)
                arr[i] = 0;
            var ans = new List<int[]>();
            while (arr.Length == n)
            {
                ans.Add(arr);
                arr = arr.PlusOneIn2ss(n - 1);
            }

            return ans.Select(x => x.GetString()).ToList();
        } 
    }
}