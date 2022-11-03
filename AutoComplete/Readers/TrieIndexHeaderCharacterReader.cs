using System.Collections.Generic;
using AutoComplete.DataStructure;

namespace AutoComplete.Readers
{
    public class TrieIndexHeaderCharacterReader
    {
        private readonly TrieIndexHeader _header;
        private readonly Dictionary<char, ushort> _characterIndex;

        public TrieIndexHeaderCharacterReader(TrieIndexHeader header)
        {
            _header = header;
            _characterIndex = new Dictionary<char, ushort>();
            for (ushort i = 0; i < _header.CharacterList.Count; i++)
            {
                if (_header.CharacterList[i] == '\0')
                    continue;

                if (!_characterIndex.ContainsKey(_header.CharacterList[i]))
                    _characterIndex.Add(_header.CharacterList[i], i);
            }
        }

        /// <summary>
        ///     Gets the index of the character.
        ///     Returns null when character is not found.
        /// </summary>
        /// <returns>The character index.</returns>
        internal ushort? GetCharacterIndex(char character)
        {
            if (!_characterIndex.ContainsKey(character))
                return null;
            return _characterIndex[character];
        }

        internal char GetCharacterAtIndex(ushort index)
        {
            return _header.CharacterList[index];
        }
    }
}