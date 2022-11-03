using System.Collections.Generic;

namespace AutoComplete.Builders
{
    public interface IIndexBuilder
    {
        IndexBuilder Add(string keyword);
        int Build();
    }
}