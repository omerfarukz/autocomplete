namespace AutoComplete.Core
{
    public interface IIndexSearcher
    {
        SearchResult Search(string term, int maxItemCount, bool suggestWhenNotFound);

        SearchResult Search(SearchOptions options);
    }
}