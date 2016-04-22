using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeparationProblem
{
    class Program
    {
        static void Main(string[] args)
        {
//            JoinBackups();
            RandomPermutationAutomatas();
//            AllPermutationAutomatas();
        }

        public static void JoinBackups()
        {
            var hardlySeparated = new List<Tuple<string, string>>();
            for(var i = 1; i <= 6; i++)
            {
                var pairs = File.ReadAllLines(string.Format("separation_backup{0}.txt", i)).Select(x => new Tuple<string, string>(x.Split(' ')[0], x.Split(' ')[1])).ToList();
                foreach(var pair in pairs)
                {
                    if(!hardlySeparated.Contains(pair))
                        hardlySeparated.Add(pair);
                }
            }

            File.WriteAllLines("joined_backup.txt", hardlySeparated.Select(x => string.Format("{0} {1}", x.Item1, x.Item2)));
        }

        public static void AllPermutationAutomatas()
        {
            var permutations = AllPermutations();
            var hardlySeparatedPairs = File.ReadAllLines("separation.txt").Select(x => new Tuple<string, string>(x.Split(' ')[0], x.Split(' ')[1])).ToList();
            var separated = new Dictionary<Tuple<string,string>, bool>();

            foreach(var permutation0 in permutations)
            {
                foreach(var permutation1 in permutations)
                {
                    for(var initState = 0; initState <= 4; initState++)
                    {
                        var automata = new Automata(5, initState, new []{permutation0, permutation1});
                        foreach(var pair in hardlySeparatedPairs.Where(x => !separated.ContainsKey(x) || !separated[x]))
                        {
                            if(automata.Separates(pair))
                            {
                                if(!separated.ContainsKey(pair))
                                    separated.Add(pair, true);
                                else
                                    separated[pair] = true;
                            }
                        }
                    }
                }
            }

            var hardPairs = hardlySeparatedPairs.Where(x => !separated[x]);
            File.WriteAllLines("hard.txt", hardPairs.Select(pair => string.Format("{0} {1}", pair.Item1, pair.Item2)));
        }

        public static List<int[]> AllPermutations()
        {
            var permutations = new List<int[]>();
            var permutation = new []{0, 1, 2, 3, 4};
            while(permutation != null)
            {
                permutations.Add(permutation);
                permutation = NextPermutation(permutation);
            }

            return permutations;
        }

        public static void RandomPermutationAutomatas()
        {
            var file = new FileInfo("separation.txt");
            file.Delete();

            var streamWriter = file.AppendText();

            const int numberOfStates = 5;
            const int stringLength = 40;
            const int experimentsNumder = 100000;

            for (var i = 0; i < experimentsNumder; i++)
            {
                var pairOfStrings = StringPairFactory.GetPairOfEquivalentStrings(stringLength, numberOfStates - 1);
                var wasSeparated = false;

                var iterations = 0;
                while (!wasSeparated && iterations < 10)
                {
                    var automata = PermutationAutomataFactory.GetRandomPermutationAutomata(numberOfStates);
                    if (automata.LastState(pairOfStrings.Item1) != automata.LastState(pairOfStrings.Item2))
                    {
                        wasSeparated = true;
                    }
                    iterations++;
                }

                if (!wasSeparated)
                {
                    streamWriter.WriteLine("{0} {1}", pairOfStrings.Item1, pairOfStrings.Item2);
                }

                Console.WriteLine(i);
            }

            streamWriter.Close();
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

        private static string PermutationToString(List<int> permutation)
        {
            return permutation.Aggregate("", (current, i) => current + (i + " "));
        }
    }
}