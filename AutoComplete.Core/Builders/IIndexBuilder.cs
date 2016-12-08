using System.Collections.Generic;

namespace AutoComplete.Core.Builders
{
    public interface IIndexBuilder
    {
        IndexBuilder Add(string keyword);

        IndexBuilder AddRange(IEnumerable<string> keywords);

        int Build();
    }
}