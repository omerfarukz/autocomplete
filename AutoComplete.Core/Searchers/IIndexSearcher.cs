using AutoComplete.Core.Domain;

namespace AutoComplete.Core.Searchers
{
    public interface IIndexSearcher
    {
        SearchResult Search(string term, int maxItemCount, bool suggestWhenNotFound);

        SearchResult Search(SearchOptions options);
    }
}