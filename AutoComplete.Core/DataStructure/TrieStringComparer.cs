using System.Collections.Generic;

namespace AutoComplete.Core.DataStructure
{
    internal class TrieStringComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            return left.CompareTo(right);
        }
    }
}
