using System;
using AutoComplete.Builders;
using AutoComplete.DataStructure;
using AutoComplete.Readers;
using Xunit;

namespace AutoComplete.Tests;

public class TrieHeaderTests
{
    [Fact]
    public void character_list_must_be_initialized_when_new_instance_was_created()
    {
        var header = new TrieIndexHeader();
        Assert.NotNull(header);
        Assert.NotNull(header.CharacterList);
        Assert.Empty(header.CharacterList);
    }

    [Fact]
    public void add_char_and_character_list_count_should_be_one()
    {
        var header = new TrieIndexHeaderBuilder().AddChar('a').Build();
        Assert.Single(header.CharacterList);
    }

    [Fact]
    public void add_duplicated_char_and_character_list_count_should_be_one()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddChar('a')
            .AddChar('a')
            .Build();

        Assert.Single(header.CharacterList);
    }

    [Fact]
    public void add_two_different_chars_and_character_list_count_should_be_two()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddChar('a')
            .AddChar('b')
            .Build();

        Assert.Equal(2, header.CharacterList.Count);
    }

    [Fact]
    public void add_empty_string_should_throw_exception()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            new TrieIndexHeaderBuilder().AddString(string.Empty);
        });
    }

    [Fact]
    public void add_null_string_should_throw_exception()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            new TrieIndexHeaderBuilder().AddString(null);
        });
    }

    [Fact]
    public void add_string_with_unique_chars_length_of_three_and_character_list_count_should_be_three()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddString("abc")
            .Build();

        Assert.Equal(3, header.CharacterList.Count);
    }

    [Fact]
    public void add_duplicated_strings_with_unique_chars_length_of_three_and_character_list_count_should_be_three()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddString("abc")
            .AddString("abc")
            .Build();

        Assert.Equal(3, header.CharacterList.Count);
    }

    [Fact]
    public void
        add_two_different_strings_not_intercepted_with_unique_chars_length_of_three_and_character_list_count_should_be_six()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddString("abc")
            .AddString("def")
            .Build();

        Assert.Equal(6, header.CharacterList.Count);
    }

    [Fact]
    public void
        add_two_strings_intercepted_two_chars_with_unique_chars_length_of_three_and_character_list_count_should_be_4()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddString("abc")
            .AddString("dab")
            .Build();

        Assert.Equal(4, header.CharacterList.Count);
    }

    [Fact]
    public void get_character_index_must_be_null()
    {
        var header = new TrieIndexHeader();
        var characterIndex = new TrieIndexHeaderCharacterReader(header).GetCharacterIndex('x');

        Assert.Null(characterIndex);
    }

    [Fact]
    public void calculate_metrics_and_get_character_index_must_be_zero()
    {
        var header = new TrieIndexHeaderBuilder().AddChar('a').Build();
        var characterIndex = new TrieIndexHeaderCharacterReader(header).GetCharacterIndex('a');

        Assert.Equal((ushort) 0, characterIndex);
    }

    [Fact]
    public void calculate_metrics_and_get_character_index_must_be_zero_and_one()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddChar('a')
            .AddChar('b')
            .Build();

        var characterIndex_a = new TrieIndexHeaderCharacterReader(header).GetCharacterIndex('a');
        var characterIndex_b = new TrieIndexHeaderCharacterReader(header).GetCharacterIndex('b');

        Assert.Equal((ushort) 0, characterIndex_a);
        Assert.Equal((ushort) 1, characterIndex_b);
    }

    [Fact]
    public void calculate_metrics_and_get_character__at_index_must_be_a_and_b()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddChar('a')
            .AddChar('b')
            .Build();

        var character_a = new TrieIndexHeaderCharacterReader(header).GetCharacterAtIndex(0);
        var character_b = new TrieIndexHeaderCharacterReader(header).GetCharacterAtIndex(1);

        Assert.Equal('a', character_a);
        Assert.Equal('b', character_b);
    }

    [Fact]
    public void calculate_metrics_and_sort_and_get_character__at_index_must_be_a_and_b()
    {
        var header = new TrieIndexHeaderBuilder()
            .AddChar('c')
            .AddChar('b')
            .AddChar('a')
            .Build();

        var characterA = new TrieIndexHeaderCharacterReader(header).GetCharacterAtIndex(0);
        var characterB = new TrieIndexHeaderCharacterReader(header).GetCharacterAtIndex(1);
        var characterC = new TrieIndexHeaderCharacterReader(header).GetCharacterAtIndex(2);

        Assert.Equal('a', characterA);
        Assert.Equal('b', characterB);
        Assert.Equal('c', characterC);
    }
}