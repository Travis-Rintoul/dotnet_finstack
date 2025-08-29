public static class UserErrors
{
    public static readonly Error NotFound =
        new("User.NotFound", "User with id {id} was not found.");

    public static readonly Error AlreadyExists =
        new("User.AlreadyExists", "User with email {email} already exists.");
}
