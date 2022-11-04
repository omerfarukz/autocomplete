[![Build Status](https://autocompletecore.visualstudio.com/autocomplete/_apis/build/status/omerfarukz.autocomplete?branchName=master)](https://autocompletecore.visualstudio.com/autocomplete/_build/latest?definitionId=4&branchName=master)

# Incredible autocomplete library
- Fastest autocomplete algorithm. 
- **O(n)** complexity for searching (n is the length of input)
- Supports for all stream types. Including on classical disk storage for cheapest hosting.
- Ready to use in web, desktop, and **cloud**!.
- Support for .Net Core 2.0 and .Net Framework 4.6.1
- **Free** commercial usage (Apache 2.0 license)

## Is it cloud ready?
Absolutely. All you need is provide Stream-based instance for building and searching indexes. Autocomplete can be run on any Stream like a MemoryStream, FileStream, Azure blobs and others.

## Where can I get it?
First, install NuGet. Then, install AutoComplete from the package manager console:

```
Install-Package AutoComplete.Core
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
SearchResult searchResult = searcher.Search(term, 5, false);
//print(searchResult)
```

