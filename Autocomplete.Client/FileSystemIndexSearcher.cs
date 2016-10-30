using AutoComplete.Core;
using System.Collections.Generic;

using System.IO;

namespace AutoComplete.Client
{
    public class FileSystemIndexSearcher : IndexSearcher
    {
        private static object _lockObject = new object();
        private static TrieIndexHeader _header;

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
            if (_header == null)
            {
                lock (_lockObject)
                {
                    if (_header == null)
                    {
                        _header = TrieNodeHelperFileSystemExtensions.ReadHeaderFile(_headerFileName);
                    }
                }
            }

            return _header;
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