using System;
using System.Collections.Generic;

namespace AutoComplete.DataStructure
{
    internal class Trie
    {
        /// <summary>
        ///     Trie root node
        /// </summary>
        public readonly TrieNode Root;

        public Trie()
        {
            Root = new TrieNode();
            Root.Children = new SortedDictionary<char, TrieNode>(new TrieCharacterComparer());
        }

        /// <summary>
        ///     Get last node of characters given from input.Keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public TrieNodeSearchResult SearchLastNodeFrom(string keyword)
        {
            if (keyword == null)
                throw new ArgumentNullException(nameof(keyword));

            var currentNode = Root;
            for (var i = 0; i < keyword.Length; i++)
            {
                // i = keyword index
                var foundNode = currentNode.GetNodeFromChildren(keyword[i]);

                if (foundNode != null)
                {
                    // switch current node to found node
                    currentNode = foundNode;
                    continue;
                }

                // return last found node
                return TrieNodeSearchResult.CreateFoundStartsWith(currentNode, i);
            }

            // equals
            return TrieNodeSearchResult.CreateFoundEquals(currentNode);
        }

        public bool Add(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentNullException(nameof(keyword));

            // get last node from given input. Next lines we merge keywords when result status is FoundStartWith
            var result = SearchLastNodeFrom(keyword);

            if (result.Status == TrieNodeSearchResultType.NotFound)
                return false;

            if (result.Status == TrieNodeSearchResultType.FoundStartsWith)
            {
                // result found
                var prefix = keyword;

                // if last found node is start with? get 'word' from key|(word)
                if (result.LastKeywordIndex.HasValue &&
                    result.LastKeywordIndex > 0)
                {
                    // input.substring ( last found character index of input.Keyword in Trie, length of remaining characters )
                    prefix = keyword.Substring(
                        result.LastKeywordIndex.Value,
                        keyword.Length - result.LastKeywordIndex.Value
                    );
                }

                var newTrie = TrieNode.CreateFrom(prefix);
                result.Node.Add(newTrie);

                return true;
            } // result found

            if (result.Status == TrieNodeSearchResultType.FoundEquals && !result.Node.IsTerminal)
            {
                // duplicate-already exists
                // set the last character of input.Keyword to terminal
                result.Node.IsTerminal = true;
            }

            return false;
        }
    }
}