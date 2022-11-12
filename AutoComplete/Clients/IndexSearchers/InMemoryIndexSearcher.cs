using System.Collections.Concurrent;
using System.IO;
using AutoComplete.Domain;
using AutoComplete.Searchers;

namespace AutoComplete.Clients.IndexSearchers
{
    public class InMemoryIndexSearcher : IndexSearcher
    {
        private const int FirstReadBufferSize = 10 * 1024;
        private readonly string _headerFileName;
        private readonly string _indexFileName;
        private readonly string _tailFileName;
        
        public InMemoryIndexSearcher(
            string headerFileName,
            string indexFileName,
            string tailFileName = null
        )
        {
            _headerFileName = headerFileName;
            _indexFileName = indexFileName;
            _tailFileName = tailFileName;
        }
        
        protected override IndexData InitializeIndexData()
        {
            IndexData indexData;
            if (_tailFileName == null)
            {
                indexData = new IndexData(
                    TrieNodeHelperFileSystemExtensions.ReadHeaderFile(_headerFileName),
                    new ManagedInMemoryStream(GetBytesFromFile(_indexFileName))
                );
            }
            else
            {
                indexData = new IndexData(
                    TrieNodeHelperFileSystemExtensions.ReadHeaderFile(_headerFileName),
                    new ManagedInMemoryStream(GetBytesFromFile(_indexFileName)),
                    new ManagedInMemoryStream(GetBytesFromFile(_tailFileName))
                );
            }

            return indexData;
        }

        private static byte[] GetBytesFromFile(string path)
        {
            using Stream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                FirstReadBufferSize,
                FileOptions.RandomAccess
            );
            var streamBytes = new byte[stream.Length];
            stream.Read(streamBytes, 0, streamBytes.Length);
            return streamBytes;
        }
    }
}