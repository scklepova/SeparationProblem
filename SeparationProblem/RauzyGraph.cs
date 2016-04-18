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
            for(var i = 1; i < word.Length - stretch; i++)
            {
                var substr = word.Substring(i, stretch);
                if(!edges.ContainsKey(prevSubstr))
                    edges.Add(prevSubstr, new List<string>());
                edges[prevSubstr].Add(substr);
                if(!edges.ContainsKey(substr))
                    edges.Add(substr, new List<string>());
                edges[substr].Add(prevSubstr);
                prevSubstr = substr;
            }

            this.word = word;
        }

        private bool HasEulerPath()
        {
            var oddVertex = edges.Keys.Count(vertex => edges[vertex].Count % 2 == 1);
            return oddVertex == 0 || oddVertex == 2;
        }

        private List<string> SwapCycles(List<string> path)
        {
            var firstPos = edges.Keys.ToDictionary(v => v, v => -1);
            var k = 1;
            var cycles = new List<List<string>>();
            while(k < path.Count)
            {
                if(firstPos[path[k]] > 0)
                {
                    var cycle = new List<string>();
                    for(var i = firstPos[path[k]]; i <= k; i++)
                        cycle.Add(path[i]);
                    cycles.Add(cycle);
                }   
                firstPos[path[k]] = k;
            }

            if(cycles.Count == 1)
                return null;

            var ans = new List<string>();
            return ans;
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

        private string word;
        private readonly Dictionary<string, List<string>> edges; 
    }
}