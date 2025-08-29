public static class UserErrors
{
    public static readonly Error NotFound =
        new("User.NotFound", "Unable to find user");

    public static readonly Error AlreadyExists =
        new("User.AlreadyExists", "User with email {email} already exists.");
}
