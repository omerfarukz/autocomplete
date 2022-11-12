using System;
using System.IO;
using System.Linq;
using AutoComplete.DataStructure;
using AutoComplete.Domain;
using AutoComplete.Readers;

namespace AutoComplete.Searchers
{
    public abstract class IndexSearcher : IIndexSearcher
    {
        private IndexData _indexData;
        private TrieBinaryReader _reader;

        public virtual SearchResult Search(SearchOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            
            var node = _reader.SearchLastNode(0, options.Term);
            return CreateResultFromNode(_reader, node, options.Term, options);
        }

        public void Init()
        {
            _indexData = InitializeIndexData();
            _reader = CreateTrieBinaryReader();
        }

        protected abstract IndexData InitializeIndexData();

        private SearchResult CreateResultFromNode(TrieBinaryReader trieBinaryReader, TrieNodeStructSearchResult node,
            string keyword, SearchOptions options)
        {
            var searchResult = new SearchResult();
            if (node.Status == TrieNodeSearchResultType.NotFound)
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

                prefix = keyword.Substring(0, node.LastFoundCharacterIndex - 1);
            }
            else if (node.Status == TrieNodeSearchResultType.FoundEquals)
            {
                prefix = _indexData.Tail == null ? keyword.Substring(0, keyword.Length - 1) : keyword;
            }

            searchResult.ResultType = node.Status;

            if (_indexData.Tail != null)
            {
                searchResult.Items = trieBinaryReader.GetAutoCompleteNodes(
                    node.LastFoundNodePosition,
                    prefix,
                    options.MaxItemCount,
                    _indexData.Tail
                ).ToArray();
            }
            else
            {
                searchResult.Items = trieBinaryReader.GetAutoCompleteNodes(
                    node.LastFoundNodePosition,
                    prefix,
                    options.MaxItemCount
                ).ToArray();
            }
            

            return searchResult;
        }

        private TrieBinaryReader CreateTrieBinaryReader()
        {
            var stream = _indexData.Index;
            var binaryReader = new BinaryReader(stream);
            var trieBinaryReader = new TrieBinaryReader(binaryReader, _indexData.Header);

            return trieBinaryReader;
        }
    }
}