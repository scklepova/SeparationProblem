namespace SeparationProblem
{
    public static class PermutationAutomataFactory
    {
        public static Automata GetRandomPermutationAutomata(int n)
        {
            var transitions = new[] {RandomFactory.GetRandomPermutation(n), RandomFactory.GetRandomPermutation(n)};
            return new Automata(n, RandomFactory.GetNext(n - 1), transitions);
        }
    }
}