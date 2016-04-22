using System;

namespace SeparationProblem
{
    public static class StringPairFactory
    {
        public static Tuple<string, string> GetRandomPairOfStrings(int length)
        {
            return new Tuple<string, string>(RandomFactory.GetRandomString(length), RandomFactory.GetRandomString(length));
        }

        public static Tuple<string, string> GetPairOfEquivalentStrings(int length, int stretch)
        {
            string eqString = null;
            string randomStr = null;
            while(eqString == null || eqString == randomStr)
            {
                randomStr = RandomFactory.GetRandomString(length);
                var graphRauzy = new RauzyGraph(randomStr, stretch);
                eqString = graphRauzy.GetEquivalentString(); 
                Console.WriteLine("------------------------------");
            }
            return new Tuple<string, string>(randomStr, eqString);
        }
    }
}