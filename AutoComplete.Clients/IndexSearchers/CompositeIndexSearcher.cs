using System;
using System.Collections;
using System.Collections.Generic;
using AutoComplete.Core.DataStructure;
using AutoComplete.Core.Domain;
using AutoComplete.Core.Searchers;
using System.Linq;

namespace Autocomplete.Clients.IndexSearchers
{
    public class CompositeIndexSearcher : IIndexSearcher
    {
        private readonly IIndexSearcher _primarySearcher;
        private readonly IIndexSearcher _secondarySearcher;

        public CompositeIndexSearcher(IIndexSearcher primarySearcher, IIndexSearcher secondarySearcher)
        {
            _primarySearcher = primarySearcher;
            _secondarySearcher = secondarySearcher;
        }

        public SearchResult Search(string term, int maxItemCount, bool suggestWhenNotFound)
        {
            return Search(new SearchOptions() { MaxItemCount = maxItemCount, SuggestWhenFoundStartsWith = suggestWhenNotFound, Term = term });
        }

        public SearchResult Search(SearchOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var primaryResult = _primarySearcher.Search(options);
            var secondaryResult = _secondarySearcher.Search(options);

            SearchResult compositeResult = CreateResult(primaryResult, secondaryResult, options);

            return compositeResult;
        }

        private SearchResult CreateResult(SearchResult primaryResult, SearchResult secondaryResult, SearchOptions options)
        {
            if (primaryResult == null)
                throw new ArgumentNullException(nameof(primaryResult));

            if (secondaryResult == null)
                throw new ArgumentNullException(nameof(secondaryResult));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var result = new SearchResult()
            {
                ResultType = TrieNodeSearchResultType.Unkown
            };

            var items = new List<string>(options.MaxItemCount * 2);

            if (primaryResult.ResultType == secondaryResult.ResultType)
            {
                if (primaryResult.Items != null)
                    items.AddRange(primaryResult.Items);

                if (secondaryResult.Items != null)
                    items.AddRange(secondaryResult.Items);

                items.Sort();

                result.ResultType = primaryResult.ResultType;
            }
            else if (primaryResult.ResultType == TrieNodeSearchResultType.FoundEquals)
            {
                if (primaryResult.Items != null)
                    items.AddRange(primaryResult.Items);

                result.ResultType = TrieNodeSearchResultType.FoundEquals;
            }
            else if (secondaryResult.ResultType == TrieNodeSearchResultType.FoundEquals)
            {
                if (secondaryResult.Items != null)
                    items.AddRange(secondaryResult.Items);

                result.ResultType = TrieNodeSearchResultType.FoundEquals;
            }
            else if (primaryResult.ResultType == TrieNodeSearchResultType.FoundStartsWith)
            {
                if (primaryResult.Items != null)
                    items.AddRange(primaryResult.Items);

                result.ResultType = TrieNodeSearchResultType.FoundStartsWith;
            }
            else if (secondaryResult.ResultType == TrieNodeSearchResultType.FoundStartsWith)
            {
                if (secondaryResult.Items != null)
                    items.AddRange(secondaryResult.Items);

                result.ResultType = TrieNodeSearchResultType.FoundStartsWith;
            }

            if (items.Count > 0)
            {
                result.Items = items.Take(options.MaxItemCount).ToArray();
            }

            return result;
        }

    }
}