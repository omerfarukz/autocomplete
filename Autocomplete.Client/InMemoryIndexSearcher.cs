using AutoComplete.Core;
using AutoComplete.Core.Domain;
using System.IO;

namespace AutoComplete.Client
{
    public class InMemoryIndexSearcher : IndexSearcher
    {
        private const int FirstReadBufferSize = 10 * 1024;

        private static object _lockObject = new object();
        private static TrieIndexHeader _header;
        private static byte[] _index;

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
            // double checked initialization
            if (_index == null)
            {
                lock (_lockObject)
                {
                    if (_index == null)
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

                        _index = streamBytes;
                    }
                }
            }

            return new ManagedInMemoryStream(_index);
        }
    }
}