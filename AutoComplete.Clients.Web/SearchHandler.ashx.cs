using AutoComplete.Core;
using AutoComplete.Desktop;
using AutoComplete.Web;
using System.Web.Hosting;

namespace AutoComplete.Clients.Web
{
    /// <summary>
    /// Summary description for SearchHandler1
    /// </summary>
    public class SearchHandler1 : IndexSearcherHttpHandlerBase
    {
        public override IIndexSearcher GetSearcher()
        {
            string headerPath = HostingEnvironment.MapPath("~/App_Data/20k_header.json");
            string indexPath = HostingEnvironment.MapPath("~/App_Data/20k_index.bin");

            IIndexSearcher searcher = new InMemoryIndexSearcher(headerPath, indexPath);

            return searcher;
        }
    }
}