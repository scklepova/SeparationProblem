using System;
using System.Collections.Generic;

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

        public int LastState(string word)
        {
            var currentState = initState;
            foreach (var c in word)
            {
                currentState = Transite(currentState, c);
                if (StateNotValid(currentState))
                    throw new Exception(string.Format("Invalid state {0}", currentState));
            }
            return currentState;
        }

        private int Transite(int state, char symbol)
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