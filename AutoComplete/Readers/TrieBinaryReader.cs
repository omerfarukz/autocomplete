using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoComplete.DataStructure;

namespace AutoComplete.Readers
{
    internal class TrieBinaryReader
    {
        private readonly BinaryReader _binaryReader;
        private readonly TrieIndexHeader _header;
        private readonly TrieIndexHeaderCharacterReader _headerReader;

        public TrieBinaryReader(BinaryReader binaryReader, TrieIndexHeader header)
        {
            _binaryReader = binaryReader;
            _header = header;
            _headerReader = new TrieIndexHeaderCharacterReader(header);
        }

        public IEnumerable<string> GetAutoCompleteNodes(
            long position,
            string prefix,
            int maxItems
        )
        {
            return GetAutoCompleteNodesInternal(position, prefix, maxItems, new List<string>());
        }
        
        public IEnumerable<string> GetAutoCompleteNodes(
            long position,
            string prefix,
            int maxItems,
            Stream tail
        )
        {
            return new List<string>(GetAutoCompleteNodesWithTail(position, tail, maxItems));
        }

        private List<string> GetAutoCompleteNodesInternal(long position, string prefix, int maxItems, List<string> results)
        {
            var character = ReadCharacter(position);
            var isTerminal = ReadIsTerminal(position);
            
            var newPrefix = string.Concat(prefix, character);
            if (isTerminal)
                results.Add(newPrefix);

            var children = GetChildrenPositionsFromNode(_header, position);
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (results.Count < maxItems)
                    {
                        GetAutoCompleteNodesInternal(child, newPrefix, maxItems, results);
                    }
                }
            }

            return results;
        }

        private IEnumerable<string> GetAutoCompleteNodesWithTail(long position, Stream tail, int count)
        {
            var positionOnTextFile = ReadPositionOnTextFile(position);
            const int bufferSize = 8;
            using var streamReader = new StreamReader(tail, Encoding.UTF8, false, bufferSize, true);
            streamReader.BaseStream.Seek(positionOnTextFile, SeekOrigin.Begin);

            var firstRead = true;
            for (var i = 0; i < count; i++)
            {
                var line = streamReader.ReadLine()!;
                if (firstRead)
                {
                    count = Math.Min(count,  int.Parse(line[..10]));
                    firstRead = false;
                }
                
                yield return line[11..];
            }
        }

        private char ReadCharacter(long position)
        {
            _binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);
            var bytes = _binaryReader.ReadUInt16();

            return _headerReader.GetCharacterAtIndex(bytes);
        }

        private bool ReadIsTerminal(long position)
        {
            var targetPosition = position + _header.COUNT_OF_CHARACTER_IN_BYTES;
            _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            return _binaryReader.ReadBoolean();
        }

        private uint ReadPositionOnTextFile(long position)
        {
            var targetPosition = position + _header.LENGHT_OF_TEXT_FILE_START_POSITION_IN_BYTES;
            _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            return _binaryReader.ReadUInt32();
        }

        private bool[] ReadChildrenFlags(long position)
        {
            var targetPosition = _header.LENGTH_OF_CHILDREN_FLAGS + position;
            _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            var children = _binaryReader.ReadBytes(_header.COUNT_OF_CHILDREN_FLAGS);
            var bitArray = new BitArray(children);
            var childrenFlags = new bool[_header.COUNT_OF_CHARSET];
            for (var i = 0; i < childrenFlags.Length; i++)
            {
                childrenFlags[i] = bitArray.Get(i);
            }

            return childrenFlags;
        }

        private int ReadChildrenOffset(long position)
        {
            var targetPosition = _header.LENGTH_OF_CHILDREN_OFFSET + position;
            _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

            return _binaryReader.ReadInt32();
        }

        /// <summary>
        ///     Gets the child position from node.
        /// </summary>
        /// <returns>The child position from node.</returns>
        /// <param name="position">Position.</param>
        /// <param name="character">Character.</param>
        private long? GetChildPositionFromNode(long position, char character)
        {
            var childIndex = _headerReader.GetCharacterIndex(character);
            if (childIndex.HasValue)
            {
                var targetPosition = _header.LENGTH_OF_CHILDREN_FLAGS + position;
                _binaryReader.BaseStream.Seek(targetPosition, SeekOrigin.Begin);

                var bytesCount = childIndex.Value / 8 + 1;
                var bitwiseChildren = _binaryReader.ReadBytes(bytesCount);
                var bitArray = new BitArray(bitwiseChildren);

                if (bitArray.Get(childIndex.Value))
                {
                    ushort childOrder = 0;
                    for (var i = 0; i < childIndex.Value; i++)
                    {
                        if (bitArray.Get(i))
                            ++childOrder;
                    }

                    var childrenOffset = ReadChildrenOffset(position);
                    var newPosition = position + childrenOffset + childOrder * _header.LENGTH_OF_STRUCT;
                    return newPosition;
                }
            }

            return null;
        }

        private long[] GetChildrenPositionsFromNode(TrieIndexHeader header, long parentPosition)
        {
            var childrenOffset = ReadChildrenOffset(parentPosition);
            if (childrenOffset == 0) // -1 equals to non-childed parent
                return Array.Empty<long>();

            var childrenFlags = ReadChildrenFlags(parentPosition);
            var childrenCount = GetFlaggedCount(childrenFlags, true);
            var childrenPositions = new long[childrenCount];

            for (var i = 0; i < childrenCount; i++)
            {
                var targetPosition = parentPosition +
                                     childrenOffset +
                                     i * header.LENGTH_OF_STRUCT;

                childrenPositions[i] = targetPosition;
            }

            return childrenPositions;
        }

        internal TrieNodeStructSearchResult SearchLastNode(long parentPosition, string keyword)
        {
            var result = TrieNodeStructSearchResult.CreateNotFound();
            var currentPosition = parentPosition;
            
            for (var i = 0; i < keyword.Length; i++)
            {
                var childPosition = GetChildPositionFromNode(currentPosition, keyword[i]);

                if (childPosition != null)
                {
                    if (i == keyword.Length - 1)
                    {
                        result = TrieNodeStructSearchResult.CreateFoundEquals(childPosition.Value);
                        break;
                    }

                    currentPosition = childPosition.Value;
                    continue;
                }

                if (i != 0)
                {
                    result = TrieNodeStructSearchResult.CreateFoundStartsWith(currentPosition, i, currentPosition);
                }

                break;
            }

            return result;
        }

        private int GetFlaggedCount(bool[] flags, bool flag)
        {
            var count = 0;
            foreach (var t in flags)
            {
                if (t == flag)
                {
                    ++count;
                }
            }

            return count;
        }
    }
}