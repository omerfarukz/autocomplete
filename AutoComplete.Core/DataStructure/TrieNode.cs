using System;
using System.Collections.Generic;
using System.Text;

namespace AutoComplete.Core.DataStructure
{
    /// <summary>
    /// Trie node implementation.
    /// </summary>
    internal class TrieNode
    {
        /// <summary>
        /// Gets or sets the node order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the children offset.
        /// </summary>
        /// <value>The children offset.</value>
        public int ChildrenOffset { get; set; }

        /// <summary>
        /// Gets or sets the children offset.
        /// </summary>
        /// <value>The children count.</value>
        public int ChildrenCount { get; set; }

        /// <summary>
        /// Gets or sets the indexed character.
        /// </summary>
        /// <value>The character.</value>
        public char Character { get ; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public TrieNode Parent { get ; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children not initialized in constuctor.</value>
        public  IDictionary<char, TrieNode> Children { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is terminal.
        /// </summary>
        /// <value><c>true</c> if this instance is terminal; otherwise, <c>false</c>.</value>
        public bool IsTerminal { get ; set; }

        /// <summary>
        /// Gets or sets the index of the child.
        /// </summary>
        /// <value>The index of the child.</value>
        public int ChildIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref=<TrieNode"/> class.
        /// </summary>
        /// <remarks>Children object is not initialized</remarks>
        /// <param name="character">Character.</param>
        public TrieNode(char character)
            : this()
        {
            Character = character;
        }

        /// <summary>
        /// Using for creating root element and serialization. TODO: make this a factory method
        /// </summary>
        public TrieNode()
        {
        }

        public void AddChild(TrieNode child)
        {
            if (child == null)
                throw new ArgumentException("child");

            if (Children == null)
                Children = new SortedDictionary<char, TrieNode>();

            // if child node character already exist on this node
            if (Children.ContainsKey(child.Character))
            {
                TrieNode existsNode = Children[child.Character];
                if (child.Children != null)
                {
                    foreach (var item in child.Children)
                    {
                        existsNode.AddChild(item.Value);
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
        /// Returns a complate indexed keyword from node(concats last character to first character)
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            StringBuilder sb = new StringBuilder();
            
            TrieNode currentNode = this;

            while (currentNode != null && currentNode.Parent != null)
            {
                sb.Insert(0, currentNode.Character);
                currentNode = currentNode.Parent;
            }

            return sb.ToString();
        }


        /// <summary>
        /// Create TrieNode from the keyword and set last char as terminal.
        /// </summary>
        /// <returns>The keyword.</returns>
        /// <param name="keyword">Keyword.</param>
        public static TrieNode CreateFromKeyword(string keyword)
        {
            return CreateFromKeyword(keyword, true);
        }

        // <summary>
        /// Froms the keyword.
        /// </summary>
        /// <returns>The keyword.</returns>
        /// <param name="keyword">Keyword.</param>
        /// <param name="setLastCharAsTerminal">If set to <c>true</c> set last char as terminal.</param>/
        public static TrieNode CreateFromKeyword(string keyword, bool setLastCharAsTerminal)
        {
            //TODO checks
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length == 0)
                throw new ArgumentException("keyword");

            TrieNode returnValue = new TrieNode(keyword[0]);
            if (keyword.Length == 1)
            {
                returnValue.IsTerminal = true;
                return returnValue;
            }

            TrieNode currentNode = returnValue;

            //warning: i starts with one(1) because zero(0) indexed character used/checked before(previous lines)
            for (int i = 1; i < keyword.Length; i++)
            {   
                TrieNode newNode = new TrieNode(keyword[i]);

                currentNode.AddChild(newNode);

                // set is terminal to true when i is last index number of keyword
                if (i == keyword.Length - 1 && setLastCharAsTerminal)
                {
                    newNode.IsTerminal = true;
                }

                currentNode = newNode;
            }

            return returnValue;
        }

    }
}

