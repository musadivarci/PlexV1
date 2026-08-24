using Plex.Primitives;

namespace Plex.Primitives.Tests;

public sealed class ResultTests
{
    [Fact]
    public void Success_exposes_value()
    {
        var result = Result.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
        Assert.True(result.Error.IsNone);
    }

    [Fact]
    public void Failure_exposes_error_and_blocks_value_access()
    {
        var result = Result.Failure<int>(new Error("demo.failure", "Something failed."));

        Assert.True(result.IsFailure);
        Assert.Equal("demo.failure", result.Error.Code);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void Map_transforms_success_value()
    {
        var result = Result.Success(21).Map(value => value * 2);

        Assert.Equal(42, result.Value);
    }
}