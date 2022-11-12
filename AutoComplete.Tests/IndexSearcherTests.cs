using System.IO;
using AutoComplete.Builders;
using AutoComplete.Clients.IndexSearchers;
using AutoComplete.DataStructure;
using AutoComplete.Domain;
using Xunit;

namespace AutoComplete.Tests;

public class IndexSearcherTests
{
    private readonly string headerFileName;
    private readonly string indexFileName;
    private readonly string tailFileName;

    public IndexSearcherTests()
    {
        headerFileName = "header.bin";
        indexFileName = "index.bin";
        tailFileName = "tail.bin";

        if (File.Exists(indexFileName))
            File.Delete(headerFileName);
        if (File.Exists(indexFileName))
            File.Delete(indexFileName);
        if (File.Exists(tailFileName))
            File.Delete(tailFileName);

        var headerStream = File.OpenWrite(headerFileName);
        var indexStream = File.OpenWrite(indexFileName);
        var tailStream = File.OpenWrite(tailFileName);
        var builder = new IndexBuilder(headerStream, indexStream, tailStream);
        builder.Add("John");
        builder.Add("Johnny");
        builder.Add("Jane");
        builder.Add("Smith");

        builder.Build();

        headerStream.Close();
        indexStream.Close();
        tailStream.Close();
    }

    [Fact]
    public void search_on_empty_index_should_return_not_found()
    {
        var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName);
        searcher.Init();

        var result = searcher.Search(new SearchOptions()
            {Term = "notexist", MaxItemCount = 1, SuggestWhenFoundStartsWith = true});

        Assert.Equal(TrieNodeSearchResultType.NotFound, result.ResultType);
    }

    [Fact]
    public void in_memory_search_should_return_found_starts_with()
    {
        var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "Johnson", MaxItemCount = 1, SuggestWhenFoundStartsWith = true});
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(TrieNodeSearchResultType.FoundStartsWith, result.ResultType);
    }

    [Fact]
    public void in_memory_search_should_return_found_equals()
    {
        var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "Jane", MaxItemCount = 1, SuggestWhenFoundStartsWith = false});
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(TrieNodeSearchResultType.FoundEquals, result.ResultType);
    }

    [Fact]
    public void in_search_should_return_not_equals()
    {
        var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "None", MaxItemCount = 1, SuggestWhenFoundStartsWith = false});
        Assert.NotNull(result);
        Assert.Null(result.Items);
        Assert.Equal(TrieNodeSearchResultType.NotFound, result.ResultType);
    }

    [Fact]
    public void in_search_should_return_completions()
    {
        var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "J", MaxItemCount = 3, SuggestWhenFoundStartsWith = true});
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Equal(3, result.Items.Length);
        Assert.Equal(TrieNodeSearchResultType.FoundEquals, result.ResultType);
    }

    [Fact]
    public void in_search_should_return_not_enough_completions()
    {
        var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "J", MaxItemCount = 10, SuggestWhenFoundStartsWith = true});
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Equal(3, result.Items.Length);
        Assert.Equal(TrieNodeSearchResultType.FoundEquals, result.ResultType);
    }

    [Fact]
    public void in_search_with_tail_should_return_completions()
    {
        var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, tailFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "Jo", MaxItemCount = 3, SuggestWhenFoundStartsWith = true});
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Equal(2, result.Items.Length);
        Assert.Equal(TrieNodeSearchResultType.FoundEquals, result.ResultType);
    }

    [Fact]
    public void filesystem_memory_search_should_return_found_starts_with()
    {
        var searcher = new FileSystemIndexSearcher(headerFileName, indexFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "Johnson", MaxItemCount = 1, SuggestWhenFoundStartsWith = true});
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(TrieNodeSearchResultType.FoundStartsWith, result.ResultType);
    }

    [Fact]
    public void filesystem_memory_search_should_return_found_equals()
    {
        var searcher = new FileSystemIndexSearcher(headerFileName, indexFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "Jane", MaxItemCount = 1, SuggestWhenFoundStartsWith = false});
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(TrieNodeSearchResultType.FoundEquals, result.ResultType);
    }

    [Fact]
    public void file_system_search_should_return_not_equals()
    {
        var searcher = new FileSystemIndexSearcher(headerFileName, indexFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "None", MaxItemCount = 1, SuggestWhenFoundStartsWith = false});
        Assert.NotNull(result);
        Assert.Null(result.Items);
        Assert.Equal(TrieNodeSearchResultType.NotFound, result.ResultType);
    }

    [Fact]
    public void file_system_search_with_tail_should_pass()
    {
        var searcher = new FileSystemIndexSearcher(headerFileName, indexFileName, tailFileName);
        searcher.Init();
        var result = searcher.Search(new SearchOptions()
            {Term = "None", MaxItemCount = 1, SuggestWhenFoundStartsWith = false});
        Assert.NotNull(result);
        Assert.Null(result.Items);
        Assert.Equal(TrieNodeSearchResultType.NotFound, result.ResultType);
    }
}