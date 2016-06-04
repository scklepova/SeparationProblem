using System;
using System.Linq;

namespace SeparationProblem
{
    public static class RandomFactory
    {
        public static int[] GetRandomArray(int max)
        {
            var n = random.Next(max) + 1;
            var a = new int[n];
            for (var i = 0; i < n; i++)
                a[i] = random.Next(max);
            return a;
        }

        public static int[] GetRandomArray(int maxValue, int n)
        {
            var a = new int[n];
            for (var i = 0; i < n; i++)
                a[i] = random.Next(maxValue);
            return a;
        }

        public static int[] GetRandomPermutation(int n)
        {
            var a = Enumerable.Range(0, n).ToArray();
            for (var i = n - 1; i >= 0; i--)
            {
                var j = random.Next(0, i);
                Swap(ref a[i], ref a[j]);
            }
            return a;
        }

        public static int GetNext(int maxValue) // выдает значение меньше макс вэл
        {
            return random.Next(maxValue);
        }

        public static string GetRandomString(int length)
        {
            var s = "";
            for (var i = 0; i < length; i++)
            {
                s += random.Next(2);
            }

            return s;
        }

        private static void Swap(ref int a, ref int b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        private static readonly Random random = new Random();
    }
}