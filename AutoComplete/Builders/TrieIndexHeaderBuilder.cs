using System;
using System.Collections.Generic;
using AutoComplete.DataStructure;

namespace AutoComplete.Builders
{
    internal class TrieIndexHeaderBuilder
    {
        private readonly List<char> _characterList;

        public TrieIndexHeaderBuilder()
        {
            _characterList = new List<char>();
        }

        internal TrieIndexHeaderBuilder AddChar(char character)
        {
            if (!_characterList.Contains(character))
                _characterList.Add(character);

            return this;
        }

        internal TrieIndexHeaderBuilder AddString(string value)
        {
            // warning: value not checked for whitespace by design
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(nameof(value));
            
            foreach (var t in value)
                AddChar(t);

            return this;
        }

        internal TrieIndexHeader Build()
        {
            var header = new TrieIndexHeader();
            header.CharacterList = _characterList;

            // SortCharacterList
            _characterList.Sort(new TrieCharacterComparer());
            
            CalculateMetrics(ref header);

            return header;
        }


        private void CalculateMetrics(ref TrieIndexHeader header)
        {
            // Set structural properties
            header.COUNT_OF_CHARSET = _characterList.Count;

            header.COUNT_OF_CHILDREN_FLAGS = header.COUNT_OF_CHARSET / 8 + (header.COUNT_OF_CHARSET % 8 == 0 ? 0 : 1);
            header.COUNT_OF_CHILDREN_FLAGS_IN_BYTES = header.COUNT_OF_CHARSET / 32 + (header.COUNT_OF_CHARSET % 32 == 0 ? 0 : 1);
            header.COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES = header.COUNT_OF_CHILDREN_FLAGS_IN_BYTES * 4;

            header.LENGTH_OF_CHILDREN_FLAGS = header.COUNT_OF_CHARACTER_IN_BYTES +
                                              header.COUNT_TERMINAL_SIZE_IN_BYTES;

            header.LENGTH_OF_CHILDREN_OFFSET = header.LENGTH_OF_CHILDREN_FLAGS +
                                               header.COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES;

            header.LENGHT_OF_TEXT_FILE_START_POSITION_IN_BYTES = header.LENGTH_OF_CHILDREN_OFFSET +
                                                                 header.COUNT_OF_TEXT_FILE_START_POSITION_IN_BYTES;

            header.LENGTH_OF_STRUCT = header.LENGHT_OF_TEXT_FILE_START_POSITION_IN_BYTES +
                                      header.COUNT_OF_CHILDREN_OFFSET_IN_BYTES;
        }
    }
}