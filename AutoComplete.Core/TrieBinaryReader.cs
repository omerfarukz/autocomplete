using AutoComplete.Core.DataStructure;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AutoComplete.Core
{
    internal class TrieBinaryReader
    {
        private BinaryReader _binaryReader;
        private TrieIndexHeader _header;

        public TrieBinaryReader(BinaryReader binaryReader, TrieIndexHeader header)
        {
            _binaryReader = binaryReader;
            _header = header;
        }

        public List<string> GetAutoCompleteNodes(long position, string prefix, int maxItemsCount, List<string> result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            if (result.Count >= maxItemsCount)
                return result;

            char character = ReadCharacter(position);
            bool isTerminal = ReadIsTerminal(position);

            string newPrefix = string.Concat(prefix, character);

            if (isTerminal)
            {
                result.Add(newPrefix);
            }

            long[] children = GetChildrenPositionsFromNode(_binaryReader, _header, position);

            if (children != null)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    if (result.Count < maxItemsCount)
                    {
                        GetAutoCompleteNodes(children[i], newPrefix, maxItemsCount, result);
                    }
                }
            }

            return result;
        }

        internal char ReadCharacter(long position)
        {
            _binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);
            UInt16 bytes = _binaryReader.ReadUInt16();

            return TrieIndexHeaderCharacterReader.Instance.GetCharacterAtIndex(_header, bytes);
        }

        internal bool ReadIsTerminal(long position)
        {
            long targetPosition = position + _header.COUNT_OF_CHARACTER_IN_BYTES; // TODO: constant
           _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            return _binaryReader.ReadBoolean();
        }

        internal bool[] ReadChildrenFlags(long position)
        {
            long targetPosition = _header.LENGTH_OF_CHILDREN_FLAGS + position;
            _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            byte[] bitwisedChildren = _binaryReader.ReadBytes(_header.COUNT_OF_CHILDREN_FLAGS);

            BitArray bitArray = new BitArray(bitwisedChildren);

            bool[] childrenFlags = new bool[_header.COUNT_OF_CHARSET];
            for (int i = 0; i < childrenFlags.Length; i++)
            {
                childrenFlags[i] = bitArray.Get(i);
            }

            return childrenFlags;
        }

        internal int ReadChildrenOffset(long position)
        {
            long targetPosition = _header.LENGTH_OF_CHILDREN_OFFSET + position;
            _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            return _binaryReader.ReadInt32();
        }

        /// <summary>
        /// Gets the node from children and return position of child node.
        /// </summary>
        /// <returns>The node from children.</returns>
        /// <param name="character">Character.</param>
        [Obsolete]
        internal long? GetNodePositionFromChildren(long parentPosition, char character)
        {
            long? offset = GetNodeOffsetFromChildren(parentPosition, character);

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
        /// <param name="_binaryReader">Binary reader.</param>
        /// <param name="parentStartPosition">Parent start position.</param>
        /// <param name="character">Character.</param>
        internal long? GetNodeOffsetFromChildren(long parentPosition, char character)
        {
            int childrenOffset = ReadChildrenOffset(parentPosition);
            if (childrenOffset == 0) // -1 equals to non-childed parent
                return null;

            bool[] childrenFlags = ReadChildrenFlags(parentPosition);
            BitArray bitArray = new BitArray(childrenFlags);
            ICollection<char> children = GetFlagedChars(bitArray, _header, true);

            IEnumerator<char> iEnumerator = children.GetEnumerator();
            int childIndex = 0;
            while (iEnumerator.MoveNext())
            {
                if (iEnumerator.Current == character)
                {
                    long targetPosition = childrenOffset + (childIndex * _header.LENGTH_OF_STRUCT);
                    return targetPosition;
                }

                ++childIndex;
            }

            return null;
        }

        /// <summary>
        /// Its like a GetNodeOffsetFromChildren but faster then 4 times
        /// </summary>
        /// <param name="_binaryReader"></param>
        /// <param name="position"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        internal long? GetChildPositionFromNode(long position, char character)
        {
            UInt16? childIndex = TrieIndexHeaderCharacterReader.Instance.GetCharacterIndex(_header, character);

            if (childIndex != null && childIndex.HasValue)
            {
                long targetPosition = _header.LENGTH_OF_CHILDREN_FLAGS + position;
                _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

                int bytesCount = (childIndex.Value / 8) + 1;
                byte[] bitwisedChildren = _binaryReader.ReadBytes(bytesCount);
                BitArray bitArray = new BitArray(bitwisedChildren);

                if (bitArray.Get(childIndex.Value) == true)
                {
                    UInt16 childOrder = 0;
                    for (int i = 0; i < childIndex.Value; i++)
                    {
                        if (bitArray.Get(i) == true)
                            ++childOrder;
                    }

                    int childrenOffset = ReadChildrenOffset(position);

                    // todo: change variable name targetPosition_2 to x
                    long targetPosition_2 = position + childrenOffset + (childOrder * _header.LENGTH_OF_STRUCT);
                    return targetPosition_2;
                }
            }

            return null;
        }

        internal long[] GetChildrenPositionsFromNode(BinaryReader binaryReader, TrieIndexHeader Header, long parentPosition)
        {
            int childrenOffset = ReadChildrenOffset(parentPosition);
            if (childrenOffset == 0) // -1 equals to non-childed parent
                return null;

            bool[] childrenFlags = ReadChildrenFlags(parentPosition);
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

        internal TrieNodeStructSearchResult GetLastNode(long parentPosition, TrieNodeInput input)
        {
            var result = TrieNodeStructSearchResult.CreateNotFound();

            long currentPosition = parentPosition;

            for (int i = 0; i < input.Keyword.Length; i++)
            {
                long? childPosition = GetChildPositionFromNode(currentPosition, input.Keyword[i]);

                if (childPosition != null)
                {
                    if (i == input.Keyword.Length - 1)
                    {
                        result = TrieNodeStructSearchResult.CreateFoundEquals(childPosition.Value);
                        break;
                    }

                    currentPosition = childPosition.Value;
                    continue;
                }
                else
                {
                    if (i != 0)
                    {
                        result = TrieNodeStructSearchResult.CreateFoundStartsWith(currentPosition, i, currentPosition);
                    }

                    break;
                }
            }

            return result;
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
                    char currentCharacter = TrieIndexHeaderCharacterReader.Instance.GetCharacterAtIndex(Header, item);
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