using NUnit.Framework;

namespace SeparationProblem.Tests
{
    public class RandomsTest
    {
        [Test]
        public void TestRandomArray()
        {
            var arr1 = RandomFactory.GetRandomArray(40);
            var arr2 = RandomFactory.GetRandomArray(40);
            Assert.AreNotEqual(arr1, arr2);
        }

        [Test]
        public void TestRandomPermutation()
        {
            var p1 = RandomFactory.GetRandomPermutation(40);
            var p2 = RandomFactory.GetRandomPermutation(40);
            Assert.AreNotEqual(p1, p2);
        }

        [Test]
        public void TestRandonString()
        {
            var s1 = RandomFactory.GetRandomString(40);
            var s2 = RandomFactory.GetRandomString(40);
            Assert.AreNotEqual(s1, s2);
        }
    }
}