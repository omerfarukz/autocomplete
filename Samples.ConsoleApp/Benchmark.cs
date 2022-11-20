using AutoComplete.Domain;
using BenchmarkDotNet.Attributes;

namespace Samples.ConsoleApp;

public class Benchmark
{
    private readonly Sample _sample = new Sample("header.bin", "index.bin", "tail.bin");
    
    [GlobalSetup]
    public async Task Build()
    {
        await _sample.Build();
    }
    
    [Benchmark]
    public SearchResult search_x_get_one_item()
    {
        return _sample.Search("x", 1, false);
    }
    
    [Benchmark]
    public SearchResult search_do_get_one_item()
    {
        return _sample.Search("do", 1, false);
    }
    
    [Benchmark]
    public SearchResult search_door_get_one_item()
    {
        return _sample.Search("door", 1, false);
    }

    [Benchmark]
    public SearchResult search_door_get_five_items()
    {
        return _sample.Search("door", 5, false);
    }
    
    [Benchmark]
    public SearchResult search_car_get_one_item()
    {
        return _sample.Search("car", 1, false);
    }

    [Benchmark]
    public SearchResult search_car_get_five_items()
    {
        return _sample.Search("car", 5, false);
    }
}