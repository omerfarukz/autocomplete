using System;
using AutoComplete.Domain;
using Xunit;

namespace AutoComplete.Tests;

public class ManagedInMemoryStreamTests
{
    [Fact]
    public void write_should_throw_not_supported_exception()
    {
        var stream = new ManagedInMemoryStream(Array.Empty<byte>());
        Assert.Throws<NotSupportedException>(() =>
        {
            stream.Write(new[] {(byte) 0});
        });
    }
}