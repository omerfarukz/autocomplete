using AutoComplete.Core.DataStructure;
using Xunit;

namespace AutoComplete.UnitTests
{
    public class TrieCharacterComparerTests
    {
        [Fact]
        public void compare_a_and_a_sould_be_zero()
        {
            var comparer = new TrieCharacterComparer();
            var result = comparer.Compare('a', 'a');

            Assert.Equal(0, result);
        }

        [Fact]
        public void compare_a_and_b_sould_be_minus_1()
        {
            var comparer = new TrieCharacterComparer();
            var result = comparer.Compare('a', 'b');

            Assert.Equal(-1, result);
        }

        [Fact]
        public void compare_questionmark_and_a_sould_less_than_zero()
        {
            var comparer = new TrieCharacterComparer();
            var result = comparer.Compare('?', 'a');

            Assert.True(0 > result);
        }

        [Fact]
        public void compare_uppercase_A_and__lowercase_a_sould_less_than_zero()
        {
            var comparer = new TrieCharacterComparer();
            var result = comparer.Compare('A', 'a');

            Assert.True(0 > result);
        }
    }
}