using AutoComplete.Core.DataStructure;

namespace AutoComplete.Core.Domain
{
    public class SearchResult
    {
        public string[] Items { get; set; }

        public TrieNodeSearchResultType ResultType { get; set; }
    }
}