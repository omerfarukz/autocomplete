using System;
using System.Collections.Generic;
using System.Text;

namespace AutoComplete.DataStructure
{
    /// <summary>
    ///     Trie node implementation.
    /// </summary>
    internal class TrieNode
    {
        /// <summary>
        ///     Initializes a new instance of the TrieNode class.
        /// </summary>
        /// <remarks>Children object is not initialized</remarks>
        /// <param name="character">Character.</param>
        public TrieNode(char character)
            : this()
        {
            Character = character;
        }

        /// <summary>
        ///     Using for creating root element and serialization
        /// </summary>
        public TrieNode()
        {
        }

        /// <summary>
        ///     Gets or sets the node order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        /// <summary>
        ///     Gets or sets the children offset.
        /// </summary>
        /// <value>The children offset.</value>
        public int ChildrenOffset { get; set; }

        /// <summary>
        ///     Gets or sets the children offset.
        /// </summary>
        /// <value>The children count.</value>
        internal int ChildrenCount { get; set; }

        /// <summary>
        ///     Gets or sets the indexed character.
        /// </summary>
        /// <value>The character.</value>
        public char Character { get; set; }

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public TrieNode Parent { get; set; }

        /// <summary>
        ///     Gets or sets the children.
        /// </summary>
        /// <value>The children not initialized in constuctor.</value>
        public IDictionary<char, TrieNode> Children { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is terminal.
        /// </summary>
        /// <value><c>true</c> if this instance is terminal; otherwise, <c>false</c>.</value>
        public bool IsTerminal { get; set; }

        /// <summary>
        ///     Gets or sets the index of the child.
        /// </summary>
        /// <value>The index of the child.</value>
        public int ChildIndex { get; set; }
        
        public uint? PositionOnTextFile { get; set; }

        public void Add(TrieNode child)
        {
            if (child == null)
                throw new ArgumentException("child");

            Children ??= new SortedDictionary<char, TrieNode>(new TrieCharacterComparer());

            // if child node character already exist on this node
            if (Children.ContainsKey(child.Character))
            {
                var existsNode = Children[child.Character];
                if (child.Children != null)
                {
                    foreach (var item in child.Children)
                    {
                        existsNode.Add(item.Value);
                    }
                }
            }
            else
            {
                child.Parent = this;
                Children.Add(child.Character, child);
            }
        }

        public TrieNode GetNodeFromChildren(char character)
        {
            if (Children == null || !Children.ContainsKey(character))
                return null;

            return Children[character];
        }

        /// <summary>
        ///     Returns a complate indexed keyword from node(concats last character to first character)
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            var sb = new StringBuilder();
            var currentNode = this;

            while (currentNode != null && currentNode.Parent != null)
            {
                sb.Insert(0, currentNode.Character);
                currentNode = currentNode.Parent;
            }

            return sb.ToString();
        }

        /// <summary>
        ///     From the keyword.
        /// </summary>
        /// <returns>The keyword.</returns>
        /// <param name="keyword">Keyword.</param>
        public static TrieNode CreateFrom(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length == 0)
                throw new ArgumentException(nameof(keyword));

            var returnValue = new TrieNode(keyword[0]);
            if (keyword.Length == 1)
            {
                returnValue.IsTerminal = true;
                return returnValue;
            }

            var currentNode = returnValue;

            //warning: i starts with one(1) because zero(0) indexed character used/checked before(previous lines)
            for (var i = 1; i < keyword.Length; i++)
            {
                var newNode = new TrieNode(keyword[i]);
                currentNode.Add(newNode);

                // set is terminal to true when i is last index number of keyword
                if (i == keyword.Length - 1)
                {
                    newNode.IsTerminal = true;
                }

                currentNode = newNode;
            }

            return returnValue;
        }
    }
}