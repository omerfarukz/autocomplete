using AutoComplete.Core.DataStructure;

using AutoComplete.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutoComplete.Core
{
    public abstract class IndexSearcher : IIndexSearcher
    {
        private Stream _indexStream;
        private Stream _headerStream;

        private TrieIndexHeader _header;
        private TrieBinaryReader _trieBinaryReader;

        public IndexSearcher()
        {
            _trieBinaryReader = new TrieBinaryReader();
        }

        /// <summary>
        /// Don't forget the close stream(s) after search
        /// </summary>
        /// <param name="headerStream"></param>
        /// <param name="indexStream"></param>
        public IndexSearcher(Stream headerStream, Stream indexStream)
        {
            _indexStream = indexStream;
            _headerStream = headerStream;
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

            if (_header == null)
                _header = GetHeader();

            SearchResult searchResult = new SearchResult();

            Stream indexStreamInstance = GetIndexStream();
            BinaryReader binaryReader = new BinaryReader(indexStreamInstance);

            var input = TrieNodeInput.Create(options.Term);
            var lastNode = _trieBinaryReader.GetLastNode(binaryReader, _header, 0, input);

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

            foundItems = _trieBinaryReader.GetAutoCompleteNodes(binaryReader, _header, lastNode.LastFoundNodePosition, prefix, options.MaxItemCount, new List<string>());

            searchResult.ResultType = lastNode.Status;
            searchResult.Items = foundItems.ToArray();

            return searchResult;
        }

        internal virtual TrieIndexHeader GetHeader()
        {
            var header = TrieSerializer.DeserializeHeaderWithXmlSerializer(_headerStream);
            _headerStream.Dispose(); // TODO: breakpoint

            return header;
        }

        internal virtual Stream GetIndexStream()
        {
            return _indexStream;
        }
    }
}