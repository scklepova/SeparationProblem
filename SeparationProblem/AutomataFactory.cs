using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeparationProblem
{
    public static class AutomataFactory
    {
        public static Automata GetRandomPermutationAutomata(int n)
        {
            while(true)
            {
                var transitions = new[] {RandomFactory.GetRandomPermutation(n), RandomFactory.GetRandomPermutation(n)};
                var automata = new Automata(n, RandomFactory.GetNext(n), transitions);
                if(automata.IsConnected())
                    return automata;
            }
        }

        public static IEnumerable<Automata> GetAllPermutationAutomata(int n)
        {
            var permutations = AllPermutations(n);
            
            foreach (var permutation0 in permutations)
            {
                foreach (var permutation1 in permutations)
                {
                    for (var initState = 0; initState < n; initState++)
                    {
                        var automata = new Automata(n, initState, new[] { permutation0, permutation1 });
                        yield return automata;
                    }
                }
            }           
        }

        public static List<Automata> GetAllNonIsomorphicPermutationAutomatas5()
        {
            var filename = "permutationAutomata5.txt";
            var lines = File.ReadAllLines(filename);
            var automatas = new List<Automata>();
            foreach (var line in lines)
            {
                var perms = line.Split(' ');
                for(int initState = 0; initState <= 4; initState++)
                    automatas.Add(new Automata(5, initState, perms[0], perms[1]));
            }
            return automatas;
        }

        public static List<int[]> AllPermutations(int n)
        {
            var permutations = new List<int[]>();
            var permutation = Enumerable.Range(0, n).ToArray();
            while (permutation != null)
            {
                permutations.Add(permutation);
                permutation = NextPermutation(permutation);
            }

            return permutations;
        }

        private static int[] NextPermutation(IEnumerable<int> permutation)
        {
            var array = permutation.ToArray();
            // Find longest non-increasing suffix
            int i = array.Length - 1;
            while (i > 0 && array[i - 1] >= array[i])
                i--;
            // Now i is the head index of the suffix

            // Are we at the last permutation already?
            if (i <= 0)
                return null;

            // Let array[i - 1] be the pivot
            // Find rightmost element that exceeds the pivot
            int j = array.Length - 1;
            while (array[j] <= array[i - 1])
                j--;
            // Now the value array[j] will become the new pivot
            // Assertion: j >= i

            // Swap the pivot with j
            int temp = array[i - 1];
            array[i - 1] = array[j];
            array[j] = temp;

            // Reverse the suffix
            j = array.Length - 1;
            while (i < j)
            {
                temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }

            // Successfully computed the next permutation
            return array;
        }

        public static IEnumerable<Automata> GetAllAutomatas(int n)
        {
            var setsOfInt = GetAllSetsOfInt(n);
            return GetAllAutomatasWithKnownSets(n, setsOfInt);
        }

        public static IEnumerable<Automata> GetAllAutomatasWithKnownSets(int n, List<int[]> setsOfInt)
        {
            foreach (var set0 in setsOfInt)
            {
                foreach (var set1 in setsOfInt)
                {
                    for (var initState = 0; initState < n; initState++)
                        yield return new Automata(n, initState, new[] { set0, set1 });
                }
            }
        }

        public static IEnumerable<Automata> GetAllConnectedAutomatasWithKnownSets(int n, List<int[]> setsOfInt)
        {
            foreach (var set0 in setsOfInt)
            {
                foreach (var set1 in setsOfInt)
                {
                    for (var initState = 0; initState < n; initState++)
                    {
                        var automata = new Automata(n, initState, new[] {set0, set1});
                        if (automata.IsConnected())
                            yield return automata;
                    }
                }
            }
        }

        public static IEnumerable<Automata> GetAllNotPermutationAutomatas(int n)
        {
            var setsOfInt = GetAllSetsOfInt(n);
            var isPermutation = setsOfInt.ToDictionary(x => x, x => IsPermutation(x, n));
//            var list = new List<Automata>();
            foreach (var set0 in setsOfInt)
            {
                foreach (var set1 in setsOfInt)
                {
                    if(!(isPermutation[set0] && isPermutation[set1]))
                        for (var initState = 0; initState < n; initState++)
                            yield return new Automata(n, initState, new[] { set0, set1 });
                }
            }
//            return list;
        }

        private static bool IsPermutation(int[] set, int n)
        {
            var used = new bool[n];
            foreach (var i in set)
            {
                used[i] = true;
            }

            return used.All(b => b);
        }

        public static List<int[]> GetAllSetsOfInt(int n)
        {
            var array = new int[n];
            for (var i = 0; i < n; i++)
                array[i] = 0;

            var list = new List<int[]>();
            while(array != null)
            {
                list.Add(array);
                array = ArrayPlusOne(n, n - 1, array);         
            }
            return list;
        }

        private static int[] ArrayPlusOne(int n, int pos, int[] array)
        {
            var newArray = (int[])array.Clone();
            while (true)
            {
                if (pos < 0)
                    return null;
                if (array[pos] + 1 < n)
                {
                    newArray[pos]++;
                    return newArray;
                }

                newArray[pos] = 0;
                pos = pos - 1;
            }
        }
    }

}