using AutoComplete.DataStructure;

namespace AutoComplete.Domain
{
    public class SearchResult
    {
        public string[] Items { get; set; }

        public TrieNodeSearchResultType ResultType { get; set; }
    }
}