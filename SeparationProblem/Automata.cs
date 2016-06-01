using System;
using System.Collections.Generic;
using System.Linq;

namespace SeparationProblem
{
    public class Automata
    {
        public Automata(int n, int initState, int[][] transitions)
        {
            this.n = n;
            this.initState = initState;
            this.transitions = transitions;
        }

        public Automata(int n, int initState, string cyclePermutation0, string cyclePermutation1)
        {
            this.n = n;
            this.initState = initState;
            transitions = new[]
            {
                GetTransitionsFromCyclePermutation(cyclePermutation0, n),
                GetTransitionsFromCyclePermutation(cyclePermutation1, n)
            };
        }

        private int[] GetTransitionsFromCyclePermutation(string cyclePermutation, int n)
        {
            var trans = new int[n];
            for (int i = 0; i < n; i++)
                trans[i] = i;
            var cycles = cyclePermutation.Split(')').Where(x => x != "").Select(x => x.Substring(1));
            foreach (var cycle in cycles)
            {
                for (var i = 0; i < cycle.Length - 1; i++)
                    trans[cycle[i] - '0' - 1] = cycle[i + 1] - '0' - 1;
                trans[cycle.Last() - '0' - 1] = cycle[0] - '0' - 1;
            }
            return trans;
        }

        public int LastState(string word)
        {
            var currentState = initState;
            foreach (var c in word)
            {
                currentState = Transite(currentState, c);
//                Console.Write(currentState);
                if (StateNotValid(currentState))
                    throw new Exception(string.Format("Invalid state {0}", currentState));
            }
            return currentState;
        }

        public bool Separates(Tuple<string, string> pair)
        {
            return LastState(pair.Item1) != LastState(pair.Item2);
        }

        public bool Separates(string item1, string item2)
        {
            return LastState(item1) != LastState(item2);
        }

        public int Transite(int state, char symbol)
        {
            return transitions[symbol - '0'][state];
        }

        private bool StateNotValid(int state)
        {
            return state < 0 || state >= n;
        }

        public override string ToString()
        {
            var s = "";
            for (var i = 0; i < n; i++)
            {
                s += string.Format("{0}: ({1}, {2});    ", i, Transite(i, '0'), Transite(i, '1'));
            }
            s += string.Format("InitState: {0}", initState);

            return s;
        }

        public bool IsConnected()
        {
            var used = new bool[n];
            var stack = new Stack<int>();
            stack.Push(initState);
            used[initState] = true;
            while(stack.Count > 0)
            {
                var v = stack.Pop();
                var v0 = Transite(v, '0');
                var v1 = Transite(v, '1');
                if(!used[v1])
                    stack.Push(v1);
                if(!used[v0])
                    stack.Push(v0);
                used[v] = true;
            }

            for(var i = 0; i < n; i++)
            {
                if(!used[i])
                    return false;
            }

            return true;
        }

        private readonly int initState;
        private readonly int n;
        private readonly int[][] transitions;
    }
}