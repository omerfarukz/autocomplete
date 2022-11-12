using AutoComplete.Builders;
using AutoComplete.Clients.IndexSearchers;
using AutoComplete.Domain;

const string headerFileName = "header.bin";
const string indexFileName = "index.bin";
const string tailFileName = "tail.txt";

await BuildIndex();
Search();

void Search()
{
    var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, tailFileName);
    searcher.Init();
    while (true)
    {
        Console.WriteLine("Type a word");
        var word = Console.ReadLine();
        var results = searcher.Search(new SearchOptions() { Term = word, MaxItemCount = 5, SuggestWhenFoundStartsWith = false});
        if (results.Items == null)
            continue;
        
        foreach (var item in results.Items)
        {
            Console.WriteLine(item);
        }
    }
}

async Task BuildIndex()
{
    if (File.Exists(headerFileName))
        File.Delete(headerFileName);
    if (File.Exists(indexFileName))
        File.Delete(indexFileName);
    if (File.Exists(tailFileName))
        File.Delete(tailFileName);

    await using var headerStream = File.OpenWrite(headerFileName);
    await using var indexStream = File.OpenWrite(indexFileName);
    await using var tailStream = File.OpenWrite(tailFileName);

    using var builder = new IndexBuilder(headerStream, indexStream, tailStream);
    foreach (var line in await File.ReadAllLinesAsync("words350k.txt"))
    {
        if(!string.IsNullOrWhiteSpace(line))
            builder.Add(line);
    }

    builder.Build();
    
    headerStream.Close();
    indexStream.Close();
    tailStream.Close();
}
