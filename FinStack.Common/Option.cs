namespace FinStack.Common;

public readonly struct Option<T>
{
    private readonly T _value;
    public bool IsSome { get; }
    public bool IsNone => !IsSome;
    
    public Option(T? value)
    {
        IsSome = value != null;
        if (value != null)
        {
            _value = value;
        }
    }

    public static Option<T> Some(T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "Cannot create Some with null");

        return new Option<T>(value);
    }

    public static Option<T> None() => default;

    public T Value => IsSome
        ? _value
        : throw new InvalidOperationException("Option has no value");

    public bool TryGetValue(out T value)
    {
        value = _value;
        return IsSome;
    }

    public Option<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
        if (IsNone) return Option<TResult>.None();
        return Option<TResult>.Some(mapper(_value));
    }

    public TResult Match<TResult>(Func<T, TResult> someFunc, Func<TResult> noneFunc)
    {
        return IsSome ? someFunc(_value) : noneFunc();
    }

    public override bool Equals(object? obj) =>
        obj is Option<T> other && Equals(other);

    public bool Equals(Option<T> other)
    {
        if (IsNone && other.IsNone) return true;
        if (IsSome && other.IsSome) return Equals(_value, other._value);
        return false;
    }

    public override int GetHashCode() =>
        IsSome ? _value?.GetHashCode() ?? 0 : 0;

    public override string ToString() =>
        IsSome ? $"Some({_value})" : "None";

    public static implicit operator Option<T>(T value) =>
        value == null ? None() : Some(value);
}

public static class Option
{
    public static Option<T> Some<T>(T value) => new (value);
    public static Option<T> None<T>() => new ();
    
}