using System.IO;
using AutoComplete.DataStructure;
using AutoComplete.Serializers;

namespace AutoComplete.Clients.IndexSearchers
{
    internal static class TrieNodeHelperFileSystemExtensions
    {
        public static TrieIndexHeader ReadHeaderFile(string path)
        {
            var serializer = new TrieIndexHeaderSerializer();
            using Stream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );
            return serializer.Deserialize(stream);
        }
    }
}