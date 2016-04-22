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
            for(var i = 1; i < word.Length - stretch + 1; i++)
            {
                var substr = word.Substring(i, stretch);
                if(!edges.ContainsKey(prevSubstr))
                    edges.Add(prevSubstr, new List<string>());
                if(!edges[prevSubstr].Contains(substr))
                    edges[prevSubstr].Add(substr);

                defaultPath.Add(substr);
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
        }

        public string GetEquivalentString()
        {
//            var intervals = SwapCycles(defaultPath);
            var intervals = new List<string>();
            var h = HasEulerPath();
            if(h == 2)
                intervals = TryGetEulerPath(defaultPath[0]);
            else if(h == 0)
            {
                foreach(var initVertex in edges.Keys.Where(x => x != defaultPath[0]))
                {
                    intervals = TryGetEulerPath(initVertex);
                    if(intervals.Count == defaultPath.Count)
                        break;
                }
            }

            if(intervals == null || intervals.Count != defaultPath.Count)
                return null;

            var ans = intervals[0];
            for(var i = 1; i < intervals.Count; i++)
                ans += intervals[i].Last();

            return ans;
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

        public List<string> TryGetEulerPath(string startV)
        {
            var usedEdges = new Dictionary<string, Dictionary<string, bool>>();
            var stack = new Stack<string>();
            var eulerPath = new List<string>();
            stack.Push(startV);
            while(stack.Count > 0)
            {
                var w = stack.Peek();
                foreach(var u in edges[w])
                {
                    if(!usedEdges.ContainsKey(w) || !usedEdges[w].ContainsKey(u) || !usedEdges[w][u])
                    {
                        stack.Push(u);
                        if(!usedEdges.ContainsKey(w))
                            usedEdges.Add(w, new Dictionary<string, bool>());
                        if(!usedEdges[w].ContainsKey(u))
                            usedEdges[w].Add(u, true);
                        usedEdges[w][u] = true;
                        break;
                    }
                }
                if(w == stack.Peek())
                {
                    stack.Pop();
                    eulerPath.Add(w);
                }
            }

            return eulerPath;
        }

        private List<List<string>> GetPathIntervals(List<string> path)
        {
            var lastPos = edges.Keys.ToDictionary(v => v, v => -1);
            var k = 0;
            var lastCycleIndex = 0;
            var intervals = new List<List<string>>();
            while(k < path.Count)
            {
                if(lastPos[path[k]] >= lastCycleIndex)
                {
                    var cycle = new List<string>();
                    for(var i = lastPos[path[k]]; i <= k; i++)
                        cycle.Add(path[i]);

                    if(lastCycleIndex < k - cycle.Count + 1)
                    {
                        var interval = new List<string>();
                        for(var i = lastCycleIndex; i <= k; i++)
                            interval.Add(path[i]);
                        intervals.Add(interval);
                    }
                    intervals.Add(cycle);
                    lastCycleIndex = k;
                }

                lastPos[path[k]] = k;
                k++;
            }

            if(lastCycleIndex < k - 1)
            {
                var interval = new List<string>();
                for(var i = lastCycleIndex; i <= k - 1; i++)
                    interval.Add(path[i]);
                intervals.Add(interval);
            }

            return intervals;
        }

        private List<string> SwapCycles2(List<List<string>> intervals)
        {
            if(intervals.Count == 1)
                return null;

            var ans = new List<string>();
            var j = 0;
            while(j < intervals.Count)
            {
                if(!IsCycle(intervals[j]))
                    AddInterval(ans, intervals[j]);
                else
                {
                    if(j + 1 < intervals.Count && intervals[j].First() == intervals[j + 1].First() && IsCycle(intervals[j + 1]))
                    {
                        AddInterval(ans, intervals[j + 1]);
                        AddInterval(ans, intervals[j]);
                        j++;
                    }
                    else
                        AddInterval(ans, intervals[j]);
                }
                j++;
            }

            return ans;
        }

        private List<string> SwapCycles(List<string> path)
        {
            var cycleVertex = "";
            foreach(var vertex in path)
            {
                if(path.Count(x => x == vertex) >= 3)
                {
                    cycleVertex = vertex;
                    break;
                }
            }

            if(cycleVertex == "")
                return null;

            var i = 0;
            var ans = new List<string>();
            while(path[i] != cycleVertex)
            {
                ans.Add(path[i]);
                i++;
            }
            var cycleBody1 = new List<string>();
            i++;
            while(path[i] != cycleVertex)
            {
                cycleBody1.Add(path[i]);
                i++;
            }
            var cycleBody2 = new List<string>();
            i++;
            while(path[i] != cycleVertex)
            {
                cycleBody2.Add(path[i]);
                i++;
            }

            ans.Add(cycleVertex);
            ans.AddRange(cycleBody2);
            ans.Add(cycleVertex);
            ans.AddRange(cycleBody1);

            while(i < path.Count)
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

        private List<string> FindEulerPath(string initVertex)
        {
            var graph = new Dictionary<string, List<string>>(edges);
            var stack = new Stack<string>();
            var ans = new List<string>();
            stack.Push(initVertex);
            while(stack.Count > 0)
            {
                var v = stack.Peek();
                if(graph[v].Count == 0)
                {
                    ans.Add(v);
                    stack.Pop();
                }
                else
                {
                    stack.Push(graph[v][0]);
                    graph[v].RemoveAt(0);
                }
            }

            return ans;
        }

        private readonly Dictionary<string, List<string>> edges;
        private readonly List<string> defaultPath;
    }
}