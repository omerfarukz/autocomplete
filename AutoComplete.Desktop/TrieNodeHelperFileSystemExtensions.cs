using AutoComplete.Core;
using AutoComplete.Core.DataStructure;
using System.IO;

namespace AutoComplete.Desktop
{
    internal static class TrieNodeHelperFileSystemExtensions
    {
        public static TrieIndexHeader ReadHeaderFile(this TrieNodeHelper instance, string path, bool autoInitCache)
        {
            Stream stream = new FileStream(
                                path,
                                FileMode.Open,
                                FileAccess.Read,
                                FileShare.Read
                            );

            return instance.ReadHeader(stream, autoInitCache);
        }

        public static void CreateHeaderFile(this TrieNodeHelper instance, string path)
        {
            Stream stream = new FileStream(
                                path,
                                FileMode.OpenOrCreate,
                                FileAccess.Write,
                                FileShare.None
                            );

            instance.CreateHeader(stream);

            stream.Close();
            stream.Dispose();
            stream = null;
        }

        public static int CreateIndexFile(this TrieNodeHelper instance, TrieNode node, string path, int readBufferSizeInBytes)
        {
            Stream stream = new FileStream(
                                path,
                                FileMode.OpenOrCreate,
                                FileAccess.Write,
                                FileShare.None,
                                readBufferSizeInBytes,
                                FileOptions.RandomAccess
                            );

            return instance.CreateIndex(node, stream);
        }
    }
}
