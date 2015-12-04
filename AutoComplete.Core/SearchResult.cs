using AutoComplete.Core.DataStructure;

namespace AutoComplete.Core
{
    public class SearchResult
    {
        public string[] Items { get; set; }

        public TrieNodeSearchResultType ResultType { get; set; }
    }
}