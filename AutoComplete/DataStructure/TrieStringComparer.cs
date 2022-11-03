using System.Collections.Generic;

namespace AutoComplete.DataStructure
{
    internal class TrieStringComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            return string.CompareOrdinal(left, right);
        }
    }
}