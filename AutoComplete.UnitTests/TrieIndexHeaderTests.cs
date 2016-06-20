using AutoComplete.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoComplete.UnitTests
{
    [TestClass]
    public class TrieIndexHeaderTests
    {
        [TestMethod]
        public void character_list_must_be_initialized_when_new_instance_was_created()
        {
            var header = new TrieIndexHeader();
            Assert.IsNotNull(header);
            Assert.IsNotNull(header.CharacterList);
            Assert.AreEqual(0, header.CharacterList.Count);
        }

        [TestMethod]
        public void add_char_and_character_list_count_should_be_one()
        {
            var header = new TrieIndexHeaderBuilder().AddChar('a').Build();

            Assert.AreEqual(1, header.CharacterList.Count);
        }

        [TestMethod]
        public void add_duplicated_char_and_character_list_count_should_be_one()
        {
            var header = new TrieIndexHeaderBuilder()
                            .AddChar('a')
                            .AddChar('a')
                            .Build();

            Assert.AreEqual(1, header.CharacterList.Count);
        }

        [TestMethod]
        public void add_two_different_chars_and_character_list_count_should_be_two()
        {
            var header = new TrieIndexHeaderBuilder()
                            .AddChar('a')
                            .AddChar('b')
                            .Build();

            Assert.AreEqual(2, header.CharacterList.Count);
        }

        [TestMethod]
        public void add_empty_and_null_string_and_character_list_count_should_be_zero()
        {
            var header = new TrieIndexHeaderBuilder()
                            .AddString(null)
                            .AddString(string.Empty)
                            .Build();

            Assert.AreEqual(0, header.CharacterList.Count);
        }

        [TestMethod]
        public void add_string_with_unique_chars_length_of_three_and_character_list_count_should_be_three()
        {
            var header = new TrieIndexHeaderBuilder()
                            .AddString("abc")
                            .Build();

            Assert.AreEqual(3, header.CharacterList.Count);
        }

        [TestMethod]
        public void add_duplicated_strings_with_unique_chars_length_of_three_and_character_list_count_should_be_three()
        {
            var header = new TrieIndexHeaderBuilder()
                .AddString("abc")
                .AddString("abc")
                .Build();

            Assert.AreEqual(3, header.CharacterList.Count);
        }

        [TestMethod]
        public void add_two_different_strings_not_intercepted_with_unique_chars_length_of_three_and_character_list_count_should_be_six()
        {
            var header = new TrieIndexHeaderBuilder()
                .AddString("abc")
                .AddString("def")
                .Build();

            Assert.AreEqual(6, header.CharacterList.Count);
        }

        [TestMethod]
        public void add_two_strings_intercepted_two_chars_with_unique_chars_length_of_three_and_character_list_count_should_be_4()
        {
            var header = new TrieIndexHeaderBuilder()
                                .AddString("abc")
                                .AddString("dab")
                                .Build();

            Assert.AreEqual(4, header.CharacterList.Count);
        }

        [TestMethod]
        public void get_character_index_must_be_null()
        {
            var header = new TrieIndexHeader();
            var characterIndex = TrieIndexHeaderCharacterReader.Instance.GetCharacterIndex(header, 'a');

            Assert.AreEqual(null, characterIndex);
        }

        [TestMethod]
        public void calculate_metrics_and_get_character_index_must_be_zero()
        {
            var header = new TrieIndexHeaderBuilder().AddChar('a').Build();

            var characterIndex = TrieIndexHeaderCharacterReader.Instance.GetCharacterIndex(header, 'a');

            Assert.AreEqual((ushort)0, characterIndex);
        }

        [TestMethod]
        public void calculate_metrics_and_get_character_index_must_be_zero_and_one()
        {
            var header = new TrieIndexHeaderBuilder()
                .AddChar('a')
                .AddChar('b')
                .Build();

            var characterIndex_a = TrieIndexHeaderCharacterReader.Instance.GetCharacterIndex(header, 'a');
            var characterIndex_b = TrieIndexHeaderCharacterReader.Instance.GetCharacterIndex(header, 'b');

            Assert.AreEqual((ushort)0, characterIndex_a);
            Assert.AreEqual((ushort)1, characterIndex_b);
        }

        [TestMethod]
        public void calculate_metrics_and_get_character__at_index_must_be_a_and_b()
        {
            var header = new TrieIndexHeaderBuilder()
                .AddChar('a')
                .AddChar('b')
                .Build();

            var character_a = TrieIndexHeaderCharacterReader.Instance.GetCharacterAtIndex(header, 0);
            var character_b = TrieIndexHeaderCharacterReader.Instance.GetCharacterAtIndex(header, 1);

            Assert.AreEqual(character_a, 'a');
            Assert.AreEqual(character_b, 'b');
        }

        [TestMethod]
        public void calculate_metrics_and_sort_and_get_character__at_index_must_be_a_and_b()
        {
            var header = new TrieIndexHeaderBuilder()
                 .AddChar('c')
                 .AddChar('b')
                 .AddChar('a')
                 .Build();

            var character_a = TrieIndexHeaderCharacterReader.Instance.GetCharacterAtIndex(header, 0);
            var character_b = TrieIndexHeaderCharacterReader.Instance.GetCharacterAtIndex(header, 1);
            var character_c = TrieIndexHeaderCharacterReader.Instance.GetCharacterAtIndex(header, 2);

            Assert.AreEqual(character_a, 'a');
            Assert.AreEqual(character_b, 'b');
            Assert.AreEqual(character_c, 'c');
        }
    }
}