# Autocomplete Library for DotNetCore
- Fully Persistent and Memory support
- Ultra lightweight and powerful
- Ready to use in web and desktop
- Portable Class Library
- Free commercial usage (apache 2.0 licence)
- Fastest autocomplete algorithm. 
- O(n) complexity for searching (n is length of input)

## Where can I get it?
First, install NuGet. Then, install AutoComplete from the package manager console:

```
Install-Package AutoComplete.Clients
```

# Is it production ready?
We published this library about 3 years ago and we are using this while 4 years for tureng. Tureng is best dictionary site of Turkey. We handle billions of request about 0.05 ms(on disc). Andalso this library providing a memory storage and this makes average search speed 10 times faster. We provide autocomplete feature over millions of records and handle billions of request.

# Is it cloud ready?
Absolutely. All you need is provide Stream based instance for building and searching indexes. Autocomplete can be run on any Stream like a MemoryStream, FileStream, azure blobs and others.

# What is the purpose of this?
We are using cloud services and we like these. Many cloud providers wants more money for extra memory capacity instead of extra disc capacity. Some other libraries powerful for autocomple. For example, lucene is powerful library for full text searching and also she provides autocomplete. We love it. But autocomplete and full text search things are completely different. Average autocomplete time for lucene is 0.7 ms(100x times slover then us)

# What does speed is mean?
We tested our code on persisted storage like in memory, classical hdd, ssd, azure blobs and some other storage providers.  We designed and optimized our index for linear reading. Search speed is really depends on your disk speed. Many cloud infrastructure provide larger read buffer(like a 30mb). These sounds good for linear reading. The search speed in this senario is depends some other resources like a network speed.

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

