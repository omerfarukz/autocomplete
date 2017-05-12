using AutoComplete.Core;
using AutoComplete.Core.DataStructure;
using AutoComplete.Core.Readers;
using System.IO;
using AutoComplete.Core.Serializers;

namespace AutoComplete.Clients.IndexSearchers
{
    internal static class TrieNodeHelperFileSystemExtensions
    {
        public static TrieIndexHeader ReadHeaderFile(string path)
        {
            var serializer = new TrieIndexHeaderSerializer();

            using (Stream stream = new FileStream(
                                path,
                                FileMode.Open,
                                FileAccess.Read,
                                FileShare.Read
            ))
            {
                return serializer.Deserialize(stream);
            }
        }

        public static void CreateHeaderFile(this TrieIndexHeader header, string path)
        {
            var serializer = new TrieIndexHeaderSerializer();
            using (Stream stream = new FileStream(
                                path,
                                FileMode.OpenOrCreate,
                                FileAccess.Write,
                                FileShare.None
            ))
            {
                serializer.Serialize(stream, header);
            }
        }

        public static int CreateIndexFile(this TrieBinaryReader instance, TrieIndexHeader header, TrieNode node, string path, int readBufferSizeInBytes)
        {
            Stream stream = new FileStream(
                                path,
                                FileMode.OpenOrCreate,
                                FileAccess.Write,
                                FileShare.None,
                                readBufferSizeInBytes,
                                FileOptions.RandomAccess
                            );

            return TrieIndexSerializer.Serialize(node, header, stream);
        }
    }
}