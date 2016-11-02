using AutoComplete.Core;
using AutoComplete.Core.Domain;
using System.Collections.Generic;
using System.IO;

namespace AutoComplete.Client
{
    public class InMemoryIndexSearcher : IndexSearcher
    {
        private const int FirstReadBufferSize = 10 * 1024;

        private static object _lockObject = new object();
        private static Dictionary<string, TrieIndexHeader> _headers;
        private static Dictionary<string, byte[]> _indexes;

        private string _headerFileName;
        private string _indexFileName;

        public InMemoryIndexSearcher(string headerFileName, string indexFileName)
            : base()
        {
            _headerFileName = headerFileName;
            _indexFileName = indexFileName;

            _headers = new Dictionary<string, TrieIndexHeader>();
            _indexes = new Dictionary<string, byte[]>();
        }

        internal override TrieIndexHeader GetHeader()
        {
            // double checked initialization
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
            // double checked initialization
            if (!_indexes.ContainsKey(_indexFileName))
            {
                lock (_lockObject)
                {
                    if (!_indexes.ContainsKey(_indexFileName))
                    {
                        Stream stream = new FileStream(
                                                _indexFileName,
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.Read,
                                                FirstReadBufferSize,
                                                FileOptions.RandomAccess
                                           );

                        stream.Position = 0;
                        byte[] streamBytes = new byte[stream.Length];
                        stream.Read(streamBytes, 0, streamBytes.Length);

                        stream.Dispose();
                        stream = null;

                        _indexes.Add(_indexFileName, streamBytes);
                    }
                }
            }

            return new ManagedInMemoryStream(_indexes[_indexFileName]);
        }
    }
}