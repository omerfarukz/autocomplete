using System;

namespace AutoComplete.Core.DataStructure
{
    internal class TrieNodeInput
    {
        public string Keyword { get; private set; }

        public static TrieNodeInput Create(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("keyword");

            var returnValue = new TrieNodeInput();
            returnValue.Keyword = keyword;

            return returnValue;
        }
    }
}