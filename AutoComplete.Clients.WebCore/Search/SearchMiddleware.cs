using AutoComplete.Client;
using AutoComplete.Core;
using AutoComplete.Core.Domain;
using AutoComplete.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete.Clients.WebCore.Search
{
    public class SearchMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SearchMiddlewareSetting _settings;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SearchMiddleware(
                RequestDelegate next,
                IOptions<SearchMiddlewareSetting> settings,
                IHostingEnvironment hostingEnvironment
            )
        {
            _next = next;
            _settings = settings?.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsValidContext(context))
            {
                SearchOptions searchOptions = GetSearchOptions(context);
                IIndexSearcher searcher = GetSearcher();

                SearchResult searchResult = searcher.Search(searchOptions);
                string formattedResult = FormatSearchResult(searchResult);

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(formattedResult);
            }

            //await _next.Invoke(context);
        }

        private bool IsValidContext(HttpContext context)
        {
            if (context == null)
                return false;

            if (context.Request == null || context.Request.Method != _settings.Method)
            {
                return false;
            }

            if (!context.Request.Query.ContainsKey(_settings.KeywordName))
            {
                return false;
            }

            return true;
        }

        #region GetSearcher

        private IIndexSearcher GetSearcher()
        {
            string headerPath = Path.Combine(_hostingEnvironment.ContentRootPath, "db\\20k_header.json");
            string indexPath = Path.Combine(_hostingEnvironment.ContentRootPath, "db\\20k_index.bin");

            IIndexSearcher searcher = new InMemoryIndexSearcher(headerPath, indexPath);
            return searcher;
        }

        private SearchOptions GetSearchOptions(HttpContext context)
        {
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.MaxItemCount = RequestHelper.ExtractValue<int>(context.Request, RequestHelper.RequestCollectionType.Query, _settings.MaxItemCountName, 5);
            searchOptions.Term = RequestHelper.ExtractValue<string>(context.Request, RequestHelper.RequestCollectionType.Query, _settings.KeywordName, null);
            
            return searchOptions;
        }

        #endregion
        #region FormatResult

        private string FormatSearchResult(SearchResult searchResult)
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

        #endregion
    }
}
