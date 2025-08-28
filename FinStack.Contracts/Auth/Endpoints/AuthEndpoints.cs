
using static FinStack.Contracts.ApiVersions;

namespace FinStack.Contracts.Users
{
    public static class AuthEndpoints
    {
        public const string Controller = v1 + "/auth";
        public const string RegisterUser = v1 + "/auth/register";
        public const string LoginUser = v1 + "/auth/login";
    }
}