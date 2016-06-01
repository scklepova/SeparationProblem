using System;
using System.Collections.Generic;
using System.Linq;

namespace SeparationProblem
{
    public class RauzyGraph
    {
        public RauzyGraph(string word, int stretch)
        {
            edges = new Dictionary<string, List<string>>();
            var prevSubstr = word.Substring(0, stretch);
            defaultPath = new List<string> {prevSubstr};
            vertexCount = new Dictionary<string, int> {{prevSubstr, 1}};
            for(var i = 1; i < word.Length - stretch + 1; i++)
            {
                var substr = word.Substring(i, stretch);
                if(!edges.ContainsKey(prevSubstr))
                    edges.Add(prevSubstr, new List<string>());
                if(!edges[prevSubstr].Contains(substr))
                    edges[prevSubstr].Add(substr);

                defaultPath.Add(substr);
                if(!vertexCount.ContainsKey(substr))
                    vertexCount.Add(substr, 0);
                vertexCount[substr]++;
                prevSubstr = substr;
            }

            if(!edges.ContainsKey(prevSubstr))
                edges.Add(prevSubstr, new List<string>());

            foreach(var vertex1 in edges.Keys)
            {
                foreach(var vertex2 in edges.Keys)
                {
                    if(vertex1.Substring(1) == vertex2.Substring(0, vertex2.Length - 1) && !edges[vertex1].Contains(vertex2))
                        edges[vertex1].Add(vertex2);
                }
            }

            givenString = word;
        }

        public string GetEquivalentString()
        {
            var intervals = defaultPath;
//            var intervals = SwapAllPossibleCycles(defaultPath);


//            var intervals = new List<string>();
//            FindAllPaths();
//            
            foreach(var initVertex in edges.Keys.Where(x => x != defaultPath[0]))
            {
                currentPath = new Stack<string>();
                if(TryFindPathOfLength(defaultPath.Count, initVertex))
                {
                    Console.WriteLine("true");
                    intervals = currentPath.ToList();
                    break;
                }
            }      
     

            if(intervals == null || intervals.Count != defaultPath.Count)
                return null;

            /**
             */

//            if(intervals.Count(x => x == intervals[0]) >= 2)
//            {
//                var s = intervals[0].StartsWith("0") ? "1" + intervals[0].Substring(1) : "0" + intervals[0].Substring(1);
//                if(intervals.Contains(s))
//                    intervals[0] = s;
//            }
//             
//            if (intervals.Count(x => x == intervals.Last()) >= 2)
//            {
//                var s = intervals.Last().Substring(0, intervals[0].Length - 1);
//                s = intervals.Last().EndsWith("0") ? s + "1" : s + "0";
//                if(intervals.Contains(s))
//                    intervals[intervals.Count - 1] = s;
//            }

            /**
             */

            //запись пути
//            var ans = intervals[0];
//            for(var i = 1; i < intervals.Count; i++)
//                ans += intervals[i].Last();

            var ans = intervals[defaultPath.Count - 1];
            for (var i = defaultPath.Count - 2; i >= 0; i--)
                ans += intervals[i].Last();

            return ans;
        }

        //На самом деле не все, а только по одному из каждой вершины, потому что дфс возвращает первый попавшийся
        public List<string> GetAllEquivalentStrings()
        {
            var eqStrs = new List<string>();
           
            foreach (var initVertex in edges.Keys)
            {
                currentPath = new Stack<string>();
                if (TryFindPathOfLength(defaultPath.Count, initVertex))
                {
//                    Console.WriteLine("true");
                    var eqStr = GetStrFromStack(currentPath.ToList());
                    if(!eqStrs.Contains(eqStr) && givenString != eqStr)
                        eqStrs.Add(eqStr);
                }
            }

            return eqStrs;
        }

        public List<string> GetEquivalentStringsBySwappingCycles()
        {
            var intervals = SwapAllPossibleCycles(defaultPath);
            var strs = intervals.Select(GetStrFromStack).ToList();
            return new List<string>(strs);
        } 

        private string GetStrFromStack(List<string> stack)
        {
            var ans = stack[stack.Count - 1];
            for (var i = stack.Count - 2; i >= 0; i--)
                ans += stack[i].Last();

            return ans;
        }

        private string GetStrFromList(List<string> list)
        {
            var ans = list[0];
            for (var i = 1; i < list.Count; i++)
                ans += list[i].Last();

            return ans;
        }

        private bool TryFindPathOfLength(int length, string initV)
        {
            if(length == 0)
            {
//                return edges.Keys.All(x => currentPath.Contains(x));
                return true;
            }

//            if(length <= preLength)
//            {
//                foreach(var path in allPaths[initV][length])
//                {
//                    var newPath = new List<string>(currentPath);
//                    newPath.Add(initV);
//                    newPath.AddRange(path);
//
//                    if(edges.Keys.All(x => newPath.Contains(x)))
//                    {
//                        currentPath1 = newPath;
//                        return true;
//                    }
//
//                    return false;
//                }
//            }

            if (vertexCount[initV] == 0)
                return false;
            currentPath.Push(initV);
            vertexCount[initV]--;
            if (edges[initV].Any(v => TryFindPathOfLength(length - 1, v)))
                return true;
            currentPath.Pop();
            vertexCount[initV]++;
            return false;
        }

        private void TryFindAllPathOfLength(int length, string initV)
        {
            if (length == 0)
            {
                return;
            }

            if (vertexCount[initV] == 0)
                return;
            currentPath.Push(initV);
            vertexCount[initV]--;
            foreach (var v in edges[initV])
            {
                TryFindAllPathOfLength(length - 1, v);
            }
            
            currentPath.Pop();
            vertexCount[initV]++;
            return;
        }

        private bool TryFindPathOfLength_RandomVersion(int length, string initV)
        {
            if (length == 0)
            {
                return true;
            }

            if (vertexCount[initV] == 0)
                return false;
            currentPath.Push(initV);
            vertexCount[initV]--;

            var possibleWays = edges[initV];
            bool f = false;

            if (possibleWays.Count == 1)
                f = TryFindPathOfLength_RandomVersion(length - 1, possibleWays[0]);
            else
            {
                var rnd = RandomFactory.GetNext(possibleWays.Count);
                if (rnd == 1)
                {
                    f = TryFindPathOfLength_RandomVersion(length - 1, possibleWays[1]);
                    if(!f)
                        f = TryFindPathOfLength_RandomVersion(length - 1, possibleWays[0]);
                }
                else
                    f = possibleWays.Any(v => TryFindPathOfLength_RandomVersion(length - 1, v));
            }

            if (f)
                return true;

            currentPath.Pop();
            vertexCount[initV]++;
            return false;
        }

        private void FindAllPaths()
        {
            allPaths = new Dictionary<string, List<List<string>>[]>();
            foreach(var v in edges.Keys)
            {
                allPaths.Add(v, new List<List<string>>[10]);
                allPaths[v][1] = new List<List<string>>(edges[v].Select(x => new List<string>{x}).ToList());
            }

            for(var i = 2; i <= preLength; i++)
            {
                foreach(var v in edges.Keys)
                {
                    allPaths[v][i] = new List<List<string>>();
                    foreach(var path in allPaths[v][i - 1])
                    {
                        foreach(var u in edges[path.Last()])
                        {
                            allPaths[v][i].Add(path);
                            allPaths[v][i].Last().Add(u);
                        }
                    }
                }
            }
        }

        private int HasEulerPath()
        {
            var inDegree = edges.Keys.ToDictionary(vertex => vertex, vertex => 0);

            foreach(var vertex in edges.Keys)
            {
                foreach(var u in edges[vertex])
                {
                    if(!inDegree.ContainsKey(u))
                        inDegree.Add(u, 0);
                    inDegree[u]++;
                }
            }

            var counter = 0;
            foreach(var vertex in edges.Keys)
            {
                if(inDegree[vertex] != edges[vertex].Count)
                    counter++;
            }

            return counter;
        }

        private List<List<string>> SwapAllPossibleCycles(List<string> path)
        {
            var allSwapped = new List<List<string>>();
            var used = new Dictionary<string, bool>();
            foreach(var vertex in path)
            {
                if(!used.ContainsKey(vertex) || used[vertex] == false)
                    if(path.Count(x => x == vertex) >= 3)
                    {
                        allSwapped.Add(SwapCycleStartsAt(path, vertex));
                    }
                if(!used.ContainsKey(vertex))
                    used.Add(vertex, true);
            }

            return allSwapped;
        }

        private List<string> SwapCycleStartsAt(List<string> path, string cycleVertex)
        {
            var i = 0;
            var ans = new List<string>();
            while (path[i] != cycleVertex)
            {
                ans.Add(path[i]);
                i++;
            }
            var cycleBody1 = new List<string>();
            i++;
            while (path[i] != cycleVertex)
            {
                cycleBody1.Add(path[i]);
                i++;
            }
            var cycleBody2 = new List<string>();
            i++;
            while (path[i] != cycleVertex)
            {
                cycleBody2.Add(path[i]);
                i++;
            }

            ans.Add(cycleVertex);
            ans.AddRange(cycleBody2);
            ans.Add(cycleVertex);
            ans.AddRange(cycleBody1);

            while (i < path.Count)
            {
                ans.Add(path[i]);
                i++;
            }

            return ans;
        }

        private void AddInterval(List<string> fullPath, List<string> interval)
        {
            fullPath.AddRange(Enumerable.Range(1, interval.Count - 1).Select(i => interval[i]));
        }

        private bool IsCycle(List<string> path)
        {
            return path.First() == path.Last();
        }

        private readonly Dictionary<string, List<string>> edges;
        private readonly List<string> defaultPath;
        private Stack<string> currentPath;
        private List<string> currentPath1;
        private Dictionary<string, List<List<string>>[]> allPaths;
        private int preLength = 7;
        private Dictionary<string, int> vertexCount;
        private Dictionary<string, int> vertexCountCopy;
        private string givenString;
        private List<List<string>> paths;
    }
}