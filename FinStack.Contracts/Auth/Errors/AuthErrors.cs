

namespace FinStack.Contracts.Auth;

public static class AuthErrors
{
    public static readonly Error NotFound =
        new("User.NotFound", "User with id {id} was not found.");

    public static readonly Error AlreadyExists =
        new("User.AlreadyExists", "User with email {email} already exists.");

    public static readonly Error LoginFailed =
        new("Auth.LoginFailed", "Login failed. Please verify your email and password.");

    public static readonly Error JWT_KEY_MISSING =
        new("JWT_KEY_MISSING", "Signing key is missing in configuration.");

    public static readonly Error JWT_ISSUER_MISSING =
        new("JWT_ISSUER_MISSING", "Signing issuer is missing in configuration.");

    public static readonly Error JWT_AUDIENCE_MISSING =
        new("JWT_AUDIENCE_MISSING", "Signing audience is missing in configuration.");
}
