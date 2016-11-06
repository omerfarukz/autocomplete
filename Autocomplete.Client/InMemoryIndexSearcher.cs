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
        private static Dictionary<string, TrieIndexHeader> _headerDictionary;
        private static Dictionary<string, byte[]> _indexData;
        private static Dictionary<string, byte[]> _tailData;

        private string _headerFileName;
        private string _indexFileName;
        private string _tailFileName;

        public InMemoryIndexSearcher(string headerFileName, string indexFileName) :
            this(headerFileName, indexFileName, null)
        { }

        public InMemoryIndexSearcher(
                string headerFileName,
                string indexFileName,
                string tailFileName
            ) : base()
        {
            _headerFileName = headerFileName;
            _indexFileName = indexFileName;
            _tailFileName = tailFileName;

            _headerDictionary = new Dictionary<string, TrieIndexHeader>();
            _indexData = new Dictionary<string, byte[]>();
            _tailData = new Dictionary<string, byte[]>();
        }

        internal override TrieIndexHeader GetHeader()
        {
            // double checked initialization
            if (!_headerDictionary.ContainsKey(_headerFileName))
            {
                lock (_lockObject)
                {
                    if (!_headerDictionary.ContainsKey(_headerFileName))
                    {
                        var currentHeader = TrieNodeHelperFileSystemExtensions.ReadHeaderFile(_headerFileName);

                        _headerDictionary.Add(_headerFileName, currentHeader);
                    }
                }
            }

            TrieIndexHeader header = _headerDictionary[_headerFileName];
            return header;
        }

        internal override Stream GetIndexStream()
        {
            // double checked initialization
            if (!_indexData.ContainsKey(_indexFileName))
            {
                lock (_lockObject)
                {
                    if (!_indexData.ContainsKey(_indexFileName))
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

                        _indexData.Add(_indexFileName, streamBytes);
                    }
                }
            }

            return new ManagedInMemoryStream(_indexData[_indexFileName]);
        }

        internal override Stream GetTailStream()
        {
            if (_tailFileName == null)
                return null;

            // double checked initialization
            if (!_tailData.ContainsKey(_tailFileName))
            {
                lock (_lockObject)
                {
                    if (!_tailData.ContainsKey(_tailFileName))
                    {
                        Stream stream = new FileStream(
                                                _tailFileName,
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

                        _tailData.Add(_tailFileName, streamBytes);
                    }
                }
            }

            return new ManagedInMemoryStream(_tailData[_tailFileName]);
        }
    }
}