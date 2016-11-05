using AutoComplete.Core;
using AutoComplete.Core.DataStructure;
using AutoComplete.Desktop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace AutoComplete.UnitTests
{
    [TestClass]
    public class SearchTests
    {
        [TestMethod]
        public void add_20k_words_and_check_node_count()
        {
            using (var headerStream = new MemoryStream())
            {
                using (var indexStream = new MemoryStream())
                {
                    var builder = new IndexBuilder(headerStream, indexStream);
                    builder.Add("John");

                    var processedNodeCount = builder.Build();

                    headerStream.Position = 0;
                    indexStream.Position = 0;

                    var searcher = new InMemoryIndexSearcher("Resource\\20k_header.json", "Resource\\20k_index.bin");
                    var searchResult = searcher.Search("found", 10, true);

                    Assert.IsNotNull(searchResult);
                    Assert.AreEqual(searchResult.ResultType, TrieNodeSearchResultType.FoundEquals);

                    Assert.IsNotNull(searchResult.Items);
                    Assert.AreEqual(7, searchResult.Items.Length);
                }
            }
        }
    }
}