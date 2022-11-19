using AutoComplete.Builders;
using AutoComplete.Clients.IndexSearchers;
using AutoComplete.Domain;
using BenchmarkDotNet.Attributes;

namespace Samples.ConsoleApp;

internal class Sample
{
    private readonly string _headerFileName;
    private readonly string _indexFileName;
    private readonly string _tailFileName;
    private InMemoryIndexSearcher _searcher;

    public Sample(string headerFileName, string indexFileName, string tailFileName)
    {
        _headerFileName = headerFileName;
        _indexFileName = indexFileName;
        _tailFileName = tailFileName;
    }

    [Benchmark]
    public SearchResult Search(string searchTerm, int itemsCount, bool suggest)
    {
        var searchOptions = new SearchOptions()
            {Term = searchTerm, MaxItemCount = itemsCount, SuggestWhenFoundStartsWith = suggest};

        return _searcher.Search(searchOptions);
    }

    public async Task Build()
    {
        if (File.Exists(_headerFileName))
            File.Delete(_headerFileName);
        if (File.Exists(_indexFileName))
            File.Delete(_indexFileName);
        if (File.Exists(_tailFileName))
            File.Delete(_tailFileName);

        await using var headerStream = File.OpenWrite(_headerFileName);
        await using var indexStream = File.OpenWrite(_indexFileName);
        await using var tailStream = File.OpenWrite(_tailFileName);

        var builder = new IndexBuilder(headerStream, indexStream, tailStream);
        foreach (var line in await File.ReadAllLinesAsync("words350k.txt"))
        {
            if (!string.IsNullOrWhiteSpace(line))
                builder.Add(line);
        }

        builder.Build();

        headerStream.Close();
        indexStream.Close();
        tailStream.Close();

        _searcher = new InMemoryIndexSearcher(_headerFileName, _indexFileName, _tailFileName);
        _searcher.Init();
    }
}