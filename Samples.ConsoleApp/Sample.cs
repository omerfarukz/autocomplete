using AutoComplete.Builders;
using AutoComplete.Clients.IndexSearchers;
using AutoComplete.Domain;

namespace Samples.ConsoleApp;

internal class Sample
{
    private const string HeaderFileName = "header.bin";
    private const string IndexFileName = "index.bin";
    private const string TailFileName = "tail.txt";
    
    public void Search()
    {
        var searcher = new InMemoryIndexSearcher(HeaderFileName, IndexFileName, TailFileName);
        searcher.Init();
        while (true)
        {
            Console.WriteLine("Type a word. (exit for termination)");
            var word = Console.ReadLine();
            if (word == "exit")
                break;
            
            var results = searcher.Search(new SearchOptions() { Term = word, MaxItemCount = 5, SuggestWhenFoundStartsWith = false});
            if (results.Items == null)
                continue;
        
            foreach (var item in results.Items)
            {
                Console.WriteLine(item);
            }
        }
    }

    public async Task BuildIndex()
    {
        if (File.Exists(HeaderFileName))
            File.Delete(HeaderFileName);
        if (File.Exists(IndexFileName))
            File.Delete(IndexFileName);
        if (File.Exists(TailFileName))
            File.Delete(TailFileName);

        await using var headerStream = File.OpenWrite(HeaderFileName);
        await using var indexStream = File.OpenWrite(IndexFileName);
        await using var tailStream = File.OpenWrite(TailFileName);

        var builder = new IndexBuilder(headerStream, indexStream, tailStream);
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
}