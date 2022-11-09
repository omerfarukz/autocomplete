using System.Diagnostics;
using AutoComplete.Builders;
using AutoComplete.Clients.IndexSearchers;
using AutoComplete.Domain;

var headerFileName = "header.bin";
var indexFileName = "index.bin";
var tailFileName = "tail.txt";

if (File.Exists(headerFileName))
    File.Delete(headerFileName);
if (File.Exists(indexFileName))
    File.Delete(indexFileName);
if (File.Exists(tailFileName))
    File.Delete(tailFileName);

await using var headerStream = File.OpenWrite(headerFileName);
await using var indexStream = File.OpenWrite(indexFileName);
await using var tailStream = File.OpenWrite(tailFileName);

var stopWatch = new Stopwatch();
stopWatch.Start();
var builder = new IndexBuilder(headerStream, indexStream, tailStream);
foreach (var line in await File.ReadAllLinesAsync("words350k.txt"))
{
    if(!string.IsNullOrWhiteSpace(line))
        builder.Add(line);
}
builder.Build();
stopWatch.Stop();
Console.WriteLine($"Build time(ms) {stopWatch.Elapsed.TotalMilliseconds}");

headerStream.Close();
indexStream.Close();
tailStream.Close();

var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, tailFileName);
searcher.Init();

while (true)
{
    Console.WriteLine("Type a word");
    var word = Console.ReadLine();
    var timings = new List<double>();
    int count = 100;
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