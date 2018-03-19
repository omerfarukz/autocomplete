using AutoComplete.Core;
using AutoComplete.Core.DataStructure;
using System;
using System.IO;
using Xunit;

namespace AutoComplete.UnitTests
{
    public class TrieTests
    {
        [Fact]
        public void build_simple_trie_test()
        {
            var trie = new Trie();
            trie.Add("Unit");
            trie.Add("Test");

            Assert.NotNull(trie.Root);
            Assert.NotNull(trie.Root.Children);
            Assert.Equal(2, trie.Root.Children.Count);
            Assert.Equal(false, trie.Root.IsTerminal);
        }

        [Fact]
        public void build_trie_and_check_common_nodes()
        {
            var trie = new Trie();
            trie.Add("Unit");
            trie.Add("Unite");

            Assert.NotNull(trie.Root);
            Assert.NotNull(trie.Root.Children);
            Assert.Equal(1, trie.Root.Children.Count);

            Assert.NotNull(trie.Root.Children['U'].Children);
            Assert.Equal(1, trie.Root.Children['U'].Children.Count);
        }

        [Fact]
        public void build_trie_and_get_last_node__should_found_and_not_found()
        {
            var trie = new Trie();
            trie.Add("Unit");
            trie.Add("Unite");

            Assert.NotNull(trie.Root);
            Assert.NotNull(trie.Root.Children);
            Assert.Equal(1, trie.Root.Children.Count);

            var unitNode = trie.SearchLastNodeFrom("Unite");

            Assert.NotNull(unitNode);
            Assert.Equal(unitNode.Status, TrieNodeSearchResultType.FoundEquals);

            var unkownNode = trie.SearchLastNodeFrom("NotFound");
            Assert.NotNull(unkownNode);
            Assert.Equal(unkownNode.Node, trie.Root);
        }

        [Fact]
        public void build_trie_and_get_last_node_should_found_starts_with()
        {
            var trie = new Trie();
            trie.Add("Unit");

            var foundStartsWithNode = trie.SearchLastNodeFrom("Unite");
            Assert.NotNull(foundStartsWithNode);
            Assert.Equal(foundStartsWithNode.Status, TrieNodeSearchResultType.FoundStartsWith);
        }

        [Fact]
        public void build_trie_and_get_last_node_should_last_keyword_index_should_equal()
        {
            var trie = new Trie();
            trie.Add("Unit");

            var foundStartsWithNode = trie.SearchLastNodeFrom("Unite");
            Assert.NotNull(foundStartsWithNode);
            Assert.True(foundStartsWithNode.LastKeywordIndex.HasValue);
            Assert.Equal(4, foundStartsWithNode.LastKeywordIndex.Value);
        }
        
        // TODO:
        //[Fact]
        //public void create_index_and_header_files__search_one_term_and_remove_both_in_memory()
        //{
        //    var indexFileName = $"index_{Guid.NewGuid()}.bin";
        //    var headerFileName = $"header_{Guid.NewGuid()}.json";
        //    var tailFileName = $"text_{Guid.NewGuid()}.txt";

        //    using (var index = new FileStream(indexFileName, FileMode.OpenOrCreate))
        //    {
        //        using (var header = new FileStream(headerFileName, FileMode.OpenOrCreate))
        //        {
        //            using (var tail = new FileStream(tailFileName, FileMode.OpenOrCreate))
        //            {
        //                IndexBuilder ib = new IndexBuilder(header, index, tail);
        //                ib.Add(new FakeKeywordDataSource());
        //                ib.Build();
        //            }
        //        }
        //    }

        //    var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, tailFileName);
        //    var result = searcher.Search("armo", 10, true);

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Items);
        //    Assert.Equal(result.ResultType, TrieNodeSearchResultType.FoundEquals);
        //    Assert.IsTrue(result.Items.Length == 3);
        //    Assert.Equal(result.Items[0], "armor");
        //    Assert.Equal(result.Items[1], "armory");

        //    File.Delete(indexFileName);
        //    File.Delete(headerFileName);
        //    File.Delete(tailFileName);

        //    Assert.IsFalse(File.Exists(indexFileName));
        //    Assert.IsFalse(File.Exists(headerFileName));
        //    Assert.IsFalse(File.Exists(tailFileName));
        //}

        //[Fact]
        //public void create_index_and_header_files__search_one_term_and_remove_both_in_file_system()
        //{
        //    var indexFileName = $"index_{Guid.NewGuid()}.bin";
        //    var headerFileName = $"header_{Guid.NewGuid()}.json";
        //    var tailFileName = $"text_{Guid.NewGuid()}.txt";

        //    using (var index = new FileStream(indexFileName, FileMode.OpenOrCreate))
        //    {
        //        using (var header = new FileStream(headerFileName, FileMode.OpenOrCreate))
        //        {
        //            using (var tail = new FileStream(tailFileName, FileMode.OpenOrCreate))
        //            {
        //                IndexBuilder ib = new IndexBuilder(header, index, tail);
        //                ib.Add(new FakeKeywordDataSource());

        //                ib.Build();
        //            }
        //        }
        //    }

        //    var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, null);
        //    var result = searcher.Search("armor", 10, true);

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Items);
        //    Assert.Equal(result.ResultType, TrieNodeSearchResultType.FoundEquals);
        //    Assert.IsTrue(result.Items.Length >= 2);
        //    Assert.Equal(result.Items[0], "armor");
        //    Assert.Equal(result.Items[1], "armory");

        //    File.Delete(indexFileName);
        //    File.Delete(headerFileName);
        //    File.Delete(tailFileName);

        //    Assert.IsFalse(File.Exists(indexFileName));
        //    Assert.IsFalse(File.Exists(headerFileName));
        //    Assert.IsFalse(File.Exists(tailFileName));
        //}


        [Fact]
        public void StreamReadWithOneByOne()
        {
            Read(1);
        }

        [Fact]
        public void StreamReadWithBuffer4()
        {
            Read(4);
        }

        [Fact]
        public void StreamReadWithBuffer32()
        {
            Read(32);
        }

        [Fact]
        public void StreamReadWithBuffer64()
        {
            Read(64);
        }

        [Fact]
        public void StreamReadWithBuffer128()
        {
            Read(128);
        }

        [Fact]
        public void StreamReadWithBuffer256()
        {
            Read(256);
        }


        [Fact]
        public void StreamReadWithBuffer512()
        {
            Read(512);
        }

        [Fact]
        public void StreamReadWithBuffer1024()
        {
            Read(1024);
        }

        private void Read(int bufferCount)
        {
            byte[] bytes = new byte[60];

            using (var ms = new MemoryStream(bytes))
            {
                byte[] buffer = new byte[bufferCount];
                for (int i = 0; i < 1000000; i++)
                {
                    ms.Position = 0;

                    while (ms.Read(buffer, 0, buffer.Length) > 0)
                    { }
                }
            }
        }
    }
}