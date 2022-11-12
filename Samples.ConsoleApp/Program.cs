using Samples.ConsoleApp;

var sample = new Sample();
await sample.BuildIndex();
sample.Search();