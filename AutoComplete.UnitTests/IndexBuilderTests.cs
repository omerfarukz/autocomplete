using AutoComplete.Core;
using AutoComplete.UnitTests.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace AutoComplete.UnitTests
{
    [TestClass]
    public class IndexBuilderTests
    {
        [TestMethod]
        public void add_single_word_and_check_node_count()
        {
            using (var headerStream = new MemoryStream())
            {
                using (var indexStream = new MemoryStream())
                {
                    var builder = new IndexBuilder(headerStream, indexStream);
                    builder.Add("John");

                    var processedNodeCount = builder.Build();

                    Assert.AreEqual(5, processedNodeCount);
                }
            }
        }

        [TestMethod]
        public void add_three_words_and_check_node_count()
        {
            using (var headerStream = new MemoryStream())
            {
                using (var indexStream = new MemoryStream())
                {
                    var builder = new IndexBuilder(headerStream, indexStream);
                    builder.Add("John");
                    builder.Add("Johnny");
                    builder.Add("Jane");

                    /*
                     *
                     *          R(1)*
                     *          J(2)
                     *      A(3)    O(4)
                     *      N(5)    H(6)
                     *     *E(7)    N(8)*
                     *              N(9)
                     *              Y(10)*
                     */

                    var processedNodeCount = builder.Build();

                    Assert.AreEqual(10, processedNodeCount);
                }
            }
        }

        [TestMethod]
        public void add_range_and_check_processes_node_count_are_equal()
        {
            using (var headerStream = new MemoryStream())
            {
                using (var indexStream = new MemoryStream())
                {
                    var builder = new IndexBuilder(headerStream, indexStream);
                    builder.AddRange(new string[] { "John", "Smith", "Jane" });

                    var processedNodeCount = builder.Build();

                    Assert.AreEqual(13, processedNodeCount);
                }
            }
        }

        [TestMethod]
        public void load_20k_keyword_with_data_source()
        {
            using (var headerStream = new MemoryStream())
            {
                using (var indexStream = new MemoryStream())
                {
                    var builder = new IndexBuilder(headerStream, indexStream);
                    builder.WithDataSource(new FakeKeywordDataSource());

                    var processedNodeCount = builder.Build();

                    Assert.AreEqual(50757, processedNodeCount);
                }
            }
        }
    }
}