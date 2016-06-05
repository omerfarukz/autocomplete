using AutoComplete.Core;
using System.Text;
using System.Web;

namespace AutoComplete.Web
{
    public abstract class IndexSearcherHttpHandlerBase : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public abstract IIndexSearcher GetSearcher();

        public virtual SearchOptions GetSearchOptions(HttpContext context)
        {
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.MaxItemCount = RequestHelper.Get<int>(context.Request, "m", 5);
            searchOptions.Term = RequestHelper.Get<string>(context.Request, "t", string.Empty);

            return searchOptions;
        }

        public virtual string FormatSearchResult(SearchResult searchResult)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\"items\":[");

            if (searchResult != null && searchResult.Items != null)
            {
                for (int i = 0; i < searchResult.Items.Length; i++)
                {
                    string escaped = EscapeJsonCharacters(searchResult.Items[i]);
                    stringBuilder.AppendFormat("\"{0}\"", escaped);

                    if (i != searchResult.Items.Length - 1)
                    {
                        stringBuilder.Append(',');
                    }
                }
            }

            stringBuilder.AppendFormat("],\"status\":\"{0}\"}}", searchResult.ResultType);

            return stringBuilder.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            SearchOptions searchOptions = GetSearchOptions(context);
            IIndexSearcher searcher = GetSearcher();

            SearchResult searchResult = searcher.Search(searchOptions);
            string formattedResult = FormatSearchResult(searchResult);

            context.Response.ContentType = "text/plain";
            context.Response.Write(formattedResult);
        }

        /// <summary>
        /// https://code.google.com/p/json-simple/source/browse/trunk/src/main/java/org/json/simple/JSONValue.java#270
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string EscapeJsonCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            string[] escapeCharacters = new string[9] { "\"", "\'", "\\", "\b", "\f", "\n", "\r", "\t", "/" };
            string[] unescapeStrings = new string[9] { "\\\"", "\\\'", "\\\\", "\\b", "\\f", "\\n", "\\r", "\\t", "\\/" };

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                bool characterAppended = false;

                // i = current character index of input
                char currentChar = input[i];
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
                    stringBuilder.Append(input[i]);
            }

            return stringBuilder.ToString();
        }
    }
}