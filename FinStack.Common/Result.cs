using Microsoft.AspNetCore.Identity;

namespace FinStack.Common;

public class Result<T>
{
    public T? Value { get; }
    public IReadOnlyList<Exception> Errors { get; }
    public bool IsSuccess => Errors.Count == 0;
    public Result(T value)
    {
        Value = value;
        Errors = new List<Exception>();
    }
    public Result(string error) => Errors = [new Exception(error)];
    public Result(Exception error) => Errors = [error];
    public Result(IEnumerable<Exception> errors)
    {
        Errors = errors as IReadOnlyList<Exception> ?? errors?.ToList();
    }
    public Result(IEnumerable<IdentityError> errors)
    {
        Errors = errors.Select(e => new Exception(e.Code)).ToList();
    }

    public bool Failed(out IEnumerable<Exception> errors)
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
    
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<IEnumerable<Exception>, TResult> onFailure)
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
}

public static class Result
{
    public static Result<T> Success<T>(T value) => new (value);
    public static Result<T> Failure<T>(string error) => new(error);
    public static Result<T> Failure<T>(Exception error) => new(error);
    public static Result<T> Failure<T>(IEnumerable<Exception> errors) => new(errors);
    public static Result<T> Failure<T>(IEnumerable<IdentityError> errors) => new(errors);
}