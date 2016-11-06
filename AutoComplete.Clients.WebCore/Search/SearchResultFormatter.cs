using AutoComplete.Core.Domain;
using System.Text;

namespace AutoComplete.Clients.WebCore.Search
{
    public static class SearchResultFormatter
    {
        #region FormatResult

        private const string partItems = "{\"items\":[";
        private const string partItemFormat = "\"{0}\"";
        private const string partStatus = "],\"status\":\"{0}\"}}";
        private const char partSeperator = ',';

        private static readonly char[] escapeCharacters = new char[9] { '\"', '\'', '\\', '\b', '\f', '\n', '\r', '\t', '/' };
        private static readonly string[] unescapeStrings = new string[9] { "\\\"", "\\\'", "\\\\", "\\b", "\\f", "\\n", "\\r", "\\t", "\\/" };

        public static string FormatToJson(this SearchResult searchResult)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(partItems);

            if (searchResult != null && searchResult.Items != null)
            {
                for (int i = 0; i < searchResult.Items.Length; i++)
                {
                    string escaped = EscapeJsonCharacters(searchResult.Items[i]);
                    stringBuilder.AppendFormat(partItemFormat, escaped);

                    if (i != searchResult.Items.Length - 1)
                    {
                        stringBuilder.Append(partSeperator);
                    }
                }
            }

            stringBuilder.AppendFormat(partStatus, searchResult.ResultType);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// https://code.google.com/p/json-simple/source/browse/trunk/src/main/java/org/json/simple/JSONValue.java#270
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string EscapeJsonCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            StringBuilder stringBuilder = new StringBuilder();
            char currentChar;
            for (int i = 0; i < input.Length; i++)
            {
                bool characterAppended = false;
                currentChar = input[i];
                for (int j = 0; j < escapeCharacters.Length; j++)
                {
                    if (currentChar.Equals(escapeCharacters[j]))
                    {
                        stringBuilder.Append(unescapeStrings[j]);
                        characterAppended = true;
                        break;
                    }
                }

                if (!characterAppended)
                    stringBuilder.Append(currentChar);
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}
