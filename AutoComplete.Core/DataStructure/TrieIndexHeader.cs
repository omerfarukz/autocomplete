using System;
using System.Collections.Generic;

namespace AutoComplete.Core
{
    public class TrieIndexHeader
    {
        public List<char> CharacterList { get; set; }

        #region STRUCTURE_PROPERTIES

        public int COUNT_OF_CHARACTER_IN_BYTES { get; set; }

        public int COUNT_TERMINAL_SIZE_IN_BYTES { get; set; }

        public int COUNT_OF_CHILDREN_OFFSET_IN_BYTES { get; set; }

        public int COUNT_OF_CHARSET { get; set; }

        public int COUNT_OF_CHILDREN_FLAGS { get; set; }

        public int COUNT_OF_CHILDREN_FLAGS_IN_BYTES { get; set; }

        public int COUNT_OF_CHILDREN_FLAGS_BIT_ARRAY_IN_BYTES { get; set; }

        public int COUNT_OF_TEXT_FILE_START_POSITION_IN_BYTES { get; set; }

        public int LENGTH_OF_STRUCT { get; set; }

        public int LENGTH_OF_CHILDREN_FLAGS { get; set; }

        public int LENGTH_OF_CHILDREN_OFFSET { get; set; }

        public int LENGHT_OF_TEXT_FILE_START_POSITION_IN_BYTES { get; set; }
        
        #endregion STRUCTURE_PROPERTIES

        public TrieIndexHeader()
        {
            CharacterList = new List<char>();

            COUNT_OF_CHARACTER_IN_BYTES = 2;
            COUNT_TERMINAL_SIZE_IN_BYTES = 1;
            COUNT_OF_CHILDREN_OFFSET_IN_BYTES = 4;
            COUNT_OF_TEXT_FILE_START_POSITION_IN_BYTES = 4;
        }
    }
}