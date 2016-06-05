using AutoComplete.Core.DataStructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AutoComplete.Core
{
    internal class TrieBinaryReader
    {
        public List<string> GetAutoCompleteNodes(BinaryReader binaryReader, TrieIndexHeader Header, long position, string prefix, int maxItemsCount, List<string> result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            if (result.Count >= maxItemsCount)
                return result;

            char character = ReadCharacter(binaryReader, Header, position);
            bool isTerminal = ReadIsTerminal(binaryReader, Header, position);

            string newPrefix = string.Concat(prefix, character);

            if (isTerminal)
            {
                result.Add(newPrefix);
            }

            long[] children = GetChildrenPositionsFromNode(binaryReader, Header, position);

            if (children != null)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    if (result.Count < maxItemsCount)
                    {
                        GetAutoCompleteNodes(binaryReader, Header, children[i], newPrefix, maxItemsCount, result);
                    }
                }
            }

            return result;
        }

        internal char ReadCharacter(BinaryReader binaryReader, TrieIndexHeader Header, long position)
        {
            binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);
            UInt16 bytes = binaryReader.ReadUInt16();

            return Header.GetCharacterAtIndex(bytes);
        }

        internal bool ReadIsTerminal(BinaryReader binaryReader, TrieIndexHeader Header, long position)
        {
            long targetPosition = position + Header.COUNT_OF_CHARACTER_IN_BYTES; // TODO: constant
            binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            return binaryReader.ReadBoolean();
        }

        internal bool[] ReadChildrenFlags(BinaryReader binaryReader, TrieIndexHeader Header, long position)
        {
            long targetPosition = Header.LENGTH_OF_CHILDREN_FLAGS + position;
            binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            byte[] bitwisedChildren = binaryReader.ReadBytes(Header.COUNT_OF_CHILDREN_FLAGS);

            BitArray bitArray = new BitArray(bitwisedChildren);

            bool[] childrenFlags = new bool[Header.COUNT_OF_CHARSET];
            for (int i = 0; i < childrenFlags.Length; i++)
            {
                childrenFlags[i] = bitArray.Get(i);
            }

            return childrenFlags;
        }

        internal int ReadChildrenOffset(BinaryReader binaryReader, TrieIndexHeader Header, long position)
        {
            long targetPosition = Header.LENGTH_OF_CHILDREN_OFFSET + position;
            binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            return binaryReader.ReadInt32();
        }

        /// <summary>
        /// Gets the node from children and return position of child node.
        /// </summary>
        /// <returns>The node from children.</returns>
        /// <param name="character">Character.</param>
        [Obsolete]
        internal long? GetNodePositionFromChildren(BinaryReader binaryReader, TrieIndexHeader Header, long parentPosition, char character)
        {
            long? offset = GetNodeOffsetFromChildren(binaryReader, Header, parentPosition, character);

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
        internal long? GetNodeOffsetFromChildren(BinaryReader binaryReader, TrieIndexHeader Header, long parentPosition, char character)
        {
            int childrenOffset = ReadChildrenOffset(binaryReader, Header, parentPosition);
            if (childrenOffset == 0) // -1 equals to non-childed parent
                return null;

            bool[] childrenFlags = ReadChildrenFlags(binaryReader, Header, parentPosition);
            BitArray bitArray = new BitArray(childrenFlags);
            ICollection<char> children = GetFlagedChars(bitArray, Header, true);

            IEnumerator<char> iEnumerator = children.GetEnumerator();
            int childIndex = 0;
            while (iEnumerator.MoveNext())
            {
                if (iEnumerator.Current == character)
                {
                    long targetPosition = childrenOffset + (childIndex * Header.LENGTH_OF_STRUCT);
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
        internal long? GetChildPositionFromNode(BinaryReader binaryReader, TrieIndexHeader Header, long position, char character)
        {
            UInt16? childIndex = Header.GetCharacterIndex(character);

            if (childIndex != null && childIndex.HasValue)
            {
                long targetPosition = Header.LENGTH_OF_CHILDREN_FLAGS + position;
                binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

                int bytesCount = (childIndex.Value / 8) + 1;
                byte[] bitwisedChildren = binaryReader.ReadBytes(bytesCount);
                BitArray bitArray = new BitArray(bitwisedChildren);

                if (bitArray.Get(childIndex.Value) == true)
                {
                    UInt16 childOrder = 0;
                    for (int i = 0; i < childIndex.Value; i++)
                    {
                        if (bitArray.Get(i) == true)
                            ++childOrder;
                    }

                    int childrenOffset = ReadChildrenOffset(binaryReader, Header, position);

                    // todo: change variable name targetPosition_2 to x
                    long targetPosition_2 = position + childrenOffset + (childOrder * Header.LENGTH_OF_STRUCT);
                    return targetPosition_2;
                }
            }

            return null;
        }

        internal long[] GetChildrenPositionsFromNode(BinaryReader binaryReader, TrieIndexHeader Header, long parentPosition)
        {
            int childrenOffset = ReadChildrenOffset(binaryReader, Header, parentPosition);
            if (childrenOffset == 0) // -1 equals to non-childed parent
                return null;

            bool[] childrenFlags = ReadChildrenFlags(binaryReader, Header, parentPosition);
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

        internal TrieNodeStructSearchResult GetLastNode(BinaryReader binaryReader, TrieIndexHeader Header, long parentPosition, TrieNodeInput input)
        {
            long currentPosition = parentPosition;

            for (int i = 0; i < input.Keyword.Length; i++)
            {
                long? childPosition = GetChildPositionFromNode(binaryReader, Header, currentPosition, input.Keyword[i]);

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

        internal ICollection<UInt16> GetFlagedCharCodes(BitArray bitArray, TrieIndexHeader Header, bool flag)
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

        [Obsolete]
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

        internal ICollection<char> GetFlagedChars(BitArray bitArray, TrieIndexHeader Header, bool flag)
        {
            ICollection<char> charList = new List<char>();
            ICollection<UInt16> charCodes = GetFlagedCharCodes(bitArray, Header, flag);

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

        private int GetFlaggedCount(bool[] flags, bool flag)
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
    }
}