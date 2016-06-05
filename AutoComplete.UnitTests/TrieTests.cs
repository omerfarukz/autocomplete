using AutoComplete.Core.DataStructure;
using AutoComplete.UnitTests.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}