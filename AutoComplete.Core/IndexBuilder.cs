using AutoComplete.Core.DataSource;
using AutoComplete.Core.DataStructure;
using System.Collections.Generic;
using System.IO;

namespace AutoComplete.Core
{
    public class IndexBuilder : IIndexBuilder
    {
        private TrieNodeHelper _helper;
        private Trie _trie;
        private Stream _headerStream;
        private Stream _indexStream;

        public IndexBuilder(Stream headerStream, Stream indexStream)
        {
            _headerStream = headerStream;
            _indexStream = indexStream;

            _helper = new TrieNodeHelper();
            _trie = new Trie();
        }

        public IndexBuilder Add(string keyword)
        {
            _trie.Add(TrieNodeInput.Create(keyword));
            return this;
        }

        public IndexBuilder AddRange(IEnumerable<string> keywords)
        {
            if (keywords != null)
            {
                foreach (var item in keywords)
                {
                    Add(item);
                }
            }

            return this;
        }

        public IndexBuilder WithDataSource(IKeywordDataSource keywordDataSource)
        {
            _trie.Load(keywordDataSource);
            return this;
        }

        /// <summary>
        /// Dont forget to close streams after read.
        /// </summary>
        /// <param name="headerStream"></param>
        /// <param name="indexStream"></param>
        /// <returns></returns>
        public int Build()
        {
            PrepareForBuild();

            _helper.CreateHeader(_headerStream);

            int _processedNodeCount = _helper.CreateIndex(_trie.Root, _indexStream);

            return _processedNodeCount;
        }

        private void PrepareForBuild()
        {
            _helper.ReorderTrieAndLoadHeader(_trie.Root);
        }

    }
}
