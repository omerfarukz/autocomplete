using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoComplete.DataStructure;
using AutoComplete.Serializers;

namespace AutoComplete.Builders
{
    public class IndexBuilder : IIndexBuilder, IDisposable
    {
        private static readonly byte[] NewLine = Encoding.UTF8.GetBytes(Environment.NewLine);
        private readonly Stream _headerStream;
        private readonly Stream _indexStream;
        private readonly Dictionary<string, uint> _keywordDictionary;
        private readonly Dictionary<string, int> _keyDictionary;
        private readonly Stream _tailStream;
        private readonly Trie _trie;
        private TrieIndexHeader _header;
        private HashSet<string> _keywords;

        public IndexBuilder(Stream headerStream, Stream indexStream, Stream tailStream = null)
        {
            _headerStream = headerStream;
            _indexStream = indexStream;
            _tailStream = tailStream;

            _header = new TrieIndexHeader();
            _trie = new Trie();
            _keywords = new HashSet<string>();
            _keywordDictionary = new Dictionary<string, uint>();
            _keyDictionary = new Dictionary<string, int>();
        }

        public IndexBuilder Add(string keyword)
        {
            if (keyword != null && !_keywords.Contains(keyword))
            {
                _trie.Add(keyword);
                _keywords.Add(keyword);
            }

            return this;
        }

        /// <summary>
        ///     Dont forget to close streams after read.
        /// </summary>
        /// <returns>Processed node count</returns>
        public int Build()
        {
            PrepareForBuild();

            var serializer = new TrieIndexHeaderSerializer();
            serializer.Serialize(_headerStream, _header);

            var processedNodeCount = TrieIndexSerializer.Serialize(_trie.Root, _header, _indexStream);
            return processedNodeCount;
        }

        private void PrepareForBuild()
        {
            ReorderTrieAndLoadHeader(_trie.Root);

            if (_tailStream != null)
                CreateTailAndModifyNodes(_trie.Root);
        }

        private void ReorderTrieAndLoadHeader(TrieNode rootNode)
        {
            var indexerQueue = new Queue<TrieNode>();
            indexerQueue.Enqueue(rootNode);

            var order = 0;
            var builder = new TrieIndexHeaderBuilder();

            while (indexerQueue.Count > 0)
            {
                var currentNode = indexerQueue.Dequeue();
                currentNode.Order = order;
                builder.AddChar(currentNode.Character);

                // set parent's children index when current node's child
                // index not equal to zero and current index is not the root
                if (currentNode.Parent != null && currentNode.ChildIndex == 0)
                {
                    currentNode.Parent.ChildrenCount = currentNode.Order - currentNode.Parent.Order;
                }

                if (currentNode.Children != null)
                {
                    var childIndex = 0;
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

            var serializerQueue = new Queue<TrieNode>();
            serializerQueue.Enqueue(root);

            while (serializerQueue.Count > 0)
            {
                var currentNode = serializerQueue.Dequeue();
                var currentNodeAsString = currentNode.GetString();
                if (currentNode == root)
                {
                    currentNode.PositionOnTextFile = 0;
                }
                else
                {
                    SetPositionOfCurrentNode(currentNode, currentNodeAsString);
                }

                if (currentNode.Children == null)
                    continue;

                foreach (var childNode in currentNode.Children)
                {
                    serializerQueue.Enqueue(childNode.Value);
                }
            }
        }

        private void SetPositionOfCurrentNode(TrieNode currentNode, string currentNodeAsString)
        {
            // am i terminal node ?
            if (currentNode.IsTerminal)
            {
                currentNode.PositionOnTextFile ??= _keywordDictionary[currentNodeAsString];
            }
            else
            {
                // who is my nearest terminal?
                var nodeResult = GetNearestTerminalChildren(currentNode);
                if (
                    nodeResult.Status == TrieNodeSearchResultType.FoundEquals ||
                    nodeResult.Status == TrieNodeSearchResultType.FoundStartsWith
                )
                {
                    var positionOnTextFile = _keywordDictionary[nodeResult.Node.GetString()];
                    nodeResult.Node.PositionOnTextFile = positionOnTextFile;
                    currentNode.PositionOnTextFile = positionOnTextFile;
                }
                else
                {
                    // no one like me. i am alone. (it's root.)
                    currentNode.PositionOnTextFile = 0;
                }
            }
        }

        private TrieNodeSearchResult GetNearestTerminalChildren(TrieNode currentNode)
        {
            var serializerQueue = new Queue<TrieNode>();
            serializerQueue.Enqueue(currentNode);

            while (serializerQueue.Count > 0)
            {
                currentNode = serializerQueue.Dequeue();
                if (currentNode.IsTerminal)
                    return TrieNodeSearchResult.CreateFoundEquals(currentNode);

                if (currentNode.Children == null)
                    continue;

                foreach (var childNode in currentNode.Children)
                {
                    serializerQueue.Enqueue(childNode.Value);
                }
            }

            return TrieNodeSearchResult.CreateNotFound();
        }

        private void SerializeKeywords(Stream stream)
        {
            var keywords = new HashSet<string>(
                _keywords.OrderBy(f => f, new TrieStringComparer())
            );

            foreach (var item in keywords)
            {
                for (var i = 1; i < item.Length; i++)
                {
                    var substring = item[..i];
                    if (keywords.Contains(substring))
                    {
                        if (!_keyDictionary.ContainsKey(substring))
                            _keyDictionary.Add(substring, 1);

                        _keyDictionary[substring]++;
                    }
                }
            }

            stream.Position = 0;
            foreach (var item in keywords)
            {
                _keywordDictionary.Add(item, (uint) stream.Position);
                var count = _keyDictionary.GetValueOrDefault(item, 0);
                var buffer = Encoding.UTF8.GetBytes($"{count,10},{item}");
                stream.Write(buffer, 0, buffer.Length);
                stream.Write(NewLine, 0, NewLine.Length);
            }

        }

        public void Dispose()
        {
            _headerStream?.Dispose();
            _indexStream?.Dispose();
            _tailStream?.Dispose();
            _keywords = null;
        }
    }
}