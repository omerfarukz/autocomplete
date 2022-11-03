using System.Collections.Generic;

namespace AutoComplete.DataStructure
{
    internal class TrieCharacterComparer : IComparer<char>
    {
        public int Compare(char left, char right)
        {
            return left.CompareTo(right);
        }
    }
}