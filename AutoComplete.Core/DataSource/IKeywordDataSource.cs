using System.Collections.Generic;

namespace AutoComplete.Core.DataSource
{
    public interface IKeywordDataSource
    {
        string Name { get; }

        IEnumerable<string> GetKeywords();
    }
}