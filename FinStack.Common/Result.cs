using Microsoft.AspNetCore.Identity;

namespace FinStack.Common;

public class Result<T>
{
    private readonly T _value;

    public IReadOnlyList<Error> Errors { get; }
    public IReadOnlyList<string> ErrorCodes
    {
        get
        {
            return Errors.Select(e => e.Code).ToList();
        }
    }

    public bool IsSuccess => Errors.Count == 0;
    public bool IsFailure => Errors.Count > 0;

    public Result(T value)
    {
        _value = value;
        Errors = new List<Error>();
    }

    public Result(Error error)
    {
        _value = default!;
        Errors = new List<Error> { error };
    }

    public Result(IEnumerable<Error> errors)
    {
        _value = default!;
        Errors = errors.ToList();
    }

    public Result(string code, string message)
    {
        _value = default!;
        Errors = [new Error(code, message)];
    }

    public Result(IEnumerable<IdentityError> errors)
    {
        _value = default!;
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
            value = _value!;
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
            return onSuccess(_value!);
        }
        else
        {
            return onFailure(Errors);
        }
    }

    public T Unwrap()
    {
        if (!IsSuccess)
        {
            var exceptions = Errors
                .Select(e => new Exception($"{e.Code}: {e.Message}"))
                .ToArray();

            throw new AggregateException("Multiple errors occurred.", exceptions);
        }

        return _value;
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value) => new(value);
    public static Result<T> Failure<T>(Error error) => new(error);
    public static Result<T> Failure<T>(string code, string message) => new(new Error(code, message));
    public static Result<T> Failure<T>(IEnumerable<Error> errors) => new(errors);
    public static Result<T> Failure<T>(IEnumerable<IdentityError> errors) => new(errors);
    public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> task) where T : notnull
    {
        var result = await task.ConfigureAwait(false);
        return result.Unwrap();
    }
}