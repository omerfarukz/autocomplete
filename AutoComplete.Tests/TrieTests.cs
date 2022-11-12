using System;
using AutoComplete.DataStructure;
using Xunit;

namespace AutoComplete.Tests;

public class TrieTests
{
    [Fact]
    public void search_last_node_from_null_keyword_should_throw()
    {
        var trie = new Trie();
        Assert.Throws<ArgumentNullException>(() => trie.SearchLastNodeFrom(null));
    }
    
    [Fact]
    public void add_null_keyword_should_throw()
    {
        var trie = new Trie();
        Assert.Throws<ArgumentNullException>(() => trie.Add(null));
    }
    
    [Fact]
    public void add_null_node_to_trie_node_should_throw()
    {
        var trie = new TrieNode();
        Assert.Throws<ArgumentNullException>(() => trie.Add(null));
    }
    
    [Fact]
    public void create_node_from_keyword_should_throw()
    {
        Assert.Throws<ArgumentNullException>(() => TrieNode.CreateFrom(null));
        Assert.Throws<ArgumentNullException>(() => TrieNode.CreateFrom(string.Empty));
    }
    
    [Fact]
    public void create_not_found_node_should_return_node_as_null()
    {
        var result = TrieNodeSearchResult.CreateNotFound();
        Assert.Null(result.Node);
    }
    
    [Fact]
    public void add_same_character_to_trie_node_should_pass_without_exception()
    {
        var trie = new TrieNode();
        var child = new TrieNode('a');
        trie.Add(child);
        trie.Add(child);
        Assert.Equal(1, trie.Children.Count);
    }
    
    [Fact]
    public void add_same_character_that_have_children_to_trie_node_should_pass_without_exception()
    {
        var trie = new TrieNode();
        var child_0 = new TrieNode('a');
        var child_1 = new TrieNode('a');
        child_1.Add(new TrieNode('1'));
        
        trie.Add(child_0);
        trie.Add(child_1);
        Assert.Equal(1, trie.Children.Count);
    }

    [Fact]
    public void add_new_sub_keyword_should_pass()
    {
        var trie = new Trie();
        trie.Add("johnny");
        trie.Add("john");
        trie.Add("jo");

        var lastNode = trie.SearchLastNodeFrom("jo");
        Assert.NotNull(lastNode);
        Assert.NotNull(lastNode.Node);
        Assert.NotNull(lastNode.Node.Children);
        Assert.Equal(1, lastNode.Node.Children.Count);
    }

    [Fact]
    public void build_simple_trie_test()
    {
        var trie = new Trie();
        trie.Add("Unit");
        trie.Add("Test");

        Assert.NotNull(trie.Root);
        Assert.NotNull(trie.Root.Children);
        Assert.Equal(2, trie.Root.Children.Count);
        Assert.False(trie.Root.IsTerminal);
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
        Assert.Equal(TrieNodeSearchResultType.FoundEquals, unitNode.Status);

        var unknownNode = trie.SearchLastNodeFrom("NotFound");
        Assert.NotNull(unknownNode);
        Assert.Equal(unknownNode.Node, trie.Root);
    }

    [Fact]
    public void build_trie_and_get_last_node_should_found_starts_with()
    {
        var trie = new Trie();
        trie.Add("Unit");

        var foundStartsWithNode = trie.SearchLastNodeFrom("Unite");
        Assert.NotNull(foundStartsWithNode);
        Assert.Equal(TrieNodeSearchResultType.FoundStartsWith, foundStartsWithNode.Status);
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
}