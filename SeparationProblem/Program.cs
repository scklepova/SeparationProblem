using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SeparationProblem.Extensions;

namespace SeparationProblem
{
    class Program
    {
        static void Main(string[] args)
        {
//            OpenFile();
//            SelectOnlyWordsWithEqualCountOfZeroAnd1();
//            Words2();
//            ConvertStringsToNums();
//            WordsWithoutThreeDigitsInARow2();
//            JoinBackups();
//            RandomPermutationAutomatas_FiltrationExperiment();
            AllPermutationAutomatas();
//            AllPathsOf();
//            FilterHard();

//            AllAutomatasVsPermEqualities();

//            WordsWithoutFourDigitsInARow2();
//            RandomPermutationAutomatasWithExactPairs();
//            AllPermutationAutomatasWithExactPairs();

//            FilterNoFourInARow();

//            FilterAllEqualities();
//            ComplementEqualities();

//            AllAutomatas();
//            WriteAutomata();

//            PermutationAutomatasForSwappedCyclesStrings();
//            AddPrefixAndSuffixForNotSeparating_infiniteCycle();
//            AddPref4();
//            FirstSeparatingAutomataNumberForAllEqualities4();

//            AllAutomatasVsFilterByRandom();

//            SwapCycles_OldVersion();
//            ShortestEquality4();
//            GetEqualitiesForPermutationAutomata();
//            GetEqualitiesForAutomata();

//            PseudoPalindromes5Equalities();

//            GetEqualitiesUsingHashtable();
//            TrimCommonEndings();

            GetEqualitiesOfConcreteLengthForAutomata();
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

        public static void RandomPermutationAutomatasWithExactPairs()
        {
            var lines = File.ReadAllLines("EqualCountnoThreeInARow24.txt");
            Console.WriteLine(lines.Count());
            var expCount = 30;

            var streamWriter = new StreamWriter(File.OpenWrite("separation.txt")) { AutoFlush = true };

            for (var i = 27000; i < lines.Count(); i++)
            {
                var line0 = lines[i];
                Console.WriteLine("i: {0}", i);
                for (var j = 0; j < lines.Count(); j++)
                {
                    var line1 = lines[j];
                    var line = line0 + line1;
                    if(line[0] == line.Last())
                        continue;
                    if (line.Contains("000") || line.Contains("111"))
                        continue;
                    var pair = new Tuple<string, string>(line, new string(line.Reverse().ToArray()));
                    if (pair.Item1 == pair.Item2)
                        continue;
                    var separated = false;
                    for(var c = 0; c < expCount; c++)
                    {
                        var automata = AutomataFactory.GetRandomPermutationAutomata(5);
                        if (automata.Separates(pair))
                        {
                            separated = true;
                            break;
                        }
                    }
                    if (!separated)
                    {
//                        Console.WriteLine("Success!!!");
//                        Console.WriteLine(line);
                        streamWriter.WriteLine(pair.ToStr());
                    }
                }
            }
            streamWriter.Close();
        }

        public static void AllPermutationAutomatasWithExactPairs()
        {
            var automatas = AutomataFactory.GetAllAutomatas(6);
//            var streamReader = new StreamReader(File.OpenRead("separation.txt"));
            var lines = File.ReadAllLines("FilteredNoFourInARow.txt");
            
            var streamWriter = new StreamWriter(File.OpenWrite("hard.txt")) {AutoFlush = true};

//            while(!streamReader.EndOfStream)
//            {
            for(var i = 0; i < lines.Count(); i++)
            {
                var line0 = lines[i];
                if (line0.StartsWith("1"))
                    break;
                Console.WriteLine("i: {0}", i);
                for(var j = 0; j < lines.Count(); j++)
                {
                    var line1 = lines[j];
//                    var line = streamReader.ReadLine();
//                    if (line == null)
//                        continue;
//                    var pair = new Tuple<string, string>(line.Split(' ')[0], line.Split(' ')[1]);
                    if(line0[0] == line1.Last())
                        continue;

                    var line = line0 + line1;
                    if(line.Contains("0000") || line.Contains("1111"))
                        continue;
                    var pair = new Tuple<string, string>(line, line.GetReverse());
                    if (pair.Item1 == pair.Item2)
                        continue;

                    if (FirstSeparatingAutomataNumber(automatas, pair) == -1)
                    {
//                        hardPairs.Add(pair);
                        Console.WriteLine("Success!!!");
                        Console.WriteLine(line);
                        streamWriter.WriteLine(pair.ToStr());
                    }
                }

            }
//            streamReader.Close();
            streamWriter.Close();
            Console.WriteLine("The end");
            Console.ReadKey();
        }

        public static void AllPermutationAutomatas()
        {
            var automatas = AutomataFactory.GetAllPermutationAutomata(5);
//            var sets = AutomataFactory.GetAllSetsOfInt(6);
//            var automatas = AutomataFactory.GetAllAutomatas(6);
            var streamReader = new StreamReader(File.OpenRead("separation.txt"));

//            var hardlySeparatedPairs = File.ReadAllLines("separation.txt").Select(x => new Tuple<string, string>(x.Split(' ')[0], x.Split(' ')[1])).ToList();
            var hardPairs = new List<Tuple<string, string>>();

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line != null)
                {
                    var words = line.Split(' ');
                    var pair = new Tuple<string, string>(words[0], words[1]);
                    var separated = false;
                    foreach (var automata in automatas)
                    {
                        if (automata.Separates(pair))
                        {
                            separated = true;
                            WriteAutomata(automata, pair);
                            break;
                        }
                    }
                    if (!separated)
                        hardPairs.Add(pair);
                }
            }

            File.WriteAllLines("hard.txt", hardPairs.Select(pair => string.Format("{0} {1}", pair.Item1, pair.Item2)));
            Console.WriteLine("The end");
            Console.ReadKey();
        }

        public static void RandomPermutationAutomatas_FiltrationExperiment()
        {
            var lines = File.ReadAllLines("EqualCountnoThreeInARow26.txt");

            var file = new FileInfo("separation.txt");
            file.Delete();

            var streamWriter = file.AppendText();

            const int numberOfStates = 5;
            const int stringLength = 40;
            const int experimentsNumder = 1000000;

            for (var i = 0; i < experimentsNumder; i++)
            {
                Tuple<string, string> pairOfStrings;
                while (true)
                {
                    var randLine = lines[RandomFactory.GetNext(lines.Count())] +
                                   lines[RandomFactory.GetNext(lines.Count())];
                    pairOfStrings = new Tuple<string, string>(randLine, new string(randLine.Reverse().ToArray()));
                    if (pairOfStrings.Item1 != pairOfStrings.Item2)
                        break;
                }

                var wasSeparated = false;
                var iterations = 0;
                while (!wasSeparated && iterations < 20)
                {
                    var automata = AutomataFactory.GetRandomPermutationAutomata(numberOfStates);
                    if (automata.Separates(pairOfStrings))
                    {
                        wasSeparated = true;
                    }
                    iterations++;
                }

                if (!wasSeparated)
                {
                    streamWriter.WriteLine("{0} {1}", pairOfStrings.Item1, pairOfStrings.Item2);
                }

            }
            streamWriter.Close();
        }

        public static List<Tuple<string, string>> FilterPairsByRandomAutomatas(List<Tuple<string, string>> pairs,
            Func<int, Automata> getRandomAutomata, int numberOfStates, int filterIterationsCount)
        {
            var filtered = new List<Tuple<string, string>>();
            foreach (var pair in pairs)
            {
                var separated = false;
                for (var i = 0; i < filterIterationsCount; i++)
                {
                    var automata = getRandomAutomata(numberOfStates);
                    if (automata.Separates(pair))
                    {
                        separated = true;
                        break;
                    }
                }

                if(!separated)
                    filtered.Add(pair);
            }

            return filtered;
        }

        public static void AllAutomatasVsFilterByRandom()
        {
            var filename = "totalEqualities4.txt";
            var n = 5;
            var sets = AutomataFactory.GetAllSetsOfInt(n);
            var automatas = AutomataFactory.GetAllConnectedAutomatasWithKnownSets(n, sets);
            var lines = File.ReadAllLines(filename);
            var pairs = new List<Tuple<string, string>>();
            for (var i = 2; i < 300; i += 3)
                pairs.Add(lines[i].GetPair());
            Console.WriteLine(pairs.Count);

            Console.WriteLine("Start hard separating");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var pair in pairs)
            {
                FirstSeparatingAutomataNumber(automatas, pair);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Start with filtering");
            stopwatch.Reset();

            stopwatch.Start();
            var filtered = FilterPairsByRandomAutomatas(pairs, x => AutomataFactory.GetRandomAutomata(n), n, 20);
            foreach (var pair in filtered)
            {
                FirstSeparatingAutomataNumber(automatas, pair);
            }
            stopwatch.Stop();
            Console.WriteLine(filtered.Count);
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine("End.");
            Console.ReadKey();
        }

        public static void WordsWithoutThreeDigitsInARow2()
        {
            var filename = "noThreeInARow.txt";
            var lines = File.ReadAllLines(filename);
            var newLines = new List<string>();

            foreach (var line in lines)
            {
                if (line != null)
                {
                    if (!line.EndsWith("00"))
                        newLines.Add(line + "0");
                    if (!line.EndsWith("11"))
                        newLines.Add(line + "1");
                }
            }

            File.WriteAllLines(filename, newLines);
        }

        public static void WordsWithoutThreeDigitsInARow()
        {
            var filename = "noThreeInARow.txt";
            var streamReader = new StreamReader(File.OpenRead(filename));
            var streamWriter = new StreamWriter(File.OpenWrite(filename + "Write"));

            while(!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line != null)
                {
                    if (!line.EndsWith("00"))
                        streamWriter.WriteLine(line + "0");
                    if (!line.EndsWith("11"))
                        streamWriter.WriteLine(line + "1");
                }
            }
        }

        public static void ConvertStringsToNums()
        {
            var filename = "noThreeInARow.txt";
            var streamReader = new StreamReader(File.OpenRead(filename));
            var streamWriter = new StreamWriter(File.OpenWrite("Nums" + filename));

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line != null)
                {
                    var num = line.ToLong();
                    streamWriter.WriteLine(num);
                }
            }
        }

        public static void Words2()
        {
            var filename = "noThreeInARow.txt";
            var streamReader = new StreamReader(File.OpenRead(filename));
            var streamWriter = new StreamWriter(File.OpenWrite("Nums" + filename));

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line != null)
                {
                    var str = LongToStr(long.Parse(line));
                    if (!str.EndsWith("00"))
                    {
                        var num = (str + "0").ToLong();
                        streamWriter.WriteLine(num);
                    }
                    if (!str.EndsWith("11"))
                    {
                        var num = (str + "1").ToLong();
                        streamWriter.WriteLine(num);
                    }       
                }
            }
        }

        private static string LongToStr(long num)
        {
            var str = "";
            while (num != 0)
            {
                var rem = num%2;
                str = rem + str;
                num = num/2;
            }
            return str;
        }

        private static void SelectOnlyWordsWithEqualCountOfZeroAnd1()
        {
            var filename = "noFourInARow.txt";
            var streamReader = new StreamReader(File.OpenRead(filename));
            var streamWriter = new StreamWriter(File.OpenWrite("EqualCount" + filename));
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line != null)
                {
                    if (line.HasEqualCountOf0And1()) 
                        streamWriter.WriteLine(line);
                }
            }
        }

        private static void FilterHard()
        {
            var filename = "hard.txt";
            var pairs = File.ReadAllLines(filename).Select(x => x.Split(' ')).ToArray();
            var diffs = new List<string[]>();
            foreach (var pair in pairs)
            {
                if(pair[0] != pair[1])
                    diffs.Add(pair);
            }

            File.WriteAllLines(filename, diffs.Select(x => string.Format("{0} {1}", x[0], x[1])));
        }

        public static void AllAutomatasVsPermEqualities()
        {
            var automatas = AutomataFactory.GetAllNotPermutationAutomatas(4);
//            var prefix = "1101100000000";
            var prefix = "010110110011100100";
            

            var hard = new List<Tuple<string, string>>();
            var equalities =
                File.ReadAllLines("separation.txt")
                    .Select(x => x.GetPair())
                    .ToList();
            var streamWriter = new StreamWriter(File.OpenWrite("hard.txt")) {AutoFlush = true};
            while (true)
            {
                Console.WriteLine(prefix);
                foreach (var pair in equalities)
                {
                    var reverseP = prefix.GetReverse();
                    var newPair = new Tuple<string, string>(prefix + pair.Item1 + reverseP,
                        prefix + pair.Item2 + reverseP);
//                    Console.WriteLine(newPair.ToStr());
                    var f = false;
                    foreach (var automata in automatas)
                    {
                        if (automata.Separates(newPair))
                        {
                            f = true;
                            break;
                        }
                    }

                    if (!f)
                    {
                        hard.Add(newPair);
                        Console.WriteLine("Success!!!");
                        streamWriter.WriteLine("Success");
                        streamWriter.WriteLine(prefix);
                        streamWriter.WriteLine(newPair.ToStr());
                    }               
                }

                do
                {
                    prefix = prefix.Select(x => x - '0').ToArray().PlusOneIn2ss(prefix.Length - 1).GetString();
                } while (!prefix.HasEqualCountOf0And1());
            }

            File.WriteAllLines("hard.txt", hard.Select(x => string.Format("{0} {1}", x.Item1, x.Item2)));
            Console.ReadKey();
        }

        public static void WordsWithoutFourDigitsInARow2()
        {
            var lines = new List<string>(){"000", "001", "010", "011", "100", "101", "110", "111"};
            while (lines[0].Length < 24)
            {
                var newLines = new List<string>();

                foreach (var line in lines)
                {
                    if (line != null)
                    {
                        if (!line.EndsWith("000"))
                            newLines.Add(line + "0");
                        if (!line.EndsWith("111"))
                            newLines.Add(line + "1");
                    }
                }

                lines = newLines.ToList();
            }

            File.WriteAllLines("noFourInARow.txt", lines);
        }

        public static void FilterNoFourInARow()
        {
            var lines = File.ReadAllLines("noFourInARow.txt");
            var newLines = new List<string>();

            foreach (var line in lines)
            {
                if(line.Contains("000") || line.Contains("111"))
                    newLines.Add(line);
            }

            File.WriteAllLines("FilteredNoFourInARow.txt", newLines);
        }

        public static void FilterAllEqualities()
        {
            var filename = "allEqualitiesIKnow.txt";
            var equalities = File.ReadAllLines(filename).Select(x => x.GetPair());
            var filteredEqualities = new List<Tuple<string, string>>();

            foreach (var equality in equalities)
            {
                if(!filteredEqualities.HasSameEquality(equality))
                    filteredEqualities.Add(equality);
            }

            File.WriteAllLines("filtered" + filename, filteredEqualities.Select(x => x.ToStr()));
        }

        public static void ComplementEqualities()
        {
            var filename = "filteredallEqualitiesIKnow.txt";
            var equalities = File.ReadAllLines(filename).Select(x => x.GetPair());
            var newEqs = new List<Tuple<string, string>>(equalities); 
            foreach (var equality in equalities)
            {
                newEqs.Add(new Tuple<string, string>(equality.Item1.ComplementString(), equality.Item2.ComplementString()));
            }

            File.WriteAllLines("complementedEqualities.txt", newEqs.Select(x => x.ToStr()));
        }

        public static void AllAutomatas()
        {
            var filename = "eq4.txt";
            var lines = File.ReadAllLines(filename);
            var sets = AutomataFactory.GetAllSetsOfInt(5);
//            File.WriteAllLines("sets6.txt", sets.Select(x => x.ToStr()));
            var automatas = AutomataFactory.GetAllAutomatasWithKnownSets(5, sets);
            var streamWriter = new StreamWriter(File.OpenWrite("totalEqualities5_2.txt")) {AutoFlush = true};
//            Console.WriteLine(automatas.Count());
            var critAutos = new List<Automata>()
            {
                automatas.ElementAt(5410719)
                //***
//                automatas.ElementAt(14849260)

//                automatas.ElementAt(5182605)

//                automatas.ElementAt(4661015)

//                automatas.ElementAt(1523795)

//                automatas.ElementAt(602385),
//                automatas.ElementAt(602389),
//                automatas.ElementAt(602375),
//                automatas.ElementAt(599250),
//                automatas.ElementAt(602420),
//                automatas.ElementAt(599450),
//                automatas.ElementAt(1147297),
//                automatas.ElementAt(1086792)
            };

            var line = lines[0];
            var prefix = "";
            while(true)
            {
                Console.WriteLine(prefix);
                var pair = line.GetPair();
                var rev = prefix.GetReverse();
                pair = new Tuple<string, string>(prefix + pair.Item1 + rev, prefix + pair.Item2 + rev);
                var separated = false;
                var automataCount = 0;
                foreach (var automata in automatas)
                {
                    if (automata.Separates(pair))
                    {
                        separated = true;
                        Console.WriteLine("Separated  " + automataCount);
                        break;
                    }
                    automataCount++;
//                    if(automataCount % 10000 == 0)
//                        Console.WriteLine(automataCount);
                }

                if (!separated)
                {
                    Console.WriteLine("Success!");
                    Console.WriteLine(prefix);
                    streamWriter.WriteLine(pair.ToStr());
                    break;
                }

//                do
//                {
                    prefix = prefix.BinaryPlusOne();
//                } while (!prefix.HasEqualCountOf0And1());
                
            }

            streamWriter.Close();
            Console.WriteLine("end");
            Console.ReadKey();
        }

        public static void AddPrefixAndSuffixForNotSeparating_infiniteCycle()
        {
            const string inputFilename = "eq4.txt";
            var pair = File.ReadAllLines(inputFilename)[0].GetPair();
            var setsOfInt = AutomataFactory.GetAllSetsOfInt(5);
            var automatas = AutomataFactory.GetAllConnectedAutomatasWithKnownSets(5, setsOfInt).ToList();
//            Console.WriteLine(automatas.Count());
            var lastSeparatedAutomataNumber = -2;

            while (lastSeparatedAutomataNumber < automatas.Count - 1)
            {
                var separatedAutomataNumber = FirstSeparatingAutomataNumber(automatas, pair);
                if (separatedAutomataNumber == - 1)
                    break;

                Console.WriteLine("Separated: " + separatedAutomataNumber);
                if (separatedAutomataNumber < lastSeparatedAutomataNumber)
                {
                    Console.WriteLine("Crash!");
                    break;
                }

                var prefix = "0";
                var suffix = "0";
                var hardAutomata = automatas.ElementAt(separatedAutomataNumber);
                var goodPrefixes = new List<string>();
                var goodSuffixes = new List<string>();

                var maxSeparated = 0;
                var maxprefix = "";
                var maxSuffix = "";
                var iter = 0;
                while (maxSeparated <= separatedAutomataNumber)
                {
                    Console.WriteLine(iter);
                    while (goodPrefixes.Count < 20)
                    {
                        Console.WriteLine(prefix);
                        if (
                            !hardAutomata.Separates(prefix + pair.Item1 + suffix,
                                prefix + pair.Item2 + suffix))
                        {
                            goodPrefixes.Add(prefix);
                            goodSuffixes.Add(suffix);
                        }
//                        do
//                        {
                            prefix = prefix.BinaryPlusOne();
//                        } while (!prefix.HasEqualCountOf0And1());
                        suffix = prefix;
//                        var newSuffPref = GetNextPrefixAndSuffix(prefix, suffix, 3);
//                        prefix = newSuffPref.Item1;
//                        suffix = newSuffPref.Item2;
//                        if (prefix.Length > 4)
//                            break;
                    }


                    for(var i = 0; i < goodPrefixes.Count; i++)
                    {
                        var goodPrefix = goodPrefixes[i];
                        var goodSuffix = goodSuffixes[i];
                        var separated = FirstSeparatingAutomataNumber(automatas,
                            new Tuple<string, string>(goodPrefix + pair.Item1 + goodSuffix,
                                goodPrefix + pair.Item2 + goodSuffix));
                        if (separated > maxSeparated || separated == -1)
                        {
                            maxSeparated = separated;
                            maxprefix = goodPrefix;
                            maxSuffix = goodSuffix;
                            if (maxSeparated == -1)
                                break;
                        }
                    }
                    iter++;
                }


                pair = new Tuple<string, string>(maxprefix + pair.Item1 + maxSuffix,
                    maxprefix + pair.Item2 + maxSuffix);
                Console.WriteLine("Prefix: {0} Suffix: {2}  Separated: {1}", maxprefix, maxSeparated, maxSuffix);
                separatedAutomataNumber = maxSeparated;
                lastSeparatedAutomataNumber = separatedAutomataNumber;
            }
            
            var streamWriter = new StreamWriter(File.OpenWrite("totalEqualities5_3.txt")){AutoFlush = true};
            streamWriter.WriteLine(pair.ToStr());
            streamWriter.Close();
            Console.WriteLine("end");
            Console.ReadKey();
        }

        private static Tuple<string, string> GetNextPrefixAndSuffix(string prefix, string suffix, int maxLen)
        {
            var newSuffix = suffix.BinaryPlusOne();
            if (newSuffix.Length > maxLen)
                return new Tuple<string, string>(prefix.BinaryPlusOne(), "");
            return new Tuple<string, string>(prefix, newSuffix);
        }

        public static void AddPref4()
        {
            var sets = AutomataFactory.GetAllSetsOfInt(4);
            var automatas = AutomataFactory.GetAllNotPermutationAutomatas(4).ToList();
            var pair = File.ReadAllLines("eq4.txt")[0].GetPair();
//            var prefix = "11000000";
            var prefix = "000";
            var eq = false;
            var suffix = "";

            while (suffix.Length + prefix.Length <= 18)
            {
//                suffix = prefix.GetReverse();
                suffix = "000";
                while (suffix.Length + prefix.Length <= 18)
                {
                    if (suffix.Length + prefix.Length <= 16)
                    {
                        suffix = suffix.BinaryPlusOne();
                        continue;
                    }
                    Console.WriteLine("{0} {1}", prefix, suffix);

                    if (FirstSeparatingAutomataNumber(automatas, prefix + pair.Item1 + suffix, prefix + pair.Item2 + suffix) == -1)
                    {
                        eq = true;
                        Console.WriteLine("Success!!!");
                        break;
                    }
                    do
                    {
                        suffix = suffix.BinaryPlusOne();
                    } while (suffix.HasEqualCountOf0And1());
                }

                if (eq)
                {
                    Console.WriteLine("Success!!!");
                    break;
                }
//                do
//                {
                    prefix = prefix.BinaryPlusOne();
//                } while (prefix.HasEqualCountOf0And1());
                suffix = "0000";
            }

            Console.WriteLine("end");
            pair = new Tuple<string, string>(prefix + pair.Item1 + suffix,
                        prefix + pair.Item2 + suffix);
            File.WriteAllLines("totalEqualities4_5.txt", new []{pair.ToStr()});
            Console.ReadKey();
        }

        public static int FirstSeparatingAutomataNumber(IEnumerable<Automata> automatas, Tuple<string, string> pair)
        {
            var automataNumber = 0;
            foreach (var automata in automatas)
            {
                if (automata.Separates(pair))
                {
                    return automataNumber;
                }
                automataNumber++;
            }

            
            Console.WriteLine("Success!!");
            return -1;
        }

        public static int FirstSeparatingAutomataNumber(IEnumerable<Automata> automatas, string w1, string w2)
        {
            var pair = new Tuple<string, string>(w1, w2);
            return FirstSeparatingAutomataNumber(automatas, pair);
        }

        public static void WriteAutomata(Automata a, Tuple<string, string> pair)
        {
//            var automatas = AutomataFactory.GetAllAutomatas(5);
//            var a = automatas.ElementAt(1147295);
            Console.WriteLine(a.ToString());
//            Console.WriteLine();
//
//            var pair =
//                new Tuple<string, string>(
//                    "0011001110010101010101010101011010101010101010101001010101101010100111001100",
//                    "0011001110010101011010101001010101010101010101101010101010101010100111001100");

            Console.WriteLine(pair.Item1);
            a.LastState(pair.Item1);
            Console.WriteLine();
            
            a.LastState(pair.Item2);
            Console.WriteLine();
            Console.WriteLine(pair.Item2);

            Console.ReadKey();
        }

        public static void PermutationAutomatasForSwappedCyclesStrings()
        {
            const string filename = "swappedEqualities.txt";
            var logWriter = new StreamWriter(File.OpenWrite("logSwapped.txt")){AutoFlush = true};
            var equalities = new List<Tuple<string, string>>();
            const int experimientsNumber = 100000;
            const int automatasNumber = 10;
            const int stringLength = 40;
            const int stretch = 5;
            const int statesCount = 5;
            var allAutomatas = AutomataFactory.GetAllAutomatas(5);
            var iterCount = 0;
            var sum = 0;

//            while (!equalities.Any())
            while(iterCount < 7)
            {
                Console.WriteLine("Iteration");
                var hardlySeprated = new List<Tuple<string, string>>();
                equalities = new List<Tuple<string, string>>();

                for (var i = 0; i < experimientsNumber; i++)
                {
                    var pairs =
                        StringPairFactory.GetPairOfEquivalentStringsWithEqual01(
                            RandomFactory.GetRandomString(stringLength), stretch);

                    foreach (var pair in pairs)
                    {
                        var separated = false;
                        for (var j = 0; j < automatasNumber; j++)
                        {
                            var automata = AutomataFactory.GetRandomPermutationAutomata(statesCount);
                            if (automata.Separates(pair))
                            {
                                separated = true;
                                break;
                            }
                        }

                        if (!separated)
                            hardlySeprated.Add(pair);
                        break;
                    }
                    
                }

//                foreach (var pair in hardlySeprated)
//                {
//                    var separated = false;
//                    foreach (var automata in allAutomatas)
//                    {
//                        if (automata.Separates(pair))
//                        {
//                            separated = true;
//                            break;
//                        }
//                    }
//
//                    if(!separated)
//                        equalities.Add(pair);
//                }

                logWriter.WriteLine("Hardly: {0};  Equalities: {1}", hardlySeprated.Count, equalities.Count);
                Console.WriteLine(hardlySeprated.Count);
                iterCount++;
                sum += hardlySeprated.Count;
            }
            
            Console.WriteLine("Average " + sum / iterCount);

            logWriter.Close();
            File.WriteAllLines(filename, equalities.Select(x => x.ToStr()));
            Console.ReadKey();
        }

        public static void FirstSeparatingAutomataNumberForAllEqualities4()
        {
            const string filename = "totalEqualities4_4.txt";
            var lines = File.ReadAllLines(filename);
            var sets = AutomataFactory.GetAllSetsOfInt(5);
            var automatas = AutomataFactory.GetAllConnectedAutomatasWithKnownSets(5, sets);
            var streamWriter = new StreamWriter(File.OpenWrite("firstSeparatedAutomataLog_2.txt")) {AutoFlush = true};
            var maxSeparatedPair = "";
            var maxSep = 0;
            for (var i = 2; i < lines.Count(); i += 3)
            {
                var line = lines[i];
                var firstSeparated = FirstSeparatingAutomataNumber(automatas, line.GetPair());
                streamWriter.WriteLine("{1} separated {0}", line, firstSeparated);
                Console.WriteLine("{1} separated {0}", line, firstSeparated);
                if (firstSeparated > maxSep || firstSeparated == -1)
                {
                    maxSep = firstSeparated;
                    maxSeparatedPair = line;
                }
            }
            streamWriter.WriteLine("Max: {0} separated {1}", maxSep, maxSeparatedPair);

            Console.WriteLine("The end");
            streamWriter.Close();
            Console.ReadKey();
        }

        public static void SwapCycles_OldVersion()
        {
            var file = new FileInfo("separation_old.txt");
            file.Delete();

            var streamWriter = file.AppendText();

            const int numberOfStates = 5;
            const int stringLength = 40;
            const int experimentsNumder = 100000;
            var eq = 0;

            for (var i = 0; i < experimentsNumder; i++)
            {
                var pairsOfStrings = new List<Tuple<string, string>>();
                while(pairsOfStrings.Count == 0)
                    pairsOfStrings = StringPairFactory.GetPairOfEquivalentStringsWithEqual01(RandomFactory.GetRandomString(stringLength), numberOfStates - 1);

                foreach (var pairOfStrings in pairsOfStrings)
                {
                    var wasSeparated = false;

                    var iterations = 0;
                    while (!wasSeparated && iterations < 10)
                    {
                        var automata = AutomataFactory.GetRandomPermutationAutomata(numberOfStates);
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
                }

                Console.WriteLine(i);
            }

            streamWriter.Close();
            Console.WriteLine(eq);
            Console.ReadKey();
        }

        public static void ShortestEquality4()
        {
            var sets = AutomataFactory.GetAllSetsOfInt(4);
            var allAutomatas = AutomataFactory.GetAllConnectedAutomatasWithKnownSets(4, sets).ToList();
            var automatas = AutomataFactory.GetAllPermutationAutomata(4).ToList();
            var eq = false;
            var len = 17;
            Tuple<string, string> equality = null;

            while (!eq)
            {
                var words = WordsFactory.GetAllWordsOfLength(len);
                for(var i = 0; i < words.Count; i++)
                {
                    var w1 = words[i];
                    if (w1[0] == '1')
                        break;
                    for(var j = i + 1; j < words.Count; j++)
                    {
                        var w2 = words[j];
//                        if (w1 != w2 && w1.Substring(0, 3) == w2.Substring(0, 3) && w1.Substring(len - 4) == w2.Substring(len - 4) && w1.Count1() == w2.Count1())
                        if(w1 != w2 && w1[0] == w2[0])
                        {
                            Console.WriteLine("{0} {1}", w1, w2);

                            if (FirstSeparatingAutomataNumber(automatas, w1, w2) == -1 &&
                                FirstSeparatingAutomataNumber(allAutomatas, w1, w2) == -1)
                            {
                                eq = true;
                                Console.WriteLine("Success!!");
                                Console.WriteLine("{0} {1}", w1, w2);
                                equality = new Tuple<string, string>(w1, w2);
                                break;
                            }
                        }
                    }
                    if (eq)
                        break;
                }
                len++;
            }

            File.WriteAllText("shortestEquality4_1.txt", equality.ToStr());
            Console.ReadKey();
        }

        public static void GetEqualitiesForPermutationAutomata()
        {
            var streamWriter = new StreamWriter(File.OpenWrite("permutationEqualities4.txt")) {AutoFlush = true};
            var len = 14;
            var automatas = AutomataFactory.GetAllPermutationAutomata(4).ToList();
            var words = WordsFactory.GetAllWordsOfLength(len);
            for (var i = 0; i < words.Count; i++)
            {
                var w1 = words[i];
                if (w1[0] == 1)
                    break;
                for (var j = i + 1; j < words.Count; j++)
                {
                    var w2 = words[j];
                    if (w1[0] != w2[0] && w1.Last() != w2.Last())
                    {
                        if(FirstSeparatingAutomataNumber(automatas, w1, w2) == -1)
                            streamWriter.WriteLine("{0} {1}", w1, w2);
                    }
                }
            }
            streamWriter.Close();
            Console.WriteLine("The end");
            Console.ReadKey();
        }

        public static void GetEqualitiesOfConcreteLengthForAutomata()
        {
            var streamWriter = new StreamWriter(File.OpenWrite("equalities3.txt")) { AutoFlush = true };
            var len = 10;
            var automatas = AutomataFactory.GetAllAutomatas(3).ToList();
            var words = WordsFactory.GetAllWordsOfLength(len);
            for (var i = 0; i < words.Count; i++)
            {
                var w1 = words[i];
                if (w1[0] == 1)
                    break;
                for (var j = i + 1; j < words.Count; j++)
                {
                    var w2 = words[j];
                    if (w1[0] == w2[0] && w1.Last() == w2.Last())
                    {
                        if (FirstSeparatingAutomataNumber(automatas, w1, w2) == -1)
                            streamWriter.WriteLine("{0} {1}", w1, w2);
                    }
                }
            }
            streamWriter.Close();
            Console.WriteLine("The end");
            Console.ReadKey();
        }

        public static void GetEqualitiesForAutomata()
        {
//            1100101 0010011100
            var permEqualities = File.ReadAllLines("permutationEqualities5.txt").Select(x => x.GetPair());
            var len = 48;
            var minLen = 46;
            var affixes = WordsFactory.GetAllWordsOfLength(1);
            for(var i = 2; i <= 16; i++)
                affixes.AddRange(WordsFactory.GetAllWordsOfLength(i));
            var automatas = AutomataFactory.GetAllConnectedAutomatasWithKnownSets(5, AutomataFactory.GetAllSetsOfInt(5)).ToList();
            var writer = new StreamWriter(File.OpenWrite("shortestEqualities5_1.txt")) {AutoFlush = true};
            var eq = false;

            foreach (var permEquality in permEqualities)
            {
                Console.WriteLine(permEquality.ToStr());
                if (eq)
                    break;
//                foreach (var prefix in affixes)
                for(var i = affixes.FindIndex(x => x == "010101") + 1; i < affixes.Count; i++)
                {
                    var prefix = affixes[i];
                    if (eq)
                        break;
                    if(prefix.Length < 3)
                        continue;
                    foreach (var suffix in affixes)
                    {
                        if(suffix == "01010101")
                            continue;
                        if (eq)
                            break;
                        if(suffix.Length < 4)
                            continue;
                        if (suffix.Length + permEquality.Item1.Length + prefix.Length <= minLen)
                            continue;
                        if (suffix.Length + permEquality.Item1.Length + prefix.Length > len)
                            break;
                        Console.WriteLine("{0} {1}", prefix, suffix);

                        var pair = permEquality.AddPrefixAndSuffix(prefix, suffix);
                        if (FirstSeparatingAutomataNumber(automatas, pair) == -1)
                        {
                            writer.WriteLine(pair.ToStr());
                            eq = true;
                            break;
                        }
                    }
                }
            }

            writer.Close();
            Console.WriteLine("The end");
            Console.ReadKey();
        }

        public static void PseudoPalindromes5Equalities()
        {
            var automatas = AutomataFactory.GetAllNonIsomorphicPermutationAutomatas5();
            var len = 19;
            var drafts = WordsFactory.GetAllWordsOfLength(len);
            var used = drafts.ToDictionary(x => x.DraftToString("01", "10"), x => false);
            var writer = new StreamWriter(File.OpenWrite("evenEqualities38_1.txt")) {AutoFlush = true};
            for(var i = 0; i < drafts.Count; i++)
            {
                if (drafts[i][0] == '1')
                    break;
                var w1 = drafts[i].DraftToString("01", "10");
                if(used[w1])
                    continue;
                for(var j = i + 1; j < drafts.Count; j++)
                {
                    if (drafts[i] == drafts[j] || drafts[i][0] == drafts[j][0] || drafts[i].Last() == drafts[j].Last() )
//                        || drafts[i].Count1() != drafts[j].Count1())
                        continue;
                    
                    var w2 = drafts[j].DraftToString("01", "10");
                    if (used[w2] )//|| (w1.FactorCount("00") % 2 == w2.FactorCount("00") % 2) && (w1.FactorCount("11") % 2 == w2.FactorCount("11") % 2))
                        continue;
//                    Console.WriteLine("{0} {1}", drafts[i], drafts[j]);
                    if (FirstSeparatingAutomataNumber(automatas, w1, w2) == -1)
                        writer.WriteLine("{0} {1}", w1, w2);
                }
                used[w1] = true;
                used[w1.GetReverse()] = true;
                used[w1.GetInverse()] = true;
                used[w1.GetReverse().GetInverse()] = true;
            }

            writer.Close();
            Console.WriteLine("The end");
            Console.ReadKey();
        }

        public static void GetEqualitiesUsingHashtable()
        {
            var automatas = AutomataFactory.GetAllNonIsomorphicPermutationAutomatas5();
            var automataIndices = new int[] {0, 10, 30, 70, 100, 120, 240, 370, 460, 540};
            var fixedAutomatas = automataIndices.Select(index => automatas[index]).ToList();
            var len = 10;
            var drafts = WordsFactory.GetAllWordsOfLength(len);
            var words01 = drafts.Where(x => x[0] == '0').Select(x => x.DraftToString("0110", "1001"));
            var words10 = drafts.Where(x => x[0] == '1').Select(x => x.DraftToString("0110", "1001"));
            var hashtable = new Hashtable();

            foreach (var word in words01)
            {
                var hash = word.Get5Hash(fixedAutomatas);
                if(hashtable.ContainsKey(hash))
                    ((List<string>)hashtable[hash]).Add(word);
                else
                    hashtable.Add(hash, new List<string> {word});
            }

            var writer = new StreamWriter(File.OpenWrite("evenEqualities_5hash_40_4.txt")) {AutoFlush = true};

            foreach (var word in words10)
            {
                var hash = word.Get5Hash(fixedAutomatas);
                if (hashtable.ContainsKey(hash))
                {
                    var candidates = (List<string>) hashtable[hash];
                    foreach (var candidate in candidates)
                    {
                        if(FirstSeparatingAutomataNumber(automatas, word, candidate) == -1)
                            writer.WriteLine("{0} {1}", word, candidate);
                    }
                }
            }

            writer.Close();
            Console.WriteLine("The end");
            Console.ReadKey();
        }

        public static void TrimCommonEndings()
        {
            var filename = "evenEqualities_5hash_40.txt";
            var equalities = File.ReadAllLines(filename).Select(x => x.GetPair());
            var trimmedEqualities = new List<Tuple<string, string>>();
            foreach (var equality in equalities)
            {
                var i = equality.Item1.Length - 1;
                while (i >= 0 && equality.Item1[i] == equality.Item2[i])
                    i--;
                trimmedEqualities.Add(new Tuple<string, string>(equality.Item1.Substring(0, i + 1), equality.Item2.Substring(0, i + 1)));
            }

            File.WriteAllLines("trimmed_" + filename, trimmedEqualities.Select(x => x.ToStr()));
        }
    } 
}