using AutoComplete.DataStructure;

namespace AutoComplete.Domain
{
    public record SearchResult
    {
        public string[] Items { get; set; }

        public TrieNodeSearchResultType ResultType { get; set; }
    }
}