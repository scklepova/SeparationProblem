using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnalyticEqualities
{
    class Program
    {
        static void Main(string[] args)
        {
//            FindPermutationEqualityDegees(); 
//            BruteForceFor4();
            BruteForceFor6();
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
//                requiredDivisors = RemoveEqualDivisors(requiredDivisors);
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
            return divisors.Distinct().Where(x => x != 1).OrderBy(x => x).ToArray();
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

                if (mask == 18711)
                {
                    var t = 0;
                }

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

                

                if (a + b + c + d < min && IsIdentity_dcba(a, b, c, d, divisors))
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

        private static bool IsIdentity_dcba(long a, long b, long c, long d, long[] divisors)
        {
            return divisors.All(divisor => IsIdentity_dcba(a, b, c, d, divisor));
        }

        private static bool IsIdentity_dcba(long a, long b, long c, long d, long divisor)
        {
            if (b%divisor == 0 && IsEquality(a + c, d, divisor) ||
                c%divisor == 0 && IsEquality(a, b + d, divisor) ||
                a%divisor == 0 && IsEquality(b, c, d, divisor) ||
                d%divisor == 0 && IsEquality(a, b, c, divisor))
                return true;
            return false;
        }

        private static bool IsIdentity_dbca(long a, long b, long c, long d, long[] divisors)
        {
            foreach (var divisor in divisors)
            {
                if(a % divisor == 0 && IsEquality(c, d, divisor)||
                   b % divisor == 0 && IsEquality(a+c, d, divisor) ||
                   c % divisor == 0 && IsEquality(a, b + d, divisor) ||
                   d % divisor == 0 && IsEquality(a, b, divisor))
                    continue;

                return false;
            }
            return true;
        }

        private static bool IsIdentity_dabc(long a, long b, long c, long d, long[] divisors)
        {
            foreach (var divisor in divisors)
            {
                if( a % divisor == 0 && IsEquality(c, d, divisor) ||
                    b % divisor == 0 && IsEquality(a+c, d, divisor) ||
                    c % divisor == 0 && IsEquality(a, d, divisor) ||
                    d % divisor == 0)
                    continue;
                return false;
            }
            return true;
        }

        private static bool IsIdentity_dbac(long a, long b, long c, long d, long[] divisors)
        {
            foreach (var divisor in divisors)
            {
                if (a % divisor == 0 && IsEquality(c, d, divisor) ||
                    b % divisor == 0 && IsEquality(a + c, d, divisor) ||
                    c % divisor == 0 && IsEquality(a, b + d, divisor) ||
                    d % divisor == 0 && IsEquality(a, b, divisor))
                    continue;
                return false;
            }
            return true;
        }

        private static bool IsIdentity_bcda(long a, long b, long c, long d, long[] divisors)
        {
            foreach (var divisor in divisors)
            {
                if (a % divisor == 0 ||
                    b % divisor == 0 && IsEquality(a, d, divisor) ||
                    c % divisor == 0 && IsEquality(a, b + d, divisor) ||
                    d % divisor == 0 && IsEquality(a, b, divisor))
                    continue;
                return false;
            }
            return true;
        }

        private static bool IsIdentity_bdca(long a, long b, long c, long d, long[] divisors)
        {
            foreach (var divisor in divisors)
            {
                if (a % divisor == 0 && IsEquality(c, d, divisor) ||
                    b % divisor == 0 && IsEquality(a+c, d, divisor) ||
                    c % divisor == 0 && IsEquality(a, b + d, divisor) ||
                    d % divisor == 0 && IsEquality(a, b, divisor))
                    continue;
                return false;
            }
            return true;
        }

        private static bool IsIdentity_badc(long a, long b, long c, long d, long[] divisors)
        {
            foreach (var divisor in divisors)
            {
                if (a % divisor == 0 && IsEquality(c, d, divisor) ||
                    b % divisor == 0 && IsEquality(c, d, divisor) ||
                    c % divisor == 0 && IsEquality(a, b, divisor) ||
                    d % divisor == 0 && IsEquality(a, b, divisor))
                    continue;
                return false;
            }
            return true;
        }

        private static bool IsEquality(long a, long b, long c, long divisor)
        {
            if (b % divisor == 0 || a%divisor == c%divisor) 
                return true;
                       
            return false;
        }

        public static bool IsIdentity_fedcba(long a, long b, long c, long d, long e, long f, long[] divisors)
        {
            foreach (var divisor in divisors)
            {
                if (a % divisor == 0 && (b > f && IsIdentity_dcba(b-f, c, d, e, divisor) || b == f && IsEquality(c, d, e, divisor) || b < f && IsIdentity_dcba(c, d, e, f - b, divisor)) ||
                    b % divisor == 0 && IsIdentity_dcba(a+c, d, e, f, divisor) ||
                    c % divisor == 0 && IsIdentity_dcba(a, b+d, e, f, divisor) ||
                    d % divisor == 0 && IsIdentity_dcba(a, b, c+e, f, divisor) ||
                    e % divisor == 0 && IsIdentity_dcba(a, b, c, d+f, divisor) ||
                    f % divisor == 0 && (a > e && IsIdentity_dcba(a - e, b, c, d, divisor) || a == e && IsEquality(b, c, d, divisor) || a < e && IsIdentity_dcba(b, c, d, e-a, divisor)))
                    continue;
                return false;
            }
            return true;
        }

        public static void BruteForceFor4()
        {
            var file = new StreamWriter(File.OpenWrite("minimumDegrees4_dcba_1.txt")) { AutoFlush = true };
            cyclesLengths[0] = new List<SortedSet<long>>();
            cyclesLengths[1] = new List<SortedSet<long>> { new SortedSet<long> { 1 } };
            var permutationLength = 2;
            SetCyclesLengths(permutationLength);
            var requiredDivisors = GetDivisors(cyclesLengths[permutationLength]);
            var reducedDivisors = ReduceDivisors(requiredDivisors).Reverse().ToArray();
            int d;
            //todo если организовать перебор по возрастанию суммарной длины, то для кадого следующего k можно будет начинать перебор с длины известной для k-1
            var length = 4;
            while (length < 3000)
            {
                var foundIdentityOfSuchLength = false;
//                Console.WriteLine(length);
                for (var a = 1; a < length - 3; a++)
                {
                    if (foundIdentityOfSuchLength)
                        break;
                    for (var b = 1; b < length - a - 2; b++)
                    {
                        if (foundIdentityOfSuchLength)
                            break;
                        for (var c = 1; c < length - a - b - 1; c++)
                        {
                            d = length - a - b - c;
                            if (IsIdentity_dcba(a, b, c, d, reducedDivisors))
                            {
                                file.WriteLine("k={4} : a={0}, b={1}, c={2}, d={3}, length={5}", a, b, c, d, permutationLength, length*2);
                                Console.WriteLine("k={4} : a={0}, b={1}, c={2}, d={3}, length = {5}", a, b, c, d, permutationLength, length*2);
                                permutationLength++;
                                SetCyclesLengths(permutationLength);
                                requiredDivisors = GetDivisors(cyclesLengths[permutationLength]);
                                reducedDivisors = ReduceDivisors(requiredDivisors).Reverse().ToArray();
                                foundIdentityOfSuchLength = true;
                                break;
                            }
                        }
                    }
                }
                if(!foundIdentityOfSuchLength)
                    length++;
            }
            

            file.Close();
            Console.WriteLine("End");
            Console.ReadKey();
        }

        public static void BruteForceFor6()
        {
            var file = new StreamWriter(File.OpenWrite("minimumDegrees6_fedcba.txt")) { AutoFlush = true };
            cyclesLengths[0] = new List<SortedSet<long>>();
            cyclesLengths[1] = new List<SortedSet<long>> { new SortedSet<long> { 1 } };
            var permutationLength = 2;
            SetCyclesLengths(permutationLength);
            var requiredDivisors = GetDivisors(cyclesLengths[permutationLength]);
            var reducedDivisors = ReduceDivisors(requiredDivisors).Reverse().ToArray();
            int f;
            //todo если организовать перебор по возрастанию суммарной длины, то для кадого следующего k можно будет начинать перебор с длины известной для k-1
            var length = 404;
            while (length < 3000)
            {
                var foundIdentityOfSuchLength = false;
                //                Console.WriteLine(length);
                for (var a = 1; a < length - 5; a++)
                {
                    if (foundIdentityOfSuchLength)
                        break;
                    for (var b = 1; b < length - a - 4; b++)
                    {
                        if (foundIdentityOfSuchLength)
                            break;
                        for (var c = 1; c < length - a - b - 3; c++)
                        {
                            if (foundIdentityOfSuchLength)
                                break;
                            for (var d = 1; d < length - a - b - c - 2; d++)
                            {
                                if (foundIdentityOfSuchLength)
                                    break;
                                for (var e = 1; e < length - a - b - c - d - 1; e++)
                                {
                                    f = length - a - b - c - d - e;
                                    if (IsIdentity_fedcba(a, b, c, d, e, f, reducedDivisors))
                                    {
                                        file.WriteLine("k={6} : a={0}, b={1}, c={2}, d={3}, e={4}, f={5} length={7}", a, b, c, d, e, f, permutationLength, length * 2);
                                        Console.WriteLine("k={6} : a={0}, b={1}, c={2}, d={3}, e={4}, f={5} length={7}", a, b, c, d, e, f, permutationLength, length * 2);
                                        permutationLength++;
                                        SetCyclesLengths(permutationLength);
                                        requiredDivisors = GetDivisors(cyclesLengths[permutationLength]);
                                        reducedDivisors = ReduceDivisors(requiredDivisors).Reverse().ToArray();
                                        foundIdentityOfSuchLength = true;
                                        break;
                                    }
                                }
                            }
                            
                        }
                    }
                }
                if (!foundIdentityOfSuchLength)
                    length++;
            }


            file.Close();
            Console.WriteLine("End");
            Console.ReadKey();
        }

        private static bool IsEquality(long a, long b, long divisor)
        {
            return a%divisor == 0 || b%divisor == 0;
        }

        private static List<SortedSet<long>>[] cyclesLengths = new List<SortedSet<long>>[40];
    }
}
