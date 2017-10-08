# Autocomplete Library for DotNetCore
- Fully Persistent and Memory support
- Ultra lightweight and powerful
- Ready to use in web and desktop
- Portable Class Library
- Free commercial usage (apache 2.0 license)
- Fastest autocomplete algorithm. 
- O(n) complexity for searching (n is length of input)

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
