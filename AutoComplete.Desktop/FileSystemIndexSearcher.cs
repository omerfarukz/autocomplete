using AutoComplete.Core;
using System.Collections.Generic;

using System.IO;

namespace AutoComplete.Desktop
{
    public class FileSystemIndexSearcher : IndexSearcher
    {
        private static object _lockObject = new object();
        private static Dictionary<string, TrieIndexHeader> _headerDictionary = new Dictionary<string, TrieIndexHeader>();

        private string _headerFileName;
        private string _indexFileName;
        private TrieNodeHelper _helper;

        public FileSystemIndexSearcher(string headerFileName, string indexFileName)
            : base()
        {
            _headerFileName = headerFileName;
            _indexFileName = indexFileName;

            _helper = new TrieNodeHelper();
        }

        internal override TrieIndexHeader GetHeader()
        {
            TrieIndexHeader header = null;

            if (!_headerDictionary.ContainsKey(_headerFileName))
            {
                header = _helper.ReadHeaderFile(_headerFileName, true);
                
                lock (_lockObject)
                {
                    _headerDictionary.Add(_headerFileName, header);
                }
            }
            else
            {
                header = _headerDictionary[_headerFileName];
            }

            return header;
        }

        internal override Stream GetIndexStream()
        {
            int bufferSize = GetHeader().LENGTH_OF_STRUCT;

            Stream indexStream = new FileStream(
                                        _indexFileName,
                                        FileMode.Open,
                                        FileAccess.Read,
                                        FileShare.Read,
                                        bufferSize,
                                        FileOptions.RandomAccess
                                   );

            return indexStream;
        }


    }
}
