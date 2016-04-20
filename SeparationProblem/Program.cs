using System;
using System.IO;

namespace SeparationProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = new FileInfo("separation.txt");
            file.Delete();

            var streamWriter = file.AppendText();

            const int numberOfStates = 5;
            const int stringLength = 40;
            const int experimentsNumder = 100000;

            for(var i = 0; i < experimentsNumder; i++)
            {
                var pairOfStrings = StringPairFactory.GetPairOfEquivalentStrings(stringLength, numberOfStates - 1);
                var wasSeparated = false;
                
                var iterations = 0;
                while(!wasSeparated && iterations < 10)
                {
                    var automata = PermutationAutomataFactory.GetRandomPermutationAutomata(numberOfStates);
                    if(automata.LastState(pairOfStrings.Item1) != automata.LastState(pairOfStrings.Item2))
                    {
                        wasSeparated = true;
                    }
                    iterations++;
                }

                if(!wasSeparated)
                {
                    streamWriter.WriteLine("{0} {1}", pairOfStrings.Item1, pairOfStrings.Item2);
                }

                Console.WriteLine(i);
            }

            streamWriter.Close();
        }
    }
}