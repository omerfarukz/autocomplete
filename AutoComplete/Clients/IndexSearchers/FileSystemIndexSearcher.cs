using System.IO;
using AutoComplete.Searchers;

namespace AutoComplete.Clients.IndexSearchers
{
    public class FileSystemIndexSearcher : IndexSearcher
    {
        private readonly string _headerFileName;
        private readonly string _indexFileName;
        private readonly string _tailFileName;

        public FileSystemIndexSearcher(string headerFileName, string indexFileName, string tailFileName = null)
        {
            _headerFileName = headerFileName;
            _indexFileName = indexFileName;
            _tailFileName = tailFileName;
        }

        protected override IndexData InitializeIndexData()
        {
            IndexData indexData;
            var header = TrieNodeHelperFileSystemExtensions.ReadHeaderFile(_headerFileName);
            if (_tailFileName == null)
            {
                indexData = new IndexData(
                    header,
                    GetStream(header.LENGTH_OF_STRUCT, FileOptions.RandomAccess)
                );
            }
            else
            {
                indexData = new IndexData(
                    header,
                    GetStream(header.LENGTH_OF_STRUCT, FileOptions.RandomAccess),
                    GetStream(8, FileOptions.SequentialScan)
                );
            }

            return indexData;
        }

        private Stream GetStream(int bufferSize, FileOptions fileOptions)
        {
            Stream indexStream = new FileStream(
                _indexFileName,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize,
                fileOptions
            );

            return indexStream;
        }
    }
}