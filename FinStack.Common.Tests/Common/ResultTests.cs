using System;
using System.Collections.Generic;
using System.Linq;
using FinStack.Common;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace FinStack.Tests.Common;

public class ResultTests
{
    [Fact]
    public void Success_CreatesSuccessfulResult()
    {
        var result = Result.Success("Hello");

        Assert.True(result.IsSuccess);
        Assert.Equal("Hello", result.Unwrap());
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Failure_FromString_CreatesFailureResult()
    {
        var result = Result.Failure<string>(new Error("ERROR", "Something went wrong"));

        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Equal("Something went wrong", result.Errors.First().Message);
    }

    [Fact]
    public void Failure_FromException_CreatesFailureResult()
    {
        var ex = new Error("Invalid");
        var result = Result.Failure<string>(ex);

        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Same(ex, result.Errors.First());
    }

    [Fact]
    public void Failure_FromMultipleExceptions_CreatesFailureResult()
    {
        var errors = new List<Error>
        {
            new Error("Error1"),
            new Error("Error2")
        };

        var result = Result.Failure<string>(errors);

        Assert.False(result.IsSuccess);
        Assert.Equal(2, result.Errors.Count);
    }

    [Fact]
    public void Succeeded_ReturnsTrue_AndOutsValue()
    {
        var result = Result.Success(42);

        var success = result.Succeeded(out var value);

        Assert.True(success);
        Assert.Equal(42, value);
    }

    [Fact]
    public void Failed_ReturnsTrue_AndOutsErrors()
    {
        var result = Result.Failure<int>(new Error("Bad input"));

        var failed = result.Failed(out var errors);

        Assert.True(failed);
        Assert.Single(errors);
    }


    [Fact]
    public void Match_HandlesSuccess()
    {
        var result = Result.Success("OK");

        var outcome = result.Match(
            onSuccess: val => $"Success: {val}",
            onFailure: _ => "Failure"
        );

        Assert.Equal("Success: OK", outcome);
    }

    [Fact]
    public void Match_HandlesFailure()
    {
        var result = Result.Failure<string>(new Error("Failed"));

        var outcome = result.Match(
            onSuccess: val => $"Success: {val}",
            onFailure: errors => $"Failure: {string.Join(",", errors.Select(e => e.Code))}"
        );

        Assert.Equal("Failure: Failed", outcome);
    }
}
