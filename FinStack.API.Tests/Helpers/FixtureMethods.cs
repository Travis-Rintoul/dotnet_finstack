using System.Net.Http.Json;
using FinStack.Application.DTOs;
using FinStack.Common;
using FinStack.Contracts.Auth;
using FinStack.Contracts.Users;

namespace FinStack.API.Tests.Helpers;

public static class FixtureMethods
{
    public static async Task<Result<Guid>> GivenUserAsync(
        this HttpClient client,
        string email,
        string password)
    {
        var request = new RegisterUseRequestDto
        {
            Email = email,
            Password = password
        };

        return await client.RegisterUserAsync(request);
    }
}
