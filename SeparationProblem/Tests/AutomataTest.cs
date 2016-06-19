using System;
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
    }
}