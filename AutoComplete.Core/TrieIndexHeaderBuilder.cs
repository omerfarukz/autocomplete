using AutoComplete.Core.DataStructure;

using System;
using System.Collections.Generic;

namespace AutoComplete.Core
{
    internal class TrieIndexHeaderBuilder
    {
        private IDictionary<char, UInt16> _characterToIndexDictionary;
        private List<char> _characterList;

        public TrieIndexHeaderBuilder()
        {
            _characterList = new List<char>();
            _characterToIndexDictionary = new Dictionary<char, UInt16>();
        }

        internal TrieIndexHeaderBuilder AddChar(char c)
        {
            if (!_characterList.Contains(c))
            {
                _characterList.Add(c);
            }

            return this;
        }

        internal TrieIndexHeaderBuilder AddString(string value)
        {
            // warning: value not checked for whitespace by design
            if (!string.IsNullOrEmpty(value))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    AddChar(value[i]);
                }
            }

            return this;
        }

        internal TrieIndexHeader Build()
        {
            var header = new TrieIndexHeader();
            header.CharacterList = _characterList;

            SortCharacterList(ref header);
            CalculateMetrics(ref header);

            return header;
        }

        private TrieIndexHeaderBuilder SortCharacterList(ref TrieIndexHeader header)
        {
            if (header == null || _characterList == null)
                throw new ArgumentNullException("header");

            _characterList.Sort(new TrieCharacterComparer());

            return this;
        }

        private void CalculateMetrics(ref TrieIndexHeader header)
        {
            // Set structural based properties
            header.COUNT_OF_CHARSET = _characterList.Count;

            header.COUNT_OF_CHILDREN_FLAGS = header.COUNT_OF_CHARSET / 8 + (header.COUNT_OF_CHARSET % 8 == 0 ? 0 : 1);
            header.COUNT_OF_CHILDREN_FLAGS_IN_BYTES = header.COUNT_OF_CHARSET / 32 + (header.COUNT_OF_CHARSET % 32 == 0 ? 0 : 1);
            header.COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES = header.COUNT_OF_CHILDREN_FLAGS_IN_BYTES * 4;

            header.LENGTH_OF_CHILDREN_FLAGS = header.COUNT_OF_CHARACTER_IN_BYTES + // 2
                                                header.COUNT_TERMINAL_SIZE_IN_BYTES; // 1;

            header.LENGTH_OF_CHILDREN_OFFSET = header.LENGTH_OF_CHILDREN_FLAGS + // 2
                                        header.COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES;

            header.LENGTH_OF_STRUCT = header.LENGTH_OF_CHILDREN_OFFSET +
                                header.COUNT_OF_CHILDREN_OFFSET_IN_BYTES;
                                     
        }
    }
}