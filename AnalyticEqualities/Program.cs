using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticEqualities
{
    class Program
    {
        static void Main(string[] args)
        {
            FindPermutationEqualityDegees();          
        }

        public static void FindPermutationEqualityDegees()
        /* Ищем перестановочные тождества среди слов вида (xy)^a(yx)^b = (yx)^a(xy)^b
         * Строки такого вида будут тождеством, если для любого порядка перестановки этот порядок делит a или b
         * Порядок перестановки - минимальное количество раз, которое нужно применить перестановку, чтобы элементы вернулись на место.
         * Например, если перестановка над 7 элементами содержит 2 цикла длин 2 и 5, то порядок перестановки будет 10
         */
        {
            var file = new StreamWriter(File.OpenWrite("minimumDegrees4_1.txt")) {AutoFlush = true};
            cyclesLengths[0] = new List<SortedSet<long>>();
            cyclesLengths[1] = new List<SortedSet<long>> { new SortedSet<long> { 1 } };
            for (var permutationLength = 2; permutationLength < 8; permutationLength++)
            {
                SetCyclesLengths(permutationLength);
                var requiredDivisors = GetDivisors(cyclesLengths[permutationLength]);
                requiredDivisors = RemoveEqualDivisors(requiredDivisors);
//                requiredDivisors = ReduceDivisors(requiredDivisors);
//                file.WriteLine(string.Format("PermLength {0} : {1}", permutationLength, requiredDivisors.ToStr()));
                file.WriteLine(permutationLength + " : " + SplitIntoFourGroups(requiredDivisors));


            }
            file.Close();
            Console.WriteLine("End");
            Console.ReadKey();
        }

        private static long[] RemoveEqualDivisors(long[] divisors)
        {
            return divisors.Distinct().ToArray();
        } 

        private static long[] ReduceDivisors(long[] divisors)
        {
            divisors = divisors.OrderBy(x => x).ToArray();
            var reduced = new List<long>();
            for (var i = 0; i < divisors.Length; i++)
            {
                var divideGreaterOne = false;
                for (var j = i + 1; j < divisors.Length; j++)
                    if (divisors[j]%divisors[i] == 0)
                    {
                        divideGreaterOne = true;
                        break;
                    }
                if(!divideGreaterOne)
                    reduced.Add(divisors[i]);
            }
            return reduced.ToArray();
        }

        private static long[] GetDivisors(IEnumerable<SortedSet<long>> cyclesLength)
        {
            return cyclesLength.Select(MathHelper.LeastCommonMultiple).ToArray();
        }

        private static void SetCyclesLengths(int permutationLength)
        {
            cyclesLengths[permutationLength] = new List<SortedSet<long>> { new SortedSet<long> { permutationLength } };
            var combinations = new List<SortedSet<long>>();
            for (var i = 1; i <= permutationLength/2; i++)
            {
                combinations.AddRange(GetAllCombinations(cyclesLengths[i], cyclesLengths[permutationLength - i]));
            }

            foreach (var combination in combinations)
            {
                if(!cyclesLengths[permutationLength].ContainsSet(combination))
                    cyclesLengths[permutationLength].Add(combination);
            }
        }

        private static IEnumerable<SortedSet<long>> GetAllCombinations(List<SortedSet<long>> list1, List<SortedSet<long>> list2)
        {
            var result = new List<SortedSet<long>>();
            foreach (var comb1 in list1)
            {
                foreach (var comb2 in list2)
                {
                    var comb = new SortedSet<long>(comb1);
                    foreach (var i in comb2)
                    {
                        comb.Add(i);
                    }
                    result.Add(comb);
                }
            }
            return result;
        }

        public static string SplitIntoTwoGroups(long[] divisors)
        {
            long mask = 0;
            var border = Math.Pow(2, divisors.Length - 1);
            long minmask = 0;
            var min = long.MaxValue;
            long mina = 0;
            long minb = 0;
            while (mask < border)
            {
                long a = 1;
                long b = 1;

                var temp = mask;
                var i = 0;
                while (i < divisors.Length)
                {
                    var c = temp%2;
                    if (c == 0)
                        a = MathHelper.LeastCommonMultiple(a, divisors[i]);
                    else
                        b = MathHelper.LeastCommonMultiple(b, divisors[i]);
                    i++;
                    temp = temp >> 1;
                }

                if (a + b < min)
                {
                    min = a + b;
                    minmask = mask;
                    mina = a;
                    minb = b;
                }

                mask++;
            }

            return string.Format("a = {0}, b = {1}, minmask = {2}", mina, minb, minmask);
//            Console.WriteLine();
//            Console.ReadKey();
        }

        public static string SplitIntoFourGroups(long[] divisors)
        {
            long mask = 0;
            var border = Math.Pow(4, divisors.Length);
            long minmask = 0;
            var min = long.MaxValue;
            long mina = 0;
            long minb = 0;
            long minc = 0;
            long mind = 0;
            while (mask < border)
            {
                long a = 1;
                long b = 1;
                long c = 1;
                long d = 1;

                var temp = mask;
                var i = 0;
                while (i < divisors.Length)
                {
                    var rem = temp % 4;
                    if (rem == 0)
                        a = MathHelper.LeastCommonMultiple(a, divisors[i]);
                    else if (rem == 1)
                        b = MathHelper.LeastCommonMultiple(b, divisors[i]);
                    else if (rem == 2)
                        c = MathHelper.LeastCommonMultiple(c, divisors[i]);
                    else
                        d = MathHelper.LeastCommonMultiple(d, divisors[i]); 
                    i++;
                    temp = temp >> 2;
                }

                if (a == 10 && b == 12 && c == 14 && d == 2)
                {
                    var t = 0;
                }

                if (a + b + c + d < min && IsEquality(a, b, c, d, divisors))
                {
                    min = a + b + c + d;
                    minmask = mask;
                    mina = a;
                    minb = b;
                    minc = c;
                    mind = d;
                }

                mask++;
            }

            return string.Format("a = {0}, b = {1}, c = {2}, d = {3}", mina, minb, minc, mind);
            //            Console.WriteLine();
            //            Console.ReadKey();
        }

        private static bool IsEquality(long a, long b, long c, long d, long[] divisors)
        {
            foreach (var divisor in divisors)
            {
                if (b % divisor == 0)
                {
                    if (!IsEquality(a + c, d, divisor)) return false;
                }
                else if (c % divisor == 0)
                {
                    if (!IsEquality(a, b + d, divisor)) return false;
                }
                else if (a % divisor == 0)
                {
                    if (!IsEquality(b, c, d, divisor)) return false;
                }               
                else if (d % divisor == 0)
                {
                    if (!IsEquality(a, b, c, divisor)) return false;
                }
                else return false;
            }
            return true;
        }

        private static bool IsEquality(long a, long b, long c, long divisor)
        {            
            if (a%divisor == 0)
            {
                if (!IsEquality(b, c, divisor)) return false;
            }
            else if (b%divisor == 0)
            {
//                if (!IsEquality(a, c, divisor)) return false;
            }
            else if (c%divisor == 0)
            {
                if (!IsEquality(a, b, divisor)) return false;
            }
            else
            {
                if (a%divisor != c%divisor) return false;
            }           
            return true;
        }

        private static bool IsEquality(long a, long b, long divisor)
        {
            return a%divisor == 0 || b%divisor == 0;
        }

        private static List<SortedSet<long>>[] cyclesLengths = new List<SortedSet<long>>[40];
    }
}
