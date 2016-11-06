using AutoComplete.Client;
using AutoComplete.Core;
using AutoComplete.Core.Domain;
using AutoComplete.Web;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace AutoComplete.Clients.WebCore.Search
{
    public class SearchMiddleware
    {
        private readonly SearchMiddlewareSetting _settings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _headerFilePath;
        private readonly string _indexFilePath;
        private readonly string _tailFilePath;

        public SearchMiddleware(
                RequestDelegate next,
                IOptions<SearchMiddlewareSetting> settings,
                IHostingEnvironment hostingEnvironment
            )
        {
            _settings = settings?.Value;
            _hostingEnvironment = hostingEnvironment;

            _headerFilePath = GetDbPath("header.json");
            _indexFilePath = GetDbPath("index.bin");

            _tailFilePath = GetDbPath("tail.txt");
            if (_settings.UseTailFile && !File.Exists(_tailFilePath))
                _tailFilePath = null;
        }

        private string GetDbPath(string fileName)
        {
            return Path.Combine(
                _hostingEnvironment.ContentRootPath,
                $"{_settings.DbDirectory}{Path.DirectorySeparatorChar}{fileName}"
            );
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsValidContext(context))
            {
                SearchOptions searchOptions = GetSearchOptions(context);
                IIndexSearcher searcher = GetSearcher();

                SearchResult searchResult = searcher.Search(searchOptions);

                context.Response.ContentType = _settings.ResponseContentType;
                await context.Response.WriteAsync(searchResult.FormatToJson());
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
                searcher = new InMemoryIndexSearcher(_headerFilePath, _indexFilePath, _tailFilePath);
            }
            else
            {
                searcher = new FileSystemIndexSearcher(_headerFilePath, _indexFilePath, _tailFilePath);
            }

            return searcher;
        }

        private SearchOptions GetSearchOptions(HttpContext context)
        {
            var searchOptions = new SearchOptions();
            searchOptions.SuggestWhenFoundStartsWith = _settings.SuggestWhenFoundStartsWith;
            searchOptions.MaxItemCount = RequestHelper.ExtractValue(context.Request, _settings.MaxItemCountName, _settings.DefaultMaxItemCount);
            searchOptions.Term = RequestHelper.ExtractValue(context.Request, _settings.KeywordName, string.Empty);

            return searchOptions;
        }

        #endregion
    }
}
