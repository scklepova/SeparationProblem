using System;

namespace SeparationProblem
{
    public static class StringPairFactory
    {
        public static Tuple<string, string> GetRandomPairOfStrings(int length)
        {
            return new Tuple<string, string>(RandomFactory.GetRandomString(length), RandomFactory.GetRandomString(length));
        }
    }
}