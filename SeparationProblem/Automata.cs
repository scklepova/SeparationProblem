using System;

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

        private readonly int initState;
        private readonly int n;
        private readonly int[][] transitions;
    }
}