using Samples.ConsoleApp;

var headerFileName = "header.bin";
var indexFileName = "index.bin";
var tailFileName = "tail.txt";

await Sample.BuildIndex(headerFileName, indexFileName, tailFileName);
Sample.Search(headerFileName, indexFileName, tailFileName);