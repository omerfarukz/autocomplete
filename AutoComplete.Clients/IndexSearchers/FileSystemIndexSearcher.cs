using AutoComplete.Core;
using AutoComplete.Core.Searchers;
using System.Collections.Generic;
using System.IO;

namespace AutoComplete.Clients.IndexSearchers
{
    public class FileSystemIndexSearcher : IndexSearcher
    {
        private static object _lockObject = new object();
        private static Dictionary<string, TrieIndexHeader> _headers;

        private string _headerFileName;
        private string _indexFileName;
        private string _tailFileName;

        public FileSystemIndexSearcher(string headerFileName, string indexFileName)
            : this(headerFileName, indexFileName, null)
        { }

        public FileSystemIndexSearcher(string headerFileName, string indexFileName, string tailFileName)
            : base()
        {
            _headerFileName = headerFileName;
            _indexFileName = indexFileName;
            _tailFileName = tailFileName;

            _headers = new Dictionary<string, TrieIndexHeader>();
        }

        internal override TrieIndexHeader GetHeader()
        {
            if (!_headers.ContainsKey(_headerFileName))
            {
                lock (_lockObject)
                {
                    if (!_headers.ContainsKey(_headerFileName))
                    {
                        _headers.Add(_headerFileName, TrieNodeHelperFileSystemExtensions.ReadHeaderFile(_headerFileName));
                    }
                }
            }

            return _headers[_headerFileName];
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

        internal override Stream GetTailStream()
        {
            if (_tailFileName == null)
                return null;

            Stream indexStream = new FileStream(
                                        _tailFileName,
                                        FileMode.Open,
                                        FileAccess.Read,
                                        FileShare.Read,
                                        8,
                                        FileOptions.SequentialScan
                                   );

            return indexStream;
        }
    }
}