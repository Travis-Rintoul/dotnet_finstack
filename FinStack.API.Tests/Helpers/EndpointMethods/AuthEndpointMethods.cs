using FinStack.Common;
using FinStack.Contracts.Auth;
using FinStack.Contracts.Users;

namespace FinStack.API.Tests.Helpers;

public static class AuthEndpointMethods
{
    public static async Task<Result<Guid>> RegisterUserAsync(this HttpClient client, RegisterUseRequestDto request)
    {
        return await client.PostAsResultAsync<Guid, RegisterUseRequestDto>(AuthEndpoints.RegisterUser, request);
    }

    public static async Task<Result<LoginUserResponseDto>> LoginUserAsync(this HttpClient client, LoginUserRequestDto request)
    {
        return await client.PostAsResultAsync<LoginUserResponseDto, LoginUserRequestDto>(AuthEndpoints.LoginUser, request);
    }
}
