using System.IO;
using AutoComplete.DataStructure;

namespace AutoComplete.Searchers
{
    public class IndexData
    {
        public TrieIndexHeader Header { get; set; }
        public Stream Index { get; set; }
        public Stream Tail { get; set; }
    }
}