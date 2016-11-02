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
        private readonly string _headerFilePath;
        private readonly string _indexFilePath;
        
        public SearchMiddleware(
                RequestDelegate next,
                IOptions<SearchMiddlewareSetting> settings,
                IHostingEnvironment hostingEnvironment
            )
        {
            _next = next;
            _settings = settings?.Value;
            _hostingEnvironment = hostingEnvironment;

            _headerFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, $"{_settings.DbDirectory}\\header.json");
            _indexFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, $"{_settings.DbDirectory}\\index.bin");
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsValidContext(context))
            {
                SearchOptions searchOptions = GetSearchOptions(context);
                IIndexSearcher searcher = GetSearcher();

                SearchResult searchResult = searcher.Search(searchOptions);
                string formattedResult = FormatSearchResult(searchResult);

                context.Response.ContentType = _settings.ResponseContentType;
                await context.Response.WriteAsync(formattedResult);
            }
        }

        private bool IsValidContext(HttpContext context)
        {
            if (
                    context == null ||
                    context.Request == null ||
                    context.Request.Method != _settings.Method ||
                    !context.Request.Query.ContainsKey(_settings.KeywordName)
                )
            {
                return false;
            }
            return true;
        }

        #region GetSearcher;

        private IIndexSearcher GetSearcher()
        {
            IIndexSearcher searcher = null;
            if (_settings.UseMemorySearcher)
            {
                searcher = new InMemoryIndexSearcher(_headerFilePath, _indexFilePath);
            }
            else
            {
                searcher = new FileSystemIndexSearcher(_headerFilePath, _indexFilePath);
            }

            return searcher;
        }

        private SearchOptions GetSearchOptions(HttpContext context)
        {
            var searchOptions = new SearchOptions();
            searchOptions.SuggestWhenFoundStartsWith = _settings.SuggestWhenFoundStartsWith;

            searchOptions.MaxItemCount = RequestHelper.ExtractValue<int>(context.Request, RequestHelper.RequestCollectionType.Query, _settings.MaxItemCountName, 5);
            searchOptions.Term = RequestHelper.ExtractValue<string>(context.Request, RequestHelper.RequestCollectionType.Query, _settings.KeywordName, null);

            return searchOptions;
        }

        #endregion

        #region FormatResult

        const string partA = "{\"items\":[";
        const string partB = "\"{0}\"";
        const string partC = "],\"status\":\"{0}\"}}";
        const char partD = ',';
        private static string FormatSearchResult(SearchResult searchResult)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(partA);

            if (searchResult != null && searchResult.Items != null)
            {
                for (int i = 0; i < searchResult.Items.Length; i++)
                {
                    string escaped = EscapeJsonCharacters(searchResult.Items[i]);
                    stringBuilder.AppendFormat(partB, escaped);

                    if (i != searchResult.Items.Length - 1)
                    {
                        stringBuilder.Append(partD);
                    }
                }
            }

            stringBuilder.AppendFormat(partC, searchResult.ResultType);
            return stringBuilder.ToString();
        }

        static readonly char[] escapeCharacters = new char[9] { '\"', '\'', '\\', '\b', '\f', '\n', '\r', '\t', '/' };
        static readonly string[] unescapeStrings = new string[9] { "\\\"", "\\\'", "\\\\", "\\b", "\\f", "\\n", "\\r", "\\t", "\\/" };

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
