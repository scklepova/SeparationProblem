using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SeparationProblem
{
    public static class StringPairFactory
    {
        public static Tuple<string, string> GetRandomPairOfStrings(int length)
        {
            return new Tuple<string, string>(RandomFactory.GetRandomString(length), RandomFactory.GetRandomString(length));
        }

        public static List<Tuple<string, string>> GetPairOfEquivalentStrings(int length, int stretch)
        {
            var eqString = new List<string>();
            string randomStr = null;
            while(eqString.Count == 0)
            {
                randomStr = RandomFactory.GetRandomString(length);
                var graphRauzy = new RauzyGraph(randomStr, stretch);
                eqString = graphRauzy.GetAllEquivalentStrings(); 
//                Console.WriteLine("------------------------------");
            }
            return eqString.Select(x => new Tuple<string, string>(randomStr, x)).ToList();
        }

        public static List<Tuple<string, string>> GetPairOfEquivalentStringsWithEqual01(string randomStr, int stretch)
        {
            var eqString = new List<string>();
           
            var graphRauzy = new RauzyGraph(randomStr, stretch);
            eqString = graphRauzy.GetEquivalentStringsBySwappingCycles();
            //                Console.WriteLine("------------------------------");
            
            return eqString.Select(x => new Tuple<string, string>(randomStr, x)).ToList();
        }
    }
}