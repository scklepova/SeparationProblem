using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
//            RandomPermutationAutomatas();
//            AllPermutationAutomatas();
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
//            AddPrefixAndSuffixForNotSeparating();
            AddPref4();
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
            var automatas = AutomataFactory.GetAllNonIsomorphicPermutationAutomatas5();
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
                    var separated = false;
                    foreach (var automata in automatas)
                    {
                        if (automata.Separates(pair))
                        {
                            separated = true;
                            break;
                        }
                    }
                    if (!separated)
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
//            while (true)
//            {
                var automatas = AutomataFactory.GetAllPermutationAutomata(7);
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
                                break;
                            }
                        }
                        if (!separated)
                            hardPairs.Add(pair);
                    }
                }
//                if (hardPairs.Any())
                    File.WriteAllLines("hard.txt", hardPairs.Select(pair => string.Format("{0} {1}", pair.Item1, pair.Item2)));
//                else
//                {
//                    streamReader.Close();
//                    Console.WriteLine("Again");
//                    RandomPermutationAutomatas();
//                    continue;
//                }
//                break;
//            }
        }

        public static void RandomPermutationAutomatas()
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

        private static void AllPathsOf()
        {
            var len = 37;
            var vertexes = new List<string> {"0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111"};

            edges = vertexes.ToDictionary(vertex => vertex, vertex => new List<string>());

            foreach(var u in vertexes)
            {
                foreach(var v in vertexes)
                {
                    if(u.Substring(1) == v.Substring(0, v.Length - 1))
                        edges[u].Add(v);
                }
            }

            pathStack = new Stack<string>();
            
            foreach(var vertex in vertexes)
            {
                used = new Dictionary<string, bool>();
                foreach(var v in vertexes)
                {
                    used.Add(v, false);
                }
                Dfs(vertex, len);
            }
        }

        private static void Dfs(string v, int len)
        {
            pathStack.Push(v);
            if(len == 0)
            {
                File.AppendAllText("paths.txt", string.Format("{0}\r\n", pathStack.ToList().PathToStr()));
                pathStack.Pop();
                return;
            }

            foreach(var u in edges[v])
            {
                Dfs(u, len - 1);
            }

            pathStack.Pop();
            return;
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

        private static void OpenFile()
        {
            var filename = "EqualCountnoThreeInARow36.txt";
            var lines = File.ReadAllLines(filename);

            lines.Count();
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
            var sets = AutomataFactory.GetAllSetsOfInt(6);
//            File.WriteAllLines("sets6.txt", sets.Select(x => x.ToStr()));
            var automatas = AutomataFactory.GetAllAutomatasWithKnownSets(6, sets);
            var streamWriter = new StreamWriter(File.OpenWrite("totalEqualities6.txt")) {AutoFlush = true};
//            Console.WriteLine(automatas.Count());
//            var critAutos = new List<Automata>()
//            {
//                automatas.ElementAt(14849260)
//
////                automatas.ElementAt(5182605)
//
////                automatas.ElementAt(4661015)
//
////                automatas.ElementAt(1523795)
//
////                automatas.ElementAt(602385),
////                automatas.ElementAt(602389),
////                automatas.ElementAt(602375),
////                automatas.ElementAt(599250),
////                automatas.ElementAt(602420),
////                automatas.ElementAt(599450),
////                automatas.ElementAt(1147297),
////                automatas.ElementAt(1086792)
//            };

            var line = lines[0];
//            var prefix = "010010";
//            while(true)
//            {
//                Console.WriteLine(prefix);
                var pair = line.GetPair();
//                var rev = prefix.GetReverse();
//                pair = new Tuple<string, string>(prefix + pair.Item1 + rev, prefix + pair.Item2 + rev);
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
                    if(automataCount % 10000 == 0)
                        Console.WriteLine(automataCount);
                }

                if (!separated)
                {
                    Console.WriteLine("Success!");
//                    Console.WriteLine(prefix);
                    streamWriter.WriteLine(pair.ToStr());
//                    break;
                }

//                do
//                {
//                    prefix = prefix.Select(x => x - '0').ToArray().PlusOneIn2ss(prefix.Length - 1).GetString();
//                } while (!prefix.HasEqualCountOf0And1());
                
//            }

            streamWriter.Close();
            Console.WriteLine("end");
            Console.ReadKey();
        }

        public static void AddPrefixAndSuffixForNotSeparating()
        {
            const string inputFilename = "eq4.txt";
            var pair = File.ReadAllLines(inputFilename)[0].GetPair();
            var setsOfInt = AutomataFactory.GetAllSetsOfInt(4);
            var automatas = AutomataFactory.GetAllConnectedAutomatasWithKnownSets(4, setsOfInt).ToList();
//            Console.WriteLine(automatas.Count());
            var lastSeparatedAutomataNumber = -2;
            var separatedAutomataNumber = -1;

            while (lastSeparatedAutomataNumber < automatas.Count - 1)
            {
                separatedAutomataNumber = SeparatedAutomataCount(automatas, pair);
                if (separatedAutomataNumber >= automatas.Count - 1)
                    break;

                Console.WriteLine("Separated: " + separatedAutomataNumber);
                if (separatedAutomataNumber <= lastSeparatedAutomataNumber)
                {
                    Console.WriteLine("Crash!");
                    break;
                }

                var prefix = "0";
                var hardAutomata = automatas.ElementAt(separatedAutomataNumber);
                var goodPrefixes = new List<string>();

                var maxSeparated = 0;
                var maxprefix = "";
                var iter = 0;
                while (maxSeparated <= separatedAutomataNumber)
                {
                    Console.WriteLine(iter);
                    while (goodPrefixes.Count < 20)
                    {
                        if (
                            !hardAutomata.Separates(prefix + pair.Item1 + prefix.GetReverse(),
                                prefix + pair.Item2 + prefix.GetReverse()))
                            goodPrefixes.Add(prefix);
                        do
                        {
                            prefix = prefix.Select(x => x - '0').ToArray().PlusOneIn2ss(prefix.Length - 1).GetString();
                        } while (!prefix.HasEqualCountOf0And1());
                    }


                    foreach (var goodPrefix in goodPrefixes)
                    {
                        var separated = SeparatedAutomataCount(automatas,
                            new Tuple<string, string>(goodPrefix + pair.Item1 + goodPrefix.GetReverse(),
                                goodPrefix + pair.Item2 + goodPrefix.GetReverse()));
                        if (separated > maxSeparated)
                        {
                            maxSeparated = separated;
                            maxprefix = goodPrefix;
                        }
                    }
                    iter++;
                }


                pair = new Tuple<string, string>(maxprefix + pair.Item1 + maxprefix.GetReverse(),
                    maxprefix + pair.Item2 + maxprefix.GetReverse());
                separatedAutomataNumber = maxSeparated;
                lastSeparatedAutomataNumber = separatedAutomataNumber;
            }
            
            var streamWriter = new StreamWriter(File.OpenWrite("totalEqualities5_1.txt")){AutoFlush = true};
            streamWriter.WriteLine(pair.ToStr());
            streamWriter.Close();
            Console.WriteLine("end");
            Console.ReadKey();
        }

        public static void AddPref4()
        {
            var sets = AutomataFactory.GetAllSetsOfInt(4);
            var automatas = AutomataFactory.GetAllConnectedAutomatasWithKnownSets(4, sets).ToList();
            var pair = File.ReadAllLines("eq4.txt")[0].GetPair();
            var prefix = "11000000";
//            var prefix = "000";
            var eq = false;
            var suffix = "";

            while (prefix.Length <= 11)
            {
                suffix = "000";              
                while (suffix.Length <= 11)
                {

                    var separated = false;
                    Console.WriteLine("{0} {1}", prefix, suffix);
                    foreach (var automata in automatas)
                    {
                        if (automata.Separates(prefix + pair.Item1 + suffix,
                            prefix + pair.Item2 + suffix))
                        {
                            separated = true;
                            break;
                        }
                    }

                    if (!separated)
                    {
                        eq = true;
                        break;
                    }
                    suffix = suffix.BinaryPlusOne();
                }

                if (eq)
                {
                    Console.WriteLine("Success!!!");
                    break;
                }
                prefix = prefix.BinaryPlusOne();
            }

            Console.WriteLine("end");
            pair = new Tuple<string, string>(prefix + pair.Item1 + suffix,
                        prefix + pair.Item2 + suffix);
            File.WriteAllLines("totalEqualities4_3.txt", new []{pair.ToStr()});
            Console.ReadKey();
        }

        public static int SeparatedAutomataCount(List<Automata> automatas, Tuple<string, string> pair )
        {
            var separated = false;
            var automataCount = 0;
            foreach (var automata in automatas)
            {
                if (automata.Separates(pair))
                {
                    separated = true;
                    break;
                }
                automataCount++;
            }

            if (!separated)
            {
                Console.WriteLine("Success!!");
            }

            return automataCount;
        }

        public static void WriteAutomata()
        {
            var automatas = AutomataFactory.GetAllAutomatas(5);
            var a = automatas.ElementAt(1147295);
            Console.WriteLine(a.ToString());
            Console.WriteLine();

            var pair =
                new Tuple<string, string>(
                    "0011001110010101010101010101011010101010101010101001010101101010100111001100",
                    "0011001110010101011010101001010101010101010101101010101010101010100111001100");

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
            const int automatasNumber = 20;
            const int stringLength = 52;
            const int stretch = 3;
            const int statesCount = 5;
            var allAutomatas = AutomataFactory.GetAllNonIsomorphicPermutationAutomatas5();

            while (!equalities.Any())
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
                    }
                }

                foreach (var pair in hardlySeprated)
                {
                    var separated = false;
                    foreach (var automata in allAutomatas)
                    {
                        if (automata.Separates(pair))
                        {
                            separated = true;
                            break;
                        }
                    }

                    if(!separated)
                        equalities.Add(pair);
                }

                logWriter.WriteLine("Hardly: {0};  Equalities: {1}", hardlySeprated.Count, equalities.Count);
                Console.WriteLine(hardlySeprated.Count);
            }

            logWriter.Close();
            File.WriteAllLines(filename, equalities.Select(x => x.ToStr()));
        }

        public void EqualitiesForPermutationAutomatas()
        {
            var statesCount = 4;
            var automatas = AutomataFactory.GetAllPermutationAutomata(4);
            var str = "0";
        }

        private static Stack<string> pathStack;
        private static Dictionary<string, List<string>> edges;
        private static Dictionary<string, bool> used;
    }
}