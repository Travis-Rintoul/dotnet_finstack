
using static FinStack.Contracts.ApiVersions;

namespace FinStack.Contracts.Users
{
    public static class UserPreferenceEndpoints
    {
        public const string GetUserPreferences = v1 + "/users/{guid}/preferences";
        public const string UpdateUserPreferences = v1 + "/users/{guid}/preferences";
    }
}