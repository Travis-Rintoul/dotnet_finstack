
using static FinStack.Contracts.ApiVersions;

namespace FinStack.Contracts.Users
{
    public static class UserEndpoints
    {
        public const string Controller = v1 + "/users";
        public const string GetUsers = v1 + "/users";
        public const string GetUserByID = v1 + "/users/{guid}";
        public const string UpdateUser = v1 + "/users/{guid}";
        public const string DeleteUser = v1 + "/users/{guid}";
    }
}