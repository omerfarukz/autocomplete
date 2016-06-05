using System;
using System.Collections.Generic;

namespace AutoComplete.Core
{
    public class TrieIndexHeader
    {
        private bool _isCharacterIndexCacheInitialized;
        private IDictionary<char, UInt16> _characterIndexDictionary;

        public List<char> CharacterList { get; set; }

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

        #endregion STRUCTURE_PROPERTIES

        public TrieIndexHeader()
        {
            _characterIndexDictionary = new Dictionary<char, UInt16>();

            CharacterList = new List<char>();

            COUNT_OF_CHARACTER_IN_BYTES = 2;
            COUNT_TERMINAL_SIZE_IN_BYTES = 1;
            COUNT_OF_CHILDREN_OFFSET_IN_BYTES = 4;
        }

        /// <summary>
        /// Gets the index of the character.
        /// Returns null when charcacter is not found.
        /// </summary>
        /// <returns>The character index.</returns>
        /// <param name="c">C.</param>
        internal ushort? GetCharacterIndex(char c)
        {
            InitCharacterCache();

            if (!_characterIndexDictionary.ContainsKey(c))
                return null;

            return _characterIndexDictionary[c];
        }

        internal char GetCharacterAtIndex(UInt16 index)
        {
            InitCharacterCache();

            return CharacterList[index];
        }

        internal void InitCharacterCache()
        {
            if (!_isCharacterIndexCacheInitialized)
            {
                lock (this)
                {
                    for (UInt16 i = 0; i < CharacterList.Count; i++)
                    {
                        if (CharacterList[i] == '\0')
                            continue;

                        _characterIndexDictionary.Add(CharacterList[i], i);
                    }

                    _isCharacterIndexCacheInitialized = true;
                } // lock
            } // if
        }
    }
}