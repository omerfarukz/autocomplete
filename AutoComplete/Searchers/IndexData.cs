using System.IO;
using AutoComplete.DataStructure;

namespace AutoComplete.Searchers
{
    public class IndexData
    {
        public readonly TrieIndexHeader Header;
        public readonly Stream Index;
        public readonly Stream Tail;

        public IndexData(TrieIndexHeader header, Stream index, Stream tail = null)
        {
            Index = index;
            Tail = tail;
            Header = header;
        }
    }
}