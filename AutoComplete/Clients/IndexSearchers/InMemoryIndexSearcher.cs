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
            var indexData = new IndexData();
            indexData.Index = new ManagedInMemoryStream(GetBytesFromFile(_indexFileName));
            indexData.Header = TrieNodeHelperFileSystemExtensions.ReadHeaderFile(_headerFileName);
            if (_tailFileName != null)
                indexData.Tail = new ManagedInMemoryStream(GetBytesFromFile(_tailFileName));
            return indexData;
        }

        private byte[] GetBytesFromFile(string path)
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