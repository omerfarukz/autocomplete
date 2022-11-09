using System.Diagnostics;
using AutoComplete.Builders;
using AutoComplete.Clients.IndexSearchers;
using AutoComplete.Domain;

const string headerFileName = "header.bin";
const string indexFileName = "index.bin";
const string tailFileName = "tail.txt";

var stopWatch = new Stopwatch();
stopWatch.Start();
await BuildIndex();
stopWatch.Stop();
Console.WriteLine($"Build time(ms) {stopWatch.Elapsed.TotalMilliseconds}");

Search(10);

void Search(int count)
{
    var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, tailFileName);
    searcher.Init();
    while (true)
    {
        Console.WriteLine("Type a word");
        var word = Console.ReadLine();
        var timings = new List<double>();
        for (int i = 0; i < count; i++)
        {
            stopWatch.Restart();
            var results = searcher.Search(new SearchOptions() { Term = word, MaxItemCount = 5, SuggestWhenFoundStartsWith = false});
            stopWatch.Stop();
            timings.Add(stopWatch.Elapsed.TotalMilliseconds);
            Console.WriteLine($"Search time(ms): {stopWatch.Elapsed.TotalMilliseconds}");
            if (i == count-1 && results.Items != null)
            {
                foreach (var item in results.Items)
                {
                    Console.WriteLine(item);
                }
            }
        }

        Console.WriteLine($"Average Search Time(ms): {timings.Average()}");
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
