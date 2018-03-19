using System.Collections.Generic;

namespace AutoComplete.Core.DataStructure
{
    internal class TrieStringComparer : IComparer<string>
    {
        public static TrieStringComparer Instance = new TrieStringComparer();

        public int Compare(string left, string right)
        {
            return left.CompareTo(right);
        }
    }
}
