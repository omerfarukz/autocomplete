using AutoComplete.Core.DataStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoComplete.Core.Builders
{
    public class IndexBuilder : IIndexBuilder
    {
        private TrieIndexHeader _header;
        private Trie _trie;
        private Stream _headerStream;
        private Stream _indexStream;

        private Stream _tailStream;
        private HashSet<string> _keywords;
        private Dictionary<string, uint> _keywordDictionary;
        static readonly byte[] newLine = Encoding.UTF8.GetBytes("\n");

        public IndexBuilder(Stream headerStream, Stream indexStream) : this(headerStream, indexStream, null)
        { }

        public IndexBuilder(Stream headerStream, Stream indexStream, Stream tailStream)
        {
            _headerStream = headerStream;
            _indexStream = indexStream;
            _tailStream = tailStream;

            _header = new TrieIndexHeader();
            _trie = new Trie();
            _keywords = new HashSet<string>();
            _keywordDictionary = new Dictionary<string, uint>();
        }

        public IndexBuilder Add(string keyword)
        {
            _trie.Add(keyword);

            if (keyword != null && !_keywords.Contains(keyword))
            {
                _keywords.Add(keyword);
            }

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

        /// <summary>
        /// Dont forget to close streams after read.
        /// </summary>
        /// <param name="headerStream"></param>
        /// <param name="indexStream"></param>
        /// <returns>Processed node count</returns>
        public int Build()
        {
            PrepareForBuild();

            TrieSerializer.SerializeHeaderWithJsonSerializer(_headerStream, _header);

            var processedNodeCount = TrieSerializer.SerializeIndexWithBinaryWriter(_trie.Root, _header, _indexStream);
            return processedNodeCount;
        }

        private void PrepareForBuild()
        {
            ReorderTrieAndLoadHeader(_trie.Root);

            if (_tailStream != null)
            {
                CreateTailAndModifyNodes(_trie.Root);
            }
        }

        private void ReorderTrieAndLoadHeader(TrieNode rootNode)
        {
            TrieIndexHeader header = new TrieIndexHeader();
            Queue<TrieNode> indexerQueue = new Queue<TrieNode>();
            indexerQueue.Enqueue(rootNode);

            int order = 0;
            var builder = new TrieIndexHeaderBuilder();

            TrieNode currentNode = null;
            while (indexerQueue.Count > 0)
            {
                currentNode = indexerQueue.Dequeue();

                if (currentNode == null)
                    throw new ArgumentNullException("Root node is null");

                currentNode.Order = order;
                builder.AddChar(currentNode.Character);

                // set parent's children index when current node's child
                // index not equal to zero and current index is not the root
                if (currentNode.Parent != null && currentNode.ChildIndex == 0)
                {
                    currentNode.Parent.ChildrenCount = (currentNode.Order - currentNode.Parent.Order);
                }

                if (currentNode.Children != null)
                {
                    int childIndex = 0;

                    foreach (var childNode in currentNode.Children)
                    {
                        childNode.Value.ChildIndex = childIndex++;
                        indexerQueue.Enqueue(childNode.Value);
                    }
                }

                ++order;
            }

            _header = builder.Build();
        }

        private void CreateTailAndModifyNodes(TrieNode root)
        {
            SerializeKeywords(_tailStream);

            Queue<TrieNode> serializerQueue = new Queue<TrieNode>();
            serializerQueue.Enqueue(root);

            TrieNode currentNode = null;
            while (serializerQueue.Count > 0)
            {
                currentNode = serializerQueue.Dequeue();

                string currentNodeAsString = currentNode.GetString();

                if (currentNode == root)
                {
                    currentNode.PositionOnTextFile = 0;
                }
                else
                {
                    // am i terminaL ?
                    if (currentNode.IsTerminal)
                    {
                        if (!currentNode.PositionOnTextFile.HasValue)
                        {
                            currentNode.PositionOnTextFile = _keywordDictionary[currentNodeAsString];
                        }
                    }
                    else
                    {
                        // who is my nearest terminal?
                        var nodeResult = GetNearestTerminalChildren(currentNode);
                        if (
                            (
                                nodeResult.Status == TrieNodeSearchResultType.FoundEquals ||
                                nodeResult.Status == TrieNodeSearchResultType.FoundStartsWith
                            )
                        )
                        {
                            if (!nodeResult.Node.PositionOnTextFile.HasValue)
                            {
                                var positionOnTextFile = _keywordDictionary[nodeResult.Node.GetString()];
                                nodeResult.Node.PositionOnTextFile = positionOnTextFile;
                                currentNode.PositionOnTextFile = positionOnTextFile;
                            }
                        }
                        else
                        {
                            // no one like me. i am alone. (it's root.)
                            currentNode.PositionOnTextFile = 0; // b
                        }
                    }
                }

                if (currentNode.Children != null)
                {
                    foreach (var childNode in currentNode.Children)
                    {
                        serializerQueue.Enqueue(childNode.Value);
                    }
                }
            }
        }

        private TrieNodeSearchResult GetNearestTerminalChildren(TrieNode currentNode)
        {
            Queue<TrieNode> serializerQueue = new Queue<TrieNode>();
            serializerQueue.Enqueue(currentNode);

            while (serializerQueue.Count > 0)
            {
                currentNode = serializerQueue.Dequeue();

                if (currentNode.IsTerminal)
                    return TrieNodeSearchResult.CreateFoundEquals(currentNode);

                if (currentNode.Children != null)
                {
                    foreach (var childNode in currentNode.Children)
                    {
                        serializerQueue.Enqueue(childNode.Value);
                    }
                }
            }

                return TrieNodeSearchResult.CreateNotFound();
        }

        private void SerializeKeywords(Stream stream)
        {
            //test
            stream.Position = 0;
            foreach (var item in _keywords.OrderBy(f => f, new TrieStringComparer()))
            {
                _keywordDictionary.Add(item, (uint)stream.Position);

                var buffer = Encoding.UTF8.GetBytes(item);
                stream.Write(buffer, 0, buffer.Length);
                stream.Write(newLine, 0, newLine.Length);
            }

            _keywords.Clear();
            _keywords = null;

        }
    }
}