using AutoComplete.Domain;

namespace AutoComplete.Searchers
{
    public interface IIndexSearcher
    {
        SearchResult Search(SearchOptions options);
        
        void Init(); 
    }
}