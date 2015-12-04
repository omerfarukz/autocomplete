namespace AutoComplete.Core.DataStructure
{
    internal class TrieNodeSearchResult
    {
        public TrieNode Node { get; private set; }

        public int? LastKeywordIndex { get; private set; }

        public TrieNodeSearchResultType Status { get; private set; }

        public static TrieNodeSearchResult CreateFoundStartsWith(TrieNode node, int lastKeywordIndex)
        {
            var result = new TrieNodeSearchResult();
            result.Status = TrieNodeSearchResultType.FoundStartsWith;
            result.Node = node;
            result.LastKeywordIndex = lastKeywordIndex;

            return result;
        }

        public static TrieNodeSearchResult CreateFoundEquals(TrieNode node)
        {
            var result = new TrieNodeSearchResult();
            result.Status = TrieNodeSearchResultType.FoundEquals;
            result.Node = node;

            return result;
        }

        public static TrieNodeSearchResult CreateNotFound()
        {
            var result = new TrieNodeSearchResult();
            result.Status = TrieNodeSearchResultType.NotFound;

            return result;
        }

    }
}
