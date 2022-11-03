using System;
using System.Collections;
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
}