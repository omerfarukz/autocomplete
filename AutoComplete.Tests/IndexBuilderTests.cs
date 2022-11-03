using System.IO;
using AutoComplete.Builders;
using Xunit;

namespace AutoComplete.Tests;

public class IndexBuilderTests
{
    [Fact]
    public void add_same_word_should_be_considered_as_single()
    {
        using var headerStream = new MemoryStream();
        using var indexStream = new MemoryStream();
        var builder = new IndexBuilder(headerStream, indexStream);
        builder.Add("John");
        builder.Add("John");
        var processedNodeCount = builder.Build();
        Assert.Equal(5, processedNodeCount);
    }
    
    [Fact]
    public void build_with_tail_should_pass()
    {
        using var headerStream = new MemoryStream();
        using var indexStream = new MemoryStream();
        using var tailStream = new MemoryStream();
        var builder = new IndexBuilder(headerStream, indexStream, tailStream);
        builder.Add("John");
        var processedNodeCount = builder.Build();
        Assert.Equal(5, processedNodeCount);
    }
    
    [Fact]
    public void add_single_word_and_check_node_count()
    {
        using var headerStream = new MemoryStream();
        using var indexStream = new MemoryStream();
        var builder = new IndexBuilder(headerStream, indexStream);
        builder.Add("John");

        var processedNodeCount = builder.Build();

        Assert.Equal(5, processedNodeCount);
    }

    [Fact]
    public void add_three_words_and_check_node_count()
    {
        using var headerStream = new MemoryStream();
        using var indexStream = new MemoryStream();
        var builder = new IndexBuilder(headerStream, indexStream);
        builder.Add("John");
        builder.Add("Johnny");
        builder.Add("Jane");

        /*
         *
         *          R(1)*
         *          J(2)
         *      A(3)    O(4)
         *      N(5)    H(6)
         *     *E(7)    N(8)*
         *              N(9)
         *              Y(10)*
         */

        var processedNodeCount = builder.Build();
        Assert.Equal(10, processedNodeCount);
    }
}