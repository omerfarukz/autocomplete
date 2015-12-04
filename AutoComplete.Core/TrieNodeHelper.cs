using AutoComplete.Core.DataStructure;
using AutoComplete.Core.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AutoComplete.Core
{
    internal class TrieNodeHelper
    {
        private const bool LOCK_ON_READ = false;

        private static object lock_object = new object();

        public TrieIndexHeader Header;

        internal void ReorderTrieAndLoadHeader(TrieNode node)
        {
            TrieIndexHeader header = new TrieIndexHeader();

            Queue<TrieNode> indexerQueue = new Queue<TrieNode>();

            int order = 0;

            while (node != null)
            {
                node.Order = order;
                header.AddChar(node.Character);

                // set parent's children index when current node's child 
                // index not equal to zero and current index is not the root
                if (node.Parent != null && node.ChildIndex == 0)
                {
                    node.Parent.ChildrenCount = (node.Order - node.Parent.Order);
                }

                if (node.Children != null)
                {
                    int childIndex = 0;

                    foreach (var childNode in node.Children)
                    {
                        childNode.Value.ChildIndex = childIndex++;
                        indexerQueue.Enqueue(childNode.Value);
                    }
                }

                ++order;

                if (indexerQueue.Count == 0)
                    break;

                node = indexerQueue.Dequeue();
            }

            this.Header = header;
            this.Header.Sort();
            this.Header.CalculateMetrics();
        }

        public TrieIndexHeader ReadHeader(Stream stream, bool autoInitCache)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TrieIndexHeader));

            var result = (TrieIndexHeader)serializer.Deserialize(stream);

            if (autoInitCache)
            {
                result.InitCharacterCache();
            }

            return result;
        }

        public void CreateHeader(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TrieIndexHeader));
            serializer.Serialize(stream, Header);
        }

        public int CreateIndex(TrieNode node, Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            Queue<TrieNode> serializerQueue = new Queue<TrieNode>();

            // using for unit testing
            int processedNodeCount = 0;

            while (node != null)
            {
                ++processedNodeCount;

                long currentPositionOfStream = bw.BaseStream.Position;

                // write character
                //bw.Write(Encoding.Unicode.GetBytes(node.Character.ToString()));
                UInt16? characterIndex = Header.GetCharacterIndex(node.Character);
                if (characterIndex != null && characterIndex.HasValue)
                    bw.Write(characterIndex.Value);
                else
                    bw.Write(Convert.ToUInt16(0)); // Its root

                bw.Write(node.IsTerminal);

                // write children flags
                // convert 512 bool value to 64 byte value for efficient storage 
                BitArray baChildren = new BitArray(Header.COUNT_OF_CHARSET);

                if (node.Children != null)
                {
                    foreach (var item in node.Children)
                    {
                        UInt16? itemIndex = Header.GetCharacterIndex(item.Key);

                        baChildren.Set(itemIndex.Value, true);
                    }
                }

                int[] childrenFlags = new int[Header.COUNT_OF_CHILDREN_FLAGS_IN_BYTES];
                baChildren.CopyToInt32Array(childrenFlags, 0);

                for (int i = 0; i < childrenFlags.Length; i++)
                {
                    bw.Write(childrenFlags[i]);
                }

                // write children offset
                bw.Write(node.ChildrenCount * Header.LENGTH_OF_STRUCT);

                if (node.Children != null)
                {
                    foreach (var childNode in node.Children)
                    {
                        serializerQueue.Enqueue(childNode.Value);
                    }
                }

                if (serializerQueue.Count == 0)
                    break;

                node = serializerQueue.Dequeue();
            }
            
            stream.Dispose();
            stream = null;

            return processedNodeCount;
        }

        #region BinaryReader

        public List<string> GetAutoCompleteNodes(BinaryReader binaryReader, long position, string prefix, int maxItemsCount, List<string> result)
        {
            if (result.Count >= maxItemsCount)
                return result;

            char character = ReadCharacter(binaryReader, position);
            bool isTerminal = ReadIsTerminal(binaryReader, position);

            string newPrefix = string.Concat(prefix, character);

            if (isTerminal)
            {
                result.Add(newPrefix);
            }

            long[] children = GetChildrenPositionsFromNode(binaryReader, position);

            if (children != null)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    if (result.Count < maxItemsCount)
                    {
                        GetAutoCompleteNodes(binaryReader, children[i], newPrefix, maxItemsCount, result);
                    }
                }
            }

            return result;
        }

        internal char ReadCharacter(BinaryReader binaryReader, long position)
        {
            UInt16 bytes = 0;

            InvokeHelper.InvokeWithLock(LOCK_ON_READ, lock_object, () =>
                {
                    binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);
                    bytes = binaryReader.ReadUInt16();
                });

            return Header.GetCharacterAtIndex(bytes);
        }

        internal bool ReadIsTerminal(BinaryReader binaryReader, long position)
        {
            long targetPosition = position + Header.COUNT_OF_CHARACTER_IN_BYTES; // TODO: constant
            bool value = false;

            InvokeHelper.InvokeWithLock(LOCK_ON_READ, lock_object, () =>
                {
                    binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);
                    value = binaryReader.ReadBoolean();
                });

            return value;
        }

        internal bool[] ReadChildrenFlags(BinaryReader binaryReader, long position)
        {
            long targetPosition = Header.LENGTH_OF_CHILDREN_FLAGS + position;

            byte[] bitwisedChildren = null;

            InvokeHelper.InvokeWithLock(LOCK_ON_READ, lock_object, () =>
                {
                    binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);
                    bitwisedChildren = binaryReader.ReadBytes(Header.COUNT_OF_CHILDREN_FLAGS);
                });

            BitArray bitArray = new BitArray(bitwisedChildren);

            bool[] childrenFlags = new bool[Header.COUNT_OF_CHARSET];
            //bitArray.CopyTo(childrenFlags, 0);
            for (int i = 0; i < childrenFlags.Length; i++)
            {
                childrenFlags[i] = bitArray.Get(i);
            }

            return childrenFlags;
        }

        internal int ReadChildrenOffset(BinaryReader binaryReader, long position)
        {
            long targetPosition = Header.LENGTH_OF_CHILDREN_OFFSET + position;

            Int32 value = 0;

            InvokeHelper.InvokeWithLock(LOCK_ON_READ, lock_object, () =>
                {
                    binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);
                    value = binaryReader.ReadInt32();
                });

            return value;
        }

        /// <summary>
        /// Gets the node from children and return position of child node.
        /// </summary>
        /// <returns>The node from children.</returns>
        /// <param name="character">Character.</param>
        internal long? GetNodePositionFromChildren(BinaryReader binaryReader, long parentPosition, char character)
        {
            long? offset = GetNodeOffsetFromChildren(binaryReader, parentPosition, character);

            if (offset != null && offset.HasValue)
            {
                offset += parentPosition;
            }

            return offset;
        }

        /// <summary>
        /// Gets the node position from children.
        /// </summary>
        /// <returns>The node position from children.</returns>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="parentStartPosition">Parent start position.</param>
        /// <param name="character">Character.</param>
        internal long? GetNodeOffsetFromChildren(BinaryReader binaryReader, long parentPosition, char character)
        {
            int childrenOffset = ReadChildrenOffset(binaryReader, parentPosition);
            if (childrenOffset == 0) // -1 equals to non-childed parent
                return null;

            bool[] childrenFlags = ReadChildrenFlags(binaryReader, parentPosition);

            BitArray bitArray = new BitArray(childrenFlags);

            ICollection<char> children = GetFlagedChars(bitArray, true);

            IEnumerator<char> iEnumerator = children.GetEnumerator();
            int childIndex = 0;
            while (iEnumerator.MoveNext())
            {
                if (iEnumerator.Current == character)
                {
                    long targetPosition = childrenOffset +
                           (childIndex * Header.LENGTH_OF_STRUCT);

                    return targetPosition;
                }

                ++childIndex;
            }

            return null;
        }

        /// <summary>
        /// Its like a GetNodeOffsetFromChildren but faster then 4 times
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="position"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        internal long? GetChildPositionFromNode(BinaryReader binaryReader, long position, char character)
        {
            UInt16? childIndex = Header.GetCharacterIndex(character);

            if (childIndex != null && childIndex.HasValue)
            {
                int bytesCount = (childIndex.Value / 8) + 1;

                long targetPosition = Header.LENGTH_OF_CHILDREN_FLAGS + position;
                byte[] bitwisedChildren;

                binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);
                bitwisedChildren = binaryReader.ReadBytes(bytesCount);

                BitArray bitArray = new BitArray(bitwisedChildren);

                if (bitArray.Get(childIndex.Value) == true)
                {
                    UInt16 childOrder = 0;
                    for (int i = 0; i < childIndex.Value; i++)
                    {
                        if (bitArray.Get(i) == true)
                            ++childOrder;
                    }

                    int childrenOffset = ReadChildrenOffset(binaryReader, position);

                    long targetPosition_2 = position +
                             childrenOffset +
                             (childOrder * Header.LENGTH_OF_STRUCT);

                    return targetPosition_2;
                }
            }

            return null;
        }

        internal long[] GetChildrenPositionsFromNode(BinaryReader binaryReader, long parentPosition)
        {
            int childrenOffset = ReadChildrenOffset(binaryReader, parentPosition);
            if (childrenOffset == 0) // -1 equals to non-childed parent
                return null;

            bool[] childrenFlags = ReadChildrenFlags(binaryReader, parentPosition);

            int childrenCount = GetFlaggedCount(childrenFlags, true);

            long[] childrenPositions = new long[childrenCount];

            for (int i = 0; i < childrenCount; i++)
            {
                long targetPosition = parentPosition +
                          childrenOffset +
                          (i * Header.LENGTH_OF_STRUCT);

                childrenPositions[i] = targetPosition;
            }

            return childrenPositions;
        }

        internal TrieNodeStructSearchResult GetLastNode(BinaryReader binaryReader, long parentPosition, TrieNodeInput input)
        {
            long currentPosition = parentPosition;

            for (int i = 0; i < input.Keyword.Length; i++)
            {
                long? childPosition = GetChildPositionFromNode(binaryReader, currentPosition, input.Keyword[i]);

                if (childPosition != null)
                {
                    if (i == input.Keyword.Length - 1)
                    {
                        return TrieNodeStructSearchResult.CreateFoundEquals(childPosition.Value);
                    }

                    currentPosition = childPosition.Value;
                    continue;
                }
                else
                {
                    if (i == 0)
                        return TrieNodeStructSearchResult.CreateNotFound();

                    return TrieNodeStructSearchResult.CreateFoundStartsWith(currentPosition, i, currentPosition);
                }
            }

            throw new Exception();
        }

        internal ICollection<UInt16> GetFlagedCharCodes(BitArray bitArray, bool flag)
        {
            ICollection<UInt16> charCodeList = new List<UInt16>(Header.COUNT_OF_CHILDREN_FLAGS_IN_BYTES); // TODO: use different constant

            for (UInt16 i = 0; i < bitArray.Length; i++)
            {
                if (bitArray.Get(i) == flag)
                {
                    charCodeList.Add(i); // TODO: use mapping
                }
            }

            return charCodeList;
        }

        internal ICollection<char> GetFlagedChars(BitArray bitArray, bool flag)
        {
            ICollection<UInt16> charCodes = GetFlagedCharCodes(bitArray, flag);
            ICollection<char> charList = new List<char>();

            if (charCodes != null)
            {
                foreach (UInt16 item in charCodes)
                {
                    char currentCharacter = Header.GetCharacterAtIndex(item);
                    charList.Add(currentCharacter);
                }
            }

            return charList;
        }

        internal int GetFlaggedCount(BitArray bitArray, bool flag)
        {
            int count = 0;
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray.Get(i) == flag)
                {
                    ++count;
                }
            }

            return count;
        }

        internal int GetFlaggedCount(bool[] flags, bool flag)
        {
            int count = 0;
            for (int i = 0; i < flags.Length; i++)
            {
                if (flags[i] == flag)
                {
                    ++count;
                }
            }

            return count;
        }

        #endregion

    }
}