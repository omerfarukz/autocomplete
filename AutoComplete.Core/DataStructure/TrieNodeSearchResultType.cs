namespace AutoComplete.Core.DataStructure
{
    public enum TrieNodeSearchResultType
    {
        Unkown = 0,
        FoundEquals = 10,
        FoundStartsWith = 20,
        NotFound = 30,
    }
}