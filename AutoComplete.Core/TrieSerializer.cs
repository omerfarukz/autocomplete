using AutoComplete.Core.DataStructure;
using AutoComplete.Core.Helpers;
using AutoComplete.Core.Readers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoComplete.Core
{
    internal class TrieSerializer
    {
        #region Serialzie

        public static void SerializeHeaderWithJsonSerializer(Stream header, TrieIndexHeader trieIndexHeader)
        {
            using (StreamWriter streamWriter = new StreamWriter(header, Encoding.UTF8, 1024, true)) // TODO: buffer size
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, trieIndexHeader);
                    jsonWriter.Flush();
                }
            }
        }

        private static JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings()
            {
                MaxDepth = Int32.MaxValue
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="trieIndexHeader"></param>
        /// <param name="index"></param>
        /// <remarks>Don't forget to dispose stream</remarks>
        /// <returns></returns>
        public static int SerializeIndexWithBinaryWriter(TrieNode rootNode, TrieIndexHeader trieIndexHeader, Stream index)
        {
            int processedNodeCount = 0;

            Queue<TrieNode> serializerQueue = new Queue<TrieNode>();
            serializerQueue.Enqueue(rootNode);

            TrieNode currentNode = null;
            BinaryWriter binaryWriter = new BinaryWriter(index);

            //uint position = 0;
            //var sb = new StringBuilder();

            while (serializerQueue.Count > 0)
            {
                currentNode = serializerQueue.Dequeue();

                if (currentNode == null)
                    throw new InvalidDataException(string.Format("Value cannot be null ", processedNodeCount));

                long currentPositionOfStream = binaryWriter.BaseStream.Position;

                // write character
                //bw.Write(Encoding.Unicode.GetBytes(node.Character.ToString()));
                UInt16? characterIndex = TrieIndexHeaderCharacterReader.Instance.GetCharacterIndex(trieIndexHeader, currentNode.Character);
                if (characterIndex != null && characterIndex.HasValue)
                {
                    binaryWriter.Write(characterIndex.Value);
                }
                else
                {
                    binaryWriter.Write(Convert.ToUInt16(0)); // Its root
                }

                binaryWriter.Write(currentNode.IsTerminal);

                //if (currentNode.IsTerminal)
                //{
                //    //sb.AppendLine(currentNode.Character);
                //}

                // write children flags
                // convert 512 bool value to 64 byte value for efficient storage
                BitArray baChildren = new BitArray(trieIndexHeader.COUNT_OF_CHARSET);
                if (currentNode.Children != null)
                {
                    foreach (var item in currentNode.Children)
                    {
                        UInt16? itemIndex = TrieIndexHeaderCharacterReader.Instance.GetCharacterIndex(trieIndexHeader, item.Key);
                        baChildren.Set(itemIndex.Value, true);
                    }
                }

                int[] childrenFlags = new int[trieIndexHeader.COUNT_OF_CHILDREN_FLAGS_IN_BYTES];
                BitArrayHelper.CopyToInt32Array(baChildren, childrenFlags, 0);

                for (int i = 0; i < childrenFlags.Length; i++)
                {
                    binaryWriter.Write(childrenFlags[i]);
                }

                // write children offset
                binaryWriter.Write(currentNode.ChildrenCount * trieIndexHeader.LENGTH_OF_STRUCT);

                // todo:position of text file
                if (currentNode.PositionOnTextFile.HasValue)
                {
                    binaryWriter.Write((uint)currentNode.PositionOnTextFile.Value);
                }
                else
                {
                    binaryWriter.Write((uint)0);
                }

                if (currentNode.Children != null)
                {
                    foreach (var childNode in currentNode.Children)
                    {
                        serializerQueue.Enqueue(childNode.Value);
                    }
                }

                ++processedNodeCount;
            }

            return processedNodeCount;
        }

        #endregion

        #region Deserialize

        public static TrieIndexHeader DeserializeHeaderWithXmlSerializer(Stream header)
        {
            return DeserializeHeaderWithXmlSerializer(header, false);
        }

        public static TrieIndexHeader DeserializeHeaderWithXmlSerializer(Stream header, bool dontAutoInitializeCache)
        {
            using (StreamReader streamReader = new StreamReader(header))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var trieIndexHeader = serializer.Deserialize<TrieIndexHeader>(jsonReader);

                    if (!dontAutoInitializeCache)
                        TrieIndexHeaderCharacterReader.Instance.InitCharacterCache(trieIndexHeader);

                    return trieIndexHeader;
                }
            }
        }

        #endregion
    }
}