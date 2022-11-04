using System.Diagnostics;
using AutoComplete.Builders;
using AutoComplete.Clients.IndexSearchers;
using AutoComplete.Domain;

var headerFileName = "header.bin";
var indexFileName = "index.bin";
var tailFileName = "tail.bin";

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

var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, tailFileName);
searcher.Init();

var stopWatch = new Stopwatch();
while (true)
{
    Console.WriteLine("Type a word");
    var line = "door";//Console.ReadLine();
    stopWatch.Restart();
    var results = searcher.Search(new SearchOptions() { Term = line, MaxItemCount = 1, SuggestWhenFoundStartsWith = false});
    stopWatch.Stop();
    Console.WriteLine(results.ResultType);
    Console.WriteLine($"Elapsed {stopWatch.Elapsed.Ticks}");
    foreach (var item in results.Items)
    {
        Console.WriteLine(item);
    }
}