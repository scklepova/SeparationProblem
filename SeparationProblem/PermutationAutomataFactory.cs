using System;
using System.Linq;

namespace SeparationProblem
{
    public static class PermutationAutomataFactory
    {
        public static Automata GetRandomPermutationAutomata(int n)
        {
            var transitions = new[] {GetRandomPermutation(n), GetRandomPermutation(n)};
            var random = new Random();
            return new Automata(n, random.Next(n - 1), transitions, GetRandomArray(n));
        }

        private static int[] GetRandomPermutation(int n)
        {
            var a = Enumerable.Range(0, n).ToArray();
            var random = new Random();
            for (var i = n - 1; i >= 0; i--)
            {
                var j = random.Next(0, i);
                Swap(ref a[i], ref a[j]);
            }
            return a;
        }

        private static void Swap(ref int a, ref int b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        private static int[] GetRandomArray(int max)
        {
            var random = new Random();
            var n = random.Next(max) + 1;
            var a = new int[n];
            for (var i = 0; i < n; i++)
                a[i] = random.Next(max);
            return a;
        }
    }
}