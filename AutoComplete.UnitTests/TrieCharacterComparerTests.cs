using AutoComplete.Core.DataStructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoComplete.UnitTests
{
    [TestClass]
    public class TrieCharacterComparerTests
    {
        [TestMethod]
        public void compare_a_and_a_sould_be_zero()
        {
            var comparer = new TrieCharacterComparer();
            var result = comparer.Compare('a', 'a');

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void compare_a_and_b_sould_be_minus_1()
        {
            var comparer = new TrieCharacterComparer();
            var result = comparer.Compare('a', 'b');

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void compare_questionmark_and_a_sould_less_than_zero()
        {
            var comparer = new TrieCharacterComparer();
            var result = comparer.Compare('?', 'a');

            Assert.IsTrue(0 > result);
        }

        [TestMethod]
        public void compare_uppercase_A_and__lowercase_a_sould_less_than_zero()
        {
            var comparer = new TrieCharacterComparer();
            var result = comparer.Compare('A', 'a');

            Assert.IsTrue(0 > result);
        }
    }
}