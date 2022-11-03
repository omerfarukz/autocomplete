using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AutoComplete.DataStructure;
using AutoComplete.Helpers;
using AutoComplete.Readers;

namespace AutoComplete.Serializers
{
    internal static class TrieIndexSerializer
    {
        #region Serializer

        /// <summary>
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="header"></param>
        /// <param name="index"></param>
        /// <remarks>Don't forget to dispose stream</remarks>
        /// <returns></returns>
        public static int Serialize(TrieNode rootNode, TrieIndexHeader header, Stream index)
        {
            var headerReader = new TrieIndexHeaderCharacterReader(header);
            var processedNodeCount = 0;
            var serializerQueue = new Queue<TrieNode>();
            serializerQueue.Enqueue(rootNode);

            var binaryWriter = new BinaryWriter(index);
            while (serializerQueue.Count > 0)
            {
                var currentNode = serializerQueue.Dequeue();
                if (currentNode == null)
                    throw new InvalidDataException("Value cannot be null ");

                // write character
                var characterIndex = headerReader.GetCharacterIndex(currentNode.Character);

                binaryWriter.Write(characterIndex ?? Convert.ToUInt16(0)); // Its root
                binaryWriter.Write(currentNode.IsTerminal);

                SerializeChildren(binaryWriter, header, headerReader, currentNode);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binaryWriter"></param>
        /// <param name="headerReader"></param>
        /// <param name="currentNode"></param>
        private static void SerializeChildren(
            BinaryWriter binaryWriter,
            TrieIndexHeader header,
            TrieIndexHeaderCharacterReader headerReader,
            TrieNode currentNode)
        {
            // write children flags. convert 512 bool value to 64 byte value for efficient storage
            var children = new BitArray(header.COUNT_OF_CHARSET);
            if (currentNode.Children != null)
            {
                foreach (var item in currentNode.Children)
                {
                    var itemIndex = headerReader.GetCharacterIndex(item.Key);
                    children.Set(itemIndex!.Value, true);
                }
            }

            var childrenFlags = new int[header.COUNT_OF_CHILDREN_FLAGS_IN_BYTES];
            children.CopyToInt32Array(childrenFlags, 0);

            foreach (var flag in childrenFlags)
            {
                binaryWriter.Write(flag);
            }
            
            // write children offset
            binaryWriter.Write(currentNode.ChildrenCount * header.LENGTH_OF_STRUCT);
            binaryWriter.Write(currentNode.PositionOnTextFile ?? 0);
        }

        #endregion
    }
}