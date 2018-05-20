using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SeparationProblem.Tests
{
    public class AutomataTest
    {
        [Test]
        public void TestTransitions()
        {
            var automata = new Automata(2, 0, new[] {new[] {0, 1}, new[] {1, 0}});
            Assert.AreEqual(0, automata.Transite(0, '0'));
            Assert.AreEqual(1, automata.Transite(0, '1'));
            Assert.AreEqual(1, automata.Transite(1, '0'));
            Assert.AreEqual(0, automata.Transite(1, '1'));
        }

        [Test]
        public void TestSeparation()
        {
            var automatas = AutomataFactory.GetAllAutomatas(2);
            var equality = new Tuple<string, string>("1000", "0010");
            var sep = new Tuple<string, string>("1000", "1001");
            Assert.True(automatas.Any(x => x.Separates(sep)));
            Assert.True(!automatas.Any(x => x.Separates(equality)));
        }

        [Test]
        public void TestGetAllPermutationAutomatas()
        {
            var automatas = AutomataFactory.GetAllPermutationAutomata(6);
            Assert.AreEqual(6*720*720, automatas.Count());
        }

        [Test]
        public void TestIdentity()
        {
            var automatas = AutomataFactory.GetReducedPermutationAutomata(8);
            var pair = GetPair(4, 60, 56);
            foreach (var automata in automatas)
            {
                if (automata.Separates(pair))
                {
                    Assert.Fail();
                }
            }
            Console.WriteLine("Success");
        }

        [Test]
        public static void BruteForce3blockIdentities()
        {
            const int k = 6;
            var automatas = AutomataFactory.GetReducedPermutationAutomata(k).ToList();
            var automatas5 = AutomataFactory.GetAllNonIsomorphicPermutationAutomatas5().ToList();
            var hardPairs = new List<Tuple<int, int, int>>();

            const int lcm = 60;
            var triplets = new List<Tuple<int, int, int>>();
            for (var a = 1; a < lcm; a++)
            for (var b = 1; b < lcm; b++)
            for (var c = a; c < lcm; c++)
                triplets.Add(new Tuple<int, int, int>(a, b, c));

            foreach (var triplet in triplets)
            {
                var pair = GetPair(triplet.Item1, triplet.Item2, triplet.Item3);
                var separated = false;
                var separatedBy5 = false;

                foreach (var automata in automatas5)
                {
                    if (automata.Separates(pair))
                    {
                        separatedBy5 = true;
                        break;
                    }
                }

                if (separatedBy5) continue;

                foreach (var automata in automatas)
                {
                    if (automata.Separates(pair))
                    {
                        separated = true;
                        break;
                    }
                }
                if (!separated)
                {
                    hardPairs.Add(triplet);
                    Console.WriteLine($"a={triplet.Item1} b={triplet.Item2} c={triplet.Item3} sum={triplet.Item1 + triplet.Item2 + triplet.Item3} a-b+c==0 - {(triplet.Item1 - triplet.Item2 + triplet.Item3) % k == 0}");
                }
            }

            File.WriteAllLines("C:/SeparationProblem/3blocks_identities_S_6.txt", hardPairs.Select(t =>
                $"a={t.Item1} b={t.Item2} c={t.Item3} sum={t.Item1 + t.Item2 + t.Item3} a-b+c==0 - {(t.Item1 - t.Item2 + t.Item3) % k == 0}"));
            Console.WriteLine("The end");
        }

        private static Tuple<string, string> GetPair(int a, int b, int c)
        {
            var word1 = string.Concat(Enumerable.Repeat("10", a)) + string.Concat(Enumerable.Repeat("01", b)) + string.Concat(Enumerable.Repeat("10", c));
            var word2 = string.Concat(Enumerable.Repeat("01", c)) + string.Concat(Enumerable.Repeat("10", b)) + string.Concat(Enumerable.Repeat("01", a));
            return new Tuple<string, string>(word1, word2);
        }

    }
}