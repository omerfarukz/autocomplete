namespace AutoComplete.Core.DataStructure
{
    internal class TrieNodeStructSearchResult
    {
        public long AbsolutePosition { get; set; }

        // make data type smaller than int32
        /// <summary>
        /// Gets or sets the last index of the found character.
        /// </summary>
        /// <value>The last index of the found character.</value>
        public int LastFoundCharacterIndex { get; set; }

        /// <summary>
        /// Gets or sets the last found node position.
        /// </summary>
        /// <value>The last found node position.</value>
        public long LastFoundNodePosition { get; set; }

        public TrieNodeSearchResultType Status { get; private set; }

        public static TrieNodeStructSearchResult CreateFoundStartsWith(long absolutePosition, int lastFoundCharacterIndex, long lastFoundNodePosition)
        {
            var result = new TrieNodeStructSearchResult();
            result.Status = TrieNodeSearchResultType.FoundStartsWith;
            result.AbsolutePosition = absolutePosition;
            result.LastFoundCharacterIndex = lastFoundCharacterIndex;
            result.LastFoundNodePosition = lastFoundNodePosition;

            return result;
        }

        public static TrieNodeStructSearchResult CreateFoundEquals(long lastFoundNodePosition)
        {
            var result = new TrieNodeStructSearchResult();
            result.Status = TrieNodeSearchResultType.FoundEquals;
            result.LastFoundNodePosition = lastFoundNodePosition;

            return result;
        }

        public static TrieNodeStructSearchResult CreateFoundEquals(long absolutePosition, long lastFoundNodePosition)
        {
            var result = new TrieNodeStructSearchResult();
            result.Status = TrieNodeSearchResultType.FoundEquals;
            result.AbsolutePosition = absolutePosition;
            result.LastFoundNodePosition = lastFoundNodePosition;

            return result;
        }

        public static TrieNodeStructSearchResult CreateNotFound()
        {
            var result = new TrieNodeStructSearchResult();
            result.Status = TrieNodeSearchResultType.NotFound;

            return result;
        }
    }
}