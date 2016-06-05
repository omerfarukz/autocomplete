using AutoComplete.Core.DataSource;

using System.Collections.Generic;

namespace AutoComplete.Core
{
    public interface IIndexBuilder
    {
        IndexBuilder Add(string keyword);

        IndexBuilder AddRange(IEnumerable<string> keywords);

        IndexBuilder WithDataSource(IKeywordDataSource keywordDataSource);

        int Build();
    }
}