using System.Collections.Generic;
using NUnit.Framework;

namespace SeparationProblem.Tests
{
    public class CyclesTest
    {
        [Test]
        public void TestGraphRauzy()
        {
            var s = "";
        }

        [Test]
        public void TestAllWordsOfLen()
        {
            var words = WordsFactory.GetAllWordsOfLength(3);
            var expected = new List<string>() {"000", "001", "010", "011", "100", "101", "110", "111"};
            Assert.AreEqual(expected, words);
        }
    }
}