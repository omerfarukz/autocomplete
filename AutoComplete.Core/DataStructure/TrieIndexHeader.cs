using AutoComplete.Core.DataStructure;

using System;
using System.Collections.Generic;

namespace AutoComplete.Core
{
    public class TrieIndexHeader
    {
        private IDictionary<char, UInt16> _charToIndexDictionary;

        public List<char> CharacterList { get; private set; }

        #region STRUCTURE_PROPERTIES

        public int COUNT_OF_CHARACTER_IN_BYTES { get; set; }

        public int COUNT_TERMINAL_SIZE_IN_BYTES { get; set; }

        public int COUNT_OF_CHILDREN_OFFSET_IN_BYTES { get; set; }

        public int COUNT_OF_CHARSET { get; set; }

        public int COUNT_OF_CHILDREN_FLAGS { get; set; }

        public int COUNT_OF_CHILDREN_FLAGS_IN_BYTES { get; set; }

        public int COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES { get; set; }

        public int LENGTH_OF_STRUCT { get; set; }

        public int LENGTH_OF_CHILDREN_FLAGS { get; set; }

        public int LENGTH_OF_CHILDREN_OFFSET { get; set; }

        #endregion

        public TrieIndexHeader()
        {
            _charToIndexDictionary = new Dictionary<char, UInt16>();

            CharacterList = new List<char>();

            COUNT_OF_CHARACTER_IN_BYTES = 2;
            COUNT_TERMINAL_SIZE_IN_BYTES = 1;
            COUNT_OF_CHILDREN_OFFSET_IN_BYTES = 4;
        }

        public void CalculateMetrics()
        {
            //Map chars-index
            InitCharacterCache();

            // Set structural based properties
            COUNT_OF_CHARSET = CharacterList.Count;

            COUNT_OF_CHILDREN_FLAGS = COUNT_OF_CHARSET / 8 + (COUNT_OF_CHARSET % 8 == 0 ? 0 : 1);
            COUNT_OF_CHILDREN_FLAGS_IN_BYTES = COUNT_OF_CHARSET / 32 + (COUNT_OF_CHARSET % 32 == 0 ? 0 : 1);
            COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES = COUNT_OF_CHILDREN_FLAGS_IN_BYTES * 4;

            LENGTH_OF_STRUCT = COUNT_OF_CHARACTER_IN_BYTES +
            COUNT_TERMINAL_SIZE_IN_BYTES +
            COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES +
            COUNT_OF_CHILDREN_OFFSET_IN_BYTES;

            LENGTH_OF_CHILDREN_FLAGS = COUNT_OF_CHARACTER_IN_BYTES + // 2
            COUNT_TERMINAL_SIZE_IN_BYTES;

            LENGTH_OF_CHILDREN_OFFSET = COUNT_OF_CHARACTER_IN_BYTES + // 2
            COUNT_TERMINAL_SIZE_IN_BYTES + // 1
            COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES;
        }

        internal void InitCharacterCache()
        {
            for (UInt16 i = 0; i < CharacterList.Count; i++)
            {
                if (CharacterList[i] == '\0')
                    continue;

                _charToIndexDictionary.Add(CharacterList[i], i);
            }
        }

        internal void AddChar(char c)
        {
            if (!CharacterList.Contains(c))
            {
                if (CharacterList.Contains(c))
                    return;

                CharacterList.Add(c);
            }
        }

        internal void AddString(string value)
        {
            // warning: value not checked for whitespace
            if (string.IsNullOrEmpty(value))
                return;

            for (int i = 0; i < value.Length; i++)
            {
                AddChar(value[i]);
            }
        }

        /// <summary>
        /// Gets the index of the character.
        /// Returns null when charcacter is not found.
        /// </summary>
        /// <returns>The character index.</returns>
        /// <param name="c">C.</param>
        internal ushort? GetCharacterIndex(char c)
        {
            if (!_charToIndexDictionary.ContainsKey(c))
                return null;

            return _charToIndexDictionary[c];
        }

        internal char GetCharacterAtIndex(UInt16 index)
        {
            return CharacterList[index];
        }

        internal void Sort()
        {
            CharacterList.Sort(new TrieCharacterComparer());
        }
    }
}

