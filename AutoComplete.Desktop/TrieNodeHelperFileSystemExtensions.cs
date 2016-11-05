using AutoComplete.Core;
using AutoComplete.Core.DataStructure;
using AutoComplete.Core.Helpers;
using System.IO;

namespace AutoComplete.Desktop
{
    internal static class TrieNodeHelperFileSystemExtensions
    {
        public static TrieIndexHeader ReadHeaderFile(string path)
        {
            Stream stream = new FileStream(
                                path,
                                FileMode.Open,
                                FileAccess.Read,
                                FileShare.Read
                            );

            return TrieSerializer.DeserializeHeaderWithXmlSerializer(stream, false);
        }

        public static void CreateHeaderFile(this TrieIndexHeader header, string path)
        {
            Stream stream = new FileStream(
                                path,
                                FileMode.OpenOrCreate,
                                FileAccess.Write,
                                FileShare.None
                            );

            TrieSerializer.SerializeHeaderWithJsonSerializer(stream, header);

            stream.Close();
            stream.Dispose();
            stream = null;
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

            return TrieSerializer.SerializeIndexWithBinaryWriter(node, header, stream);
        }
    }
}