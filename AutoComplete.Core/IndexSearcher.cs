using AutoComplete.Core.DataStructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutoComplete.Core
{
    public class IndexSearcher : IIndexSearcher
    {
        private Stream _indexStream;
        private Stream _headerStream;
        private TrieNodeHelper _helper;

        protected IndexSearcher()
        {
            _helper = new TrieNodeHelper();
        }

        /// <summary>
        /// Don't forget the close stream(s) after search
        /// </summary>
        /// <param name="headerStream"></param>
        /// <param name="indexStream"></param>
        public IndexSearcher(Stream headerStream, Stream indexStream)
        {
            _headerStream = headerStream;
            _indexStream = indexStream;
        }

        public virtual SearchResult Search(string term, int maxItemCount, bool suggestWhenNotFound)
        {
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.Term = term;
            searchOptions.MaxItemCount = maxItemCount;
            searchOptions.SuggestWhenFoundStartsWith = suggestWhenNotFound;

            return Search(searchOptions);
        }

        public SearchResult Search(SearchOptions options)
        {
            if (options == null)
                throw new ArgumentException("options");

            SearchResult searchResult = new SearchResult();

            _helper.Header = GetHeader();

            Stream indexStreamInstance = GetIndexStream();

            BinaryReader binaryReader = new BinaryReader(indexStreamInstance);

            var input = TrieNodeInput.Create(options.Term);
            var lastNode = _helper.GetLastNode(binaryReader, 0, input);

            List<string> foundItems = new List<string>();

            if (lastNode.Status == TrieNodeSearchResultType.NotFound || lastNode.Status == TrieNodeSearchResultType.Unkown)
            {
                searchResult.ResultType = lastNode.Status;
                return searchResult;
            }

            string prefix = null;

            if (lastNode.Status == TrieNodeSearchResultType.FoundStartsWith)
            {
                if (!options.SuggestWhenFoundStartsWith)
                {
                    searchResult.ResultType = lastNode.Status;
                    return searchResult;
                }

                prefix = input.Keyword.Substring(0, lastNode.LastFoundCharacterIndex - 1);
            }
            else if (lastNode.Status == TrieNodeSearchResultType.FoundEquals)
            {
                prefix = input.Keyword.Substring(0, input.Keyword.Length - 1);
            }

            foundItems = _helper.GetAutoCompleteNodes(binaryReader, lastNode.LastFoundNodePosition, prefix, options.MaxItemCount, new List<string>());

            searchResult.ResultType = lastNode.Status;
            searchResult.Items = foundItems.ToArray();

            return searchResult;
        }

        internal virtual TrieIndexHeader GetHeader()
        {
            return _helper.ReadHeader(_headerStream, true);
        }

        internal virtual Stream GetIndexStream()
        {
            return _indexStream;
        }

    }
}
