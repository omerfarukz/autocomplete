using AutoComplete.Core.DataStructure;
using AutoComplete.Core.Domain;
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

        public IndexSearcher()
        {}

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

            var trieBinaryReader = CreateTrieBinaryReader();

            var input = TrieNodeInput.Create(options.Term);
            var node = trieBinaryReader.GetLastNode(0, input);

            return CreateResultFromNode(trieBinaryReader, node, input, options);
        }

        private SearchResult CreateResultFromNode(TrieBinaryReader trieBinaryReader, TrieNodeStructSearchResult node, TrieNodeInput input, SearchOptions options)
        {
            var searchResult = new SearchResult();
            if (node.Status == TrieNodeSearchResultType.NotFound || node.Status == TrieNodeSearchResultType.Unkown)
            {
                searchResult.ResultType = node.Status;
                return searchResult;
            }

            string prefix = null;
            if (node.Status == TrieNodeSearchResultType.FoundStartsWith)
            {
                if (!options.SuggestWhenFoundStartsWith)
                {
                    searchResult.ResultType = node.Status;
                    return searchResult;
                }

                prefix = input.Keyword.Substring(0, node.LastFoundCharacterIndex - 1);
            }
            else if (node.Status == TrieNodeSearchResultType.FoundEquals)
            {
                prefix = input.Keyword.Substring(0, input.Keyword.Length - 1);
            }

            searchResult.ResultType = node.Status;
            searchResult.Items = trieBinaryReader.GetAutoCompleteNodes(
                                                                    node.LastFoundNodePosition,
                                                                    prefix,
                                                                    options.MaxItemCount,
                                                                    new List<string>()
                                                                ).ToArray();

            return searchResult;
        }

        internal virtual TrieIndexHeader GetHeader()
        {
            var header = TrieSerializer.DeserializeHeaderWithXmlSerializer(_headerStream);
            _headerStream.Dispose(); // TODO: 

            return header;
        }

        internal virtual Stream GetIndexStream()
        {
            return _indexStream;
        }

        private TrieBinaryReader CreateTrieBinaryReader()
        {
            Stream stream = GetIndexStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            var trieBinaryReader = new TrieBinaryReader(binaryReader, _header);

            return trieBinaryReader;
        }
    }
}