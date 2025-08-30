using FinStack.Common;
using FinStack.Contracts.Users;

namespace FinStack.API.Tests.Helpers;

public static class UserEndpointMethods
{
    public static async Task<Result<IEnumerable<GetUsersResponseDto>>> GetUsersAsync(this HttpClient client)
    {
        return await client.GetAsResultAsync<IEnumerable<GetUsersResponseDto>>(UserEndpoints.GetUsers);
    }
}
