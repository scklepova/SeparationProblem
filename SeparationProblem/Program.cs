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
//            OpenFile();
//            SelectOnlyWordsWithEqualCountOfZeroAnd1();
//            Words2();
//            ConvertStringsToNums();
//            WordsWithoutThreeDigitsInARow2();
//            JoinBackups();
            RandomPermutationAutomatas();
            AllPermutationAutomatas();
//            AllPathsOf();
//            FilterHard();
//            AllPermutationAutomatasWithExactPairs();
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

        public static void AllPermutationAutomatasWithExactPairs()
        {
            var automatas = PermutationAutomataFactory.GetAllNonIsomorphicPermutationAutomatas5();
            var hardPairs = new List<Tuple<string, string>>();
            var lines = File.ReadAllLines("EqualCountnoThreeInARow24.txt");

            foreach (var line0 in lines)
            {
                foreach (var line1 in lines)
                {
                    var line = line0 + line1;                  
                    var pair = new Tuple<string, string>(line, new string(line.Reverse().ToArray()));
                    if(pair.Item1 == pair.Item2)
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
                        hardPairs.Add(pair);
                }
            }

            File.WriteAllLines("hard.txt", hardPairs.Select(pair => string.Format("{0} {1}", pair.Item1, pair.Item2)));
        }

        public static void AllPermutationAutomatas()
        {
            var automatas = PermutationAutomataFactory.GetAllNonIsomorphicPermutationAutomatas5();
            var streamReader = new StreamReader(File.OpenRead("separation.txt"));
            
//            var hardlySeparatedPairs = File.ReadAllLines("separation.txt").Select(x => new Tuple<string, string>(x.Split(' ')[0], x.Split(' ')[1])).ToList();
            var hardPairs = new List<Tuple<string, string>>();

            while(!streamReader.EndOfStream)
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
                    if(!separated)
                        hardPairs.Add(pair);
                }
            }

            File.WriteAllLines("hard.txt", hardPairs.Select(pair => string.Format("{0} {1}", pair.Item1, pair.Item2)));
        }
        
        public static void RandomPermutationAutomatas()
        {
            var lines = File.ReadAllLines("EqualCountnoThreeInARow24.txt");

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
                    var automata = PermutationAutomataFactory.GetRandomPermutationAutomata(numberOfStates);
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

        

        private static string PermutationToString(List<int> permutation)
        {
            return permutation.Aggregate("", (current, i) => current + (i + " "));
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
                File.AppendAllText("paths.txt", string.Format("{0}\r\n", PathToStr(pathStack.ToList())));
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

        private static string PathToStr(List<string> path)
        {
            var ans = path[0];
            for (var i = 1; i < path.Count; i++)
                ans += path[i].Last();

            return ans;
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
                    var num = StrToLong(line);
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
                        var num = StrToLong(str + "0");
                        streamWriter.WriteLine(num);
                    }
                    if (!str.EndsWith("11"))
                    {
                        var num = StrToLong(str + "1");
                        streamWriter.WriteLine(num);
                    }       
                }
            }
        }

        private static long StrToLong(string str)
        {
            var num = 0;
            foreach (var digit in str)
            {
                num = num*2 + (digit - '0');
            }
            return num;
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
            var filename = "noThreeInARow24.txt";
            var streamReader = new StreamReader(File.OpenRead(filename));
            var streamWriter = new StreamWriter(File.OpenWrite("EqualCount" + filename));
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line != null)
                {
                    if (EqualCountOf0And1(line)) 
                        streamWriter.WriteLine(line);
                }
            }
        }

        private static bool EqualCountOf0And1(string line)
        {
            var count0 = line.Count(x => x == '0');
            var count1 = line.Count(x => x == '1');

            return count0 == count1;
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

        private static Stack<string> pathStack;
        private static Dictionary<string, List<string>> edges;
        private static Dictionary<string, bool> used;
    }
}