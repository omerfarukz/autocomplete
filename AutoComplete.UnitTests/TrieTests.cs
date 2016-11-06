using AutoComplete.Core;
using AutoComplete.Core.DataStructure;
using AutoComplete.Desktop;
using AutoComplete.UnitTests.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace AutoComplete.UnitTests
{
    [TestClass]
    public class TrieTests
    {
        [TestMethod]
        public void build_simple_trie_test()
        {
            var trie = new Trie();
            trie.Add(TrieNodeInput.Create("Unit"));
            trie.Add(TrieNodeInput.Create("Test"));

            Assert.IsNotNull(trie.Root);
            Assert.IsNotNull(trie.Root.Children);
            Assert.AreEqual(2, trie.Root.Children.Count);
            Assert.AreEqual(false, trie.Root.IsTerminal);
        }

        [TestMethod]
        public void build_trie_and_check_common_nodes()
        {
            var trie = new Trie();
            trie.Add(TrieNodeInput.Create("Unit"));
            trie.Add(TrieNodeInput.Create("Unite"));

            Assert.IsNotNull(trie.Root);
            Assert.IsNotNull(trie.Root.Children);
            Assert.AreEqual(1, trie.Root.Children.Count);

            Assert.IsNotNull(trie.Root.Children['U'].Children);
            Assert.AreEqual(1, trie.Root.Children['U'].Children.Count);
        }

        [TestMethod]
        public void build_trie_and_get_last_node__should_found_and_not_found()
        {
            var trie = new Trie();
            trie.Add(TrieNodeInput.Create("Unit"));
            trie.Add(TrieNodeInput.Create("Unite"));

            Assert.IsNotNull(trie.Root);
            Assert.IsNotNull(trie.Root.Children);
            Assert.AreEqual(1, trie.Root.Children.Count);

            var unitNode = trie.GetLastNode(TrieNodeInput.Create("Unite"));

            Assert.IsNotNull(unitNode);
            Assert.AreEqual(unitNode.Status, TrieNodeSearchResultType.FoundEquals);

            var unkownNode = trie.GetLastNode(TrieNodeInput.Create("NotFound"));
            Assert.IsNotNull(unkownNode);
            Assert.AreEqual(unkownNode.Node, trie.Root);
        }

        [TestMethod]
        public void build_trie_and_get_last_node_should_found_starts_with()
        {
            var trie = new Trie();
            trie.Add(TrieNodeInput.Create("Unit"));

            var foundStartsWithNode = trie.GetLastNode(TrieNodeInput.Create("Unite"));
            Assert.IsNotNull(foundStartsWithNode);
            Assert.AreEqual(foundStartsWithNode.Status, TrieNodeSearchResultType.FoundStartsWith);
        }

        [TestMethod]
        public void build_trie_and_get_last_node_should_last_keyword_index_should_equal()
        {
            var trie = new Trie();
            trie.Add(TrieNodeInput.Create("Unit"));

            var foundStartsWithNode = trie.GetLastNode(TrieNodeInput.Create("Unite"));
            Assert.IsNotNull(foundStartsWithNode);
            Assert.IsTrue(foundStartsWithNode.LastKeywordIndex.HasValue);
            Assert.AreEqual(4, foundStartsWithNode.LastKeywordIndex.Value);
        }

        [TestMethod]
        public void trie_load_faked_source_shold_be_success()
        {
            var trie = new Trie();
            int loadedCount = trie.Load(new FakeKeywordDataSource());

            Assert.AreEqual(20000, loadedCount);
        }

        [TestMethod]
        public void create_index_and_header_files__search_one_term_and_remove_both_in_memory()
        {
            var indexFileName = $"index_{Guid.NewGuid()}.bin";
            var headerFileName = $"header_{Guid.NewGuid()}.json";
            var tailFileName = $"text_{Guid.NewGuid()}.txt";

            using (var index = new FileStream(indexFileName, FileMode.OpenOrCreate))
            {
                using (var header = new FileStream(headerFileName, FileMode.OpenOrCreate))
                {
                    using (var tail = new FileStream(tailFileName, FileMode.OpenOrCreate))
                    {
                        IndexBuilder ib = new IndexBuilder(header, index, tail);
                        ib.Add(new FakeKeywordDataSource());

                        ib.Build();
                    }
                }
            }
            
            var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, null);
            var result = searcher.Search("armor",10, true);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(result.ResultType, TrieNodeSearchResultType.FoundEquals);
            Assert.IsTrue(result.Items.Length >= 2);
            Assert.AreEqual(result.Items[0], "armor");
            Assert.AreEqual(result.Items[1], "armory");

            File.Delete(indexFileName);
            File.Delete(headerFileName);
            File.Delete(tailFileName);

            Assert.IsFalse(File.Exists(indexFileName));
            Assert.IsFalse(File.Exists(headerFileName));
            Assert.IsFalse(File.Exists(tailFileName));
        }

        [TestMethod]
        public void create_index_and_header_files__search_one_term_and_remove_both_in_file_system()
        {
            var indexFileName = $"index_{Guid.NewGuid()}.bin";
            var headerFileName = $"header_{Guid.NewGuid()}.json";
            var tailFileName = $"text_{Guid.NewGuid()}.txt";

            using (var index = new FileStream(indexFileName, FileMode.OpenOrCreate))
            {
                using (var header = new FileStream(headerFileName, FileMode.OpenOrCreate))
                {
                    using (var tail = new FileStream(tailFileName, FileMode.OpenOrCreate))
                    {
                        IndexBuilder ib = new IndexBuilder(header, index, tail);
                        ib.Add(new FakeKeywordDataSource());

                        ib.Build();
                    }
                }
            }

            var searcher = new InMemoryIndexSearcher(headerFileName, indexFileName, null);
            var result = searcher.Search("armor", 10, true);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(result.ResultType, TrieNodeSearchResultType.FoundEquals);
            Assert.IsTrue(result.Items.Length >= 2);
            Assert.AreEqual(result.Items[0], "armor");
            Assert.AreEqual(result.Items[1], "armory");

            File.Delete(indexFileName);
            File.Delete(headerFileName);
            File.Delete(tailFileName);

            Assert.IsFalse(File.Exists(indexFileName));
            Assert.IsFalse(File.Exists(headerFileName));
            Assert.IsFalse(File.Exists(tailFileName));
        }


        [TestMethod]
        public void StreamReadWithOneByOne()
        {
            Read(1);
        }

        [TestMethod]
        public void StreamReadWithBuffer4()
        {
            Read(4);
        }

        [TestMethod]
        public void StreamReadWithBuffer32()
        {
            Read(32);
        }

        [TestMethod]
        public void StreamReadWithBuffer64()
        {
            Read(64);
        }

        [TestMethod]
        public void StreamReadWithBuffer128()
        {
            Read(128);
        }

        [TestMethod]
        public void StreamReadWithBuffer256()
        {
            Read(256);
        }


        [TestMethod]
        public void StreamReadWithBuffer512()
        {
            Read(512);
        }

        [TestMethod]
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