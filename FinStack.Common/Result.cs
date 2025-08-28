using Microsoft.AspNetCore.Identity;

namespace FinStack.Common;

public class Result<T>
{
    public T? Value { get; }
    public IReadOnlyList<Error> Errors { get; }
    public bool IsSuccess => Errors.Count == 0;
    public Result(T value)
    {
        Value = value;
        Errors = new List<Error>();
    }

    public Result(Error error)
    {
        Errors = new List<Error> { error };
    }

    public Result(IEnumerable<Error> errors)
    {
        Errors = errors.ToList();
    }

    public Result(string code, string message)
    {
        Errors = [new Error(code, message)];
    }

    public Result(IEnumerable<IdentityError> errors)
    {
        Errors = errors.Select(e => new Error(e.Code, e.Description)).ToList();
    }

    public bool Failed(out IEnumerable<Error> errors)
    {
        errors = Errors;
        return !IsSuccess;
    }

    public bool Succeeded(out T value)
    {
        if (IsSuccess)
        {
            value = Value!;
            return true;
        }
        else
        {
            value = default!;
            return false;
        }
    }

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<IEnumerable<Error>, TResult> onFailure)
    {
        if (IsSuccess)
        {
            return onSuccess(Value!);
        }
        else
        {
            return onFailure(Errors);
        }
    }

    public T? Unwrap()
    {
        if (!IsSuccess)
        {
            var exceptions = Errors
                .Select(e => new Exception($"{e.Code}: {e.Message}"))
                .ToArray();

            throw new AggregateException("Multiple errors occurred.", exceptions);
        }

        return Value;
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value) => new (value);
    public static Result<T> Failure<T>(Error error) => new(error);
    public static Result<T> Failure<T>(string code, string message) => new(new Error(code, message));
    public static Result<T> Failure<T>(IEnumerable<Error> errors) => new(errors);
    public static Result<T> Failure<T>(IEnumerable<IdentityError> errors) => new(errors);
}