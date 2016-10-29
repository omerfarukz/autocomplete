using AutoComplete.Core;
using System.Collections.Generic;

using System.IO;

namespace AutoComplete.Client
{
    public class FileSystemIndexSearcher : IndexSearcher
    {
        //check
        private static object _lockObject = new object();
        private static Dictionary<string, TrieIndexHeader> _headerDictionary = new Dictionary<string, TrieIndexHeader>();

        private string _headerFileName;
        private string _indexFileName;

        public FileSystemIndexSearcher(string headerFileName, string indexFileName)
            : base()
        {
            _headerFileName = headerFileName;
            _indexFileName = indexFileName;
        }

        internal override TrieIndexHeader GetHeader()
        {
            TrieIndexHeader header = null;

            if (!_headerDictionary.ContainsKey(_headerFileName))
            {
                header = TrieNodeHelperFileSystemExtensions.ReadHeaderFile(_headerFileName);

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