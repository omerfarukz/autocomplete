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
        private static Dictionary<string, TrieIndexHeader> _headerDictionary = new Dictionary<string, TrieIndexHeader>();
        private static Dictionary<string, byte[]> _indexData = new Dictionary<string, byte[]>();

        private string _headerFileName;
        private string _indexFileName;

        public InMemoryIndexSearcher(string headerFileName, string indexFileName)
            : base()
        {
            _headerFileName = headerFileName;
            _indexFileName = indexFileName;
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
                        Stream temporaryStream = new FileStream(
                                                _indexFileName,
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.Read,
                                                FirstReadBufferSize,
                                                FileOptions.RandomAccess
                                           );

                        temporaryStream.Position = 0;

                        byte[] streamBytes = new byte[temporaryStream.Length];
                        temporaryStream.Read(streamBytes, 0, streamBytes.Length);

                        //temporaryStream.Close();
                        temporaryStream.Dispose();
                        temporaryStream = null;

                        _indexData.Add(_indexFileName, streamBytes);
                    }
                }
            }

            Stream actualStream = new ManagedInMemoryStream(_indexData[_indexFileName]);

            return actualStream;
        }
    }
}