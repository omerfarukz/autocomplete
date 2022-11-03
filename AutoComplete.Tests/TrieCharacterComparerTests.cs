using AutoComplete.DataStructure;
using Xunit;

namespace AutoComplete.Tests;

public class TrieCharacterComparerTests
{
    [Fact]
    public void compare_a_and_a_should_be_zero()
    {
        var comparer = new TrieCharacterComparer();
        var result = comparer.Compare('a', 'a');

        Assert.Equal(0, result);
    }

    [Fact]
    public void compare_a_and_b_should_be_minus_1()
    {
        var comparer = new TrieCharacterComparer();
        var result = comparer.Compare('a', 'b');

        Assert.Equal(-1, result);
    }

    [Fact]
    public void compare_question_mark_and_a_should_less_than_zero()
    {
        var comparer = new TrieCharacterComparer();
        var result = comparer.Compare('?', 'a');

        Assert.True(0 > result);
    }

    [Fact]
    public void compare_uppercase_A_and__lowercase_a_should_less_than_zero()
    {
        var comparer = new TrieCharacterComparer();
        var result = comparer.Compare('A', 'a');

        Assert.True(0 > result);
    }
}