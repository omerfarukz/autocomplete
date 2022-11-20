[![CI & CD](https://github.com/omerfarukz/autocomplete/actions/workflows/CI%20&%20CD.yml/badge.svg)](https://github.com/omerfarukz/autocomplete/actions/workflows/CI%20&%20CD.yml)
[![license](https://img.shields.io/github/license/omerfarukz/autocomplete)](https://github.com/omerfarukz/autocomplete/blob/master/LICENSE.txt)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=coverage)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)

[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=omerfarukz_autocomplete&metric=bugs)](https://sonarcloud.io/summary/new_code?id=omerfarukz_autocomplete)

# Incredible autocomplete library
- Get search result in **μs** (microseconds) or nanoseconds
- Fastest autocomplete algorithm. 
- **O(n)** complexity for searching (n is the length of input)
- Supports for all stream types. Including on classical disk storage for cheapest hosting.
- Ready to use in web, desktop, and **cloud**!.
- Support for .Net Standard 2.1+ and .Net Framework 4.6.1
- **Free** commercial usage

```
BenchmarkDotNet=v0.13.2, OS=macOS 13.0.1 (22A400) [Darwin 22.1.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.100-rc.2.22477.23
  [Host]     : .NET 6.0.2 (6.0.222.6406), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 6.0.2 (6.0.222.6406), Arm64 RyuJIT AdvSIMD
```
|                     Method |       Mean |    Error |   StdDev |
|--------------------------- |-----------:|---------:|---------:|
|   search_door_get_one_item |   711.1 ns |  9.46 ns |  8.85 ns |
| search_door_get_five_items | 1,045.1 ns | 10.81 ns | 10.11 ns |
|    search_car_get_one_item |   614.0 ns | 12.46 ns | 11.05 ns |
|  search_car_get_five_items |   872.1 ns |  3.76 ns |  5.02 ns |

## Is it cloud ready?
Absolutely. All you need is provide Stream-based instance for building and searching indexes. Autocomplete can be run on any Stream like a MemoryStream, FileStream, Azure blobs and others.

## Where can I get it?
First, install NuGet. Then, install AutoComplete from the package manager console:

```
Install-Package AutoComplete.Net
```

## Is it production ready?
Definitely. We published this library on 2016 and we are using this since 2015 for tureng. Tureng is the best dictionary site of Turkey. We handle billions of request about 0.05 ms(on disk). And also this library providing a memory storage and this makes average search speed 1000 times faster. We provide autocomplete feature over millions of records and handle billions of request.

## What is the purpose of this?
We are using cloud services and we like these. Many cloud providers want more money for extra memory capacity instead of extra disk capacity. Some other libraries powerful for autocomplete. For example, Lucene is a powerful library for full-text searching and also she provides autocomplete. We love it. But autocomplete and full-text search things are completely different. Average autocomplete time for Lucene is 70 ms(1000 times slower than us)

## What does speed is mean?
We tested our code on persisted storage like HDD, SSD, Azure blobs and some other storage providers.  We designed and optimized our index for linear reading. Search speed really depends on your disk speed. Many cloud infrastructure provides larger read buffer(like a 30mb). These sound good for linear reading. The search speed in this scenario is depended on some other resources like a network speed.

http://tureng.com - Live on best dictionary site of Turkey!

## Index Builder Sample
```csharp
var builder = new IndexBuilder(headerStream, indexStream);
builder.Add("keyword");
//builder.AddRange([IEnumerable<string>]);
//builder.WithDataSource(source);
builder.Build();
```

## Search sample
```csharp
IIndexSearcher searcher = new InMemoryIndexSearcher(headerPath, indexPath);
searcher.Init();
SearchResult searchResult = searcher.Search(term, 5, false);
//print(searchResult)

// Type a word
// perf
// Elapsed(ms) 0.0057
// perf
// perfay
// perfect
// perfecta
// perfectability

```

