using AutoComplete.Core.DataSource;

using System;
using System.Collections.Generic;

namespace AutoComplete.Core.DataStructure
{
    internal class Trie
    {
        /// <summary>
        /// Trie root node
        /// </summary>
        public readonly TrieNode Root;

        public Trie()
        {
            this.Root = new TrieNode();
            this.Root.Children = new SortedDictionary<char, TrieNode>(new TrieCharacterComparer());
        }

        /// <summary>
        /// Get last node of characters given from input.Keyword
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TrieNodeSearchResult GetLastNode(TrieNodeInput input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            TrieNode currentNode = this.Root;

            for (int i = 0; i < input.Keyword.Length; i++)
            {
                // i = keyword index
                var foundNode = currentNode.GetNodeFromChildren(input.Keyword[i]);

                if (foundNode != null)
                {
                    // switch current node to found node
                    currentNode = foundNode;
                    continue;
                }
                else
                {
                    // return last found node
                    return TrieNodeSearchResult.CreateFoundStartsWith(currentNode, i);
                }
            }

            // equals
            return TrieNodeSearchResult.CreateFoundEquals(currentNode);
        }

        /// <summary>
        /// Load the specified dataSource and returns count of words processed.
        /// </summary>
        /// <param name="dataSource">Data source.</param>
        public int Load(IKeywordDataSource dataSource)
        {
            if (dataSource == null)
                throw new ArgumentException("dataSource");

            int countsOfWordsProcessed = 0;

            foreach (var keyword in dataSource.GetKeywords())
            {
                var input = TrieNodeInput.Create(keyword);

                Add(input);

                ++countsOfWordsProcessed;
            }

            return countsOfWordsProcessed;
        }

        public bool Add(TrieNodeInput input)
        {
            // Get last node from given input. Next lines we merge keywords when result status is FoundStartWith
            var result = GetLastNode(input);

            if (
                result.Status == TrieNodeSearchResultType.Unkown ||
                result.Status == TrieNodeSearchResultType.NotFound)
            {
                return false;
            }
            else if (result.Status == TrieNodeSearchResultType.FoundStartsWith)
            {
                //result found
                string keyword = input.Keyword;

                // if last found node is start with? get 'word' from key|(word)
                if (result.LastKeywordIndex != null && result.LastKeywordIndex.HasValue && result.LastKeywordIndex.Value > 0)
                {
                    // input.substring ( last found character index of input.Keyword in Trie, length of remaining characters )
                    keyword = input.Keyword.Substring(
                        result.LastKeywordIndex.Value,
                        input.Keyword.Length - result.LastKeywordIndex.Value
                    );
                }

                var newTrie = TrieNode.CreateFromKeyword(keyword);
                result.Node.AddChild(newTrie);

                return true;
            } //result found
            else if (result.Status == TrieNodeSearchResultType.FoundEquals)
            {
                // dublicate-already exists

                // set the last character of input.Keyword to terminal
                if (!result.Node.IsTerminal)
                    result.Node.IsTerminal = true;
            }

            return false;
        }
    }
}