
namespace FinStack.Contracts.Auth;

public static class PasswordErrors
{
    public static readonly Error PasswordTooShort =
        new("PasswordTooShort", "Passwords must be at least 6 characters.");

    public static readonly Error PasswordRequiresNonAlphanumeric =
        new("PasswordRequiresNonAlphanumeric", "Passwords must have at least one non alphanumeric character.");

    public static readonly Error PasswordRequiresLower =
        new("PasswordRequiresLower", "Passwords must have at least one lowercase ('a'-'z').");

    public static readonly Error PasswordRequiresUpper =
        new("PasswordRequiresUpper", "Passwords must have at least one uppercase ('A'-'Z').");
}
