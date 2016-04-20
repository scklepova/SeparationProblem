namespace SeparationProblem
{
    public static class PermutationAutomataFactory
    {
        public static Automata GetRandomPermutationAutomata(int n)
        {
            while(true)
            {
                var transitions = new[] {RandomFactory.GetRandomPermutation(n), RandomFactory.GetRandomPermutation(n)};
                var automata = new Automata(n, RandomFactory.GetNext(n - 1), transitions);
                if(automata.IsConnected())
                    return automata;
            }
        }
    }
}