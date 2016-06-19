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
            
            return eqString.Where(x => x != randomStr).Select(x => new Tuple<string, string>(randomStr, x)).ToList();
        }

//        public static Tuple<string, string> GetPairOfEquivalentStrings_OldVersion(int length, int stretch)
//        {
//            string eqString = null;
//            string randomStr = null;
//            while (eqString == null || eqString == randomStr)
//            {
//                randomStr = RandomFactory.GetRandomString(length);
//                var graphRauzy = new RauzyGraph(randomStr, stretch);
//                eqString = graphRauzy.GetEquivalentString_OldVersion();
//            }
//            return new Tuple<string, string>(randomStr, eqString);
//        }

        public static Tuple<string, string> GetPairOfEquivalentStringsDifferentAtTheEdges(int stringLength, int stretch)
        {
            string eqString = null;
            string randomStr = null;
            while (eqString == null || eqString == randomStr)
            {
                randomStr = RandomFactory.GetRandomString(stringLength);
                var graphRauzy = new RauzyGraph(randomStr, stretch);
                eqString = graphRauzy.GetEquivalentStringWithDiffAtTheEdges();
            }
            return new Tuple<string, string>(randomStr, eqString);
        }
    }
}