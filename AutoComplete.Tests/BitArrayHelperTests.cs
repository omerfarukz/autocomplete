using System;
using System.Collections;
using System.Linq;
using AutoComplete.Helpers;
using Xunit;

namespace AutoComplete.Tests;

public class BitArrayHelperTests
{
    [Fact]
    public void passing_null_args_should_throw_argument_exception()
    {
        BitArray bitArray = null;
        Assert.Throws<ArgumentNullException>(() =>
        {
            BitArrayHelper.CopyToInt32Array(bitArray, Array.Empty<int>(), 0);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            BitArrayHelper.CopyToInt32Array(new BitArray(Array.Empty<bool>()), null, 0);
        });
    }

    [Fact]
    public void copy_to_int_32_to_next_block_should_pass()
    {
        var bits = Enumerable.Range(0, 33).Select(f => f % 2 == 0).ToArray();
        var array = new int[33];
        BitArrayHelper.CopyToInt32Array(
            new BitArray(Enumerable.Range(0, 33).Select(f => f % 2 == 0).ToArray()), 
            array, 
            0);
        Assert.Equal(1, array[1]);
    }
}