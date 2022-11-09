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
- Get result in **μs**
- Fastest autocomplete algorithm. 
- **O(n)** complexity for searching (n is the length of input)
- Supports for all stream types. Including on classical disk storage for cheapest hosting.
- Ready to use in web, desktop, and **cloud**!.
- Support for .Net Standard 2.1+ and .Net Framework 4.6.1
- **Free** commercial usage

## Is it cloud ready?
Absolutely. All you need is provide Stream-based instance for building and searching indexes. Autocomplete can be run on any Stream like a MemoryStream, FileStream, Azure blobs and others.

## Where can I get it?
First, install NuGet. Then, install AutoComplete from the package manager console:

```
Install-Package AutoComplete.Net
```

## Is it production ready?
Definitely. We published this library about 3 years ago and we are using this while 4 years for tureng. Tureng is the best dictionary site of Turkey. We handle billions of request about 0.05 ms(on disk). And also this library providing a memory storage and this makes average search speed 10 times faster. We provide autocomplete feature over millions of records and handle billions of request.

## What is the purpose of this?
We are using cloud services and we like these. Many cloud providers want more money for extra memory capacity instead of extra disk capacity. Some other libraries powerful for autocomplete. For example, Lucene is a powerful library for full-text searching and also she provides autocomplete. We love it. But autocomplete and full-text search things are completely different. Average autocomplete time for Lucene is 0.7 ms(100 times slower than us)

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
// Elapsed 0.0773
// perf
// perfay
// perfect
// perfecta
// perfectability

```

