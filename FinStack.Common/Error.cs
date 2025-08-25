using System.Text.Json.Serialization;

public enum ErrorSeverity
{
    Error,
    Warning,
}

public record Error
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    [JsonIgnore]
    public ErrorSeverity Severity { get; set; } = ErrorSeverity.Error;

    public Error(string Code)
    {
        this.Code = Code;
    }

    public Error(string Code, string Message)
    {
        this.Code = Code;
        this.Message = Message;
    }

    public static readonly Error FirstNameRequired =
        new("FirstNameRequired", "First name is required.");

    public static readonly Error LastNameRequired =
        new("LastNameRequired", "Last name is required.");

    public static Error UserNotFound(Guid userGuid) =>
        new("UserNotFound", $"User with id {userGuid} was not found.");

    public static Error UserAlreadyExists(string email) =>
        new("UserAlreadyExists", $"User with email {email} was already found.");


    private static readonly Dictionary<string, Error> _errorsByCode =
    new()
    {
        { FirstNameRequired.Code, FirstNameRequired },
        { LastNameRequired.Code, LastNameRequired }
    };

    public static Error? FromCode(string code) =>
        _errorsByCode.TryGetValue(code, out var error) ? error : null;
}
