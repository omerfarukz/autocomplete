using AutoComplete.DataStructure;
using Xunit;

namespace AutoComplete.Tests;

public class TrieNodeStructSearchResultTest
{
    [Fact]
    public void create_not_found_result_should_pass()
    {
        var result = TrieNodeStructSearchResult.CreateNotFound();
        Assert.NotNull(result);
        Assert.Equal(TrieNodeSearchResultType.NotFound, result.Status);
    }

    [Fact]
    public void create_found_equals_result_should_pass()
    {
        var result = TrieNodeStructSearchResult.CreateFoundEquals(1, 2);
        Assert.NotNull(result);
        Assert.Equal(1, result.AbsolutePosition);
        Assert.Equal(2, result.LastFoundNodePosition);
        Assert.Equal(TrieNodeSearchResultType.FoundEquals, result.Status);
    }

    [Fact]
    public void create_found_starts_with_result_should_pass()
    {
        var result = TrieNodeStructSearchResult.CreateFoundStartsWith(1, 2, 3);
        Assert.NotNull(result);
        Assert.Equal(1, result.AbsolutePosition);
        Assert.Equal(2, result.LastFoundCharacterIndex);
        Assert.Equal(3, result.LastFoundNodePosition);
        Assert.Equal(TrieNodeSearchResultType.FoundStartsWith, result.Status);
    }
}