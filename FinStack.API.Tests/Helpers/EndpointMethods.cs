using System.Net.Http.Json;
using FinStack.Common;
using FinStack.Contracts.Users;
using FinStack.Domain.Entities;

public static class EndpointMethods
{
    public static async Task<Result<GetUsersResponseDto[]>> GetUsersAsync(this HttpClient client)
    {
        var response = await client.GetAsync(UserEndpoints.GetUsers);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Result.Failure<GetUsersResponseDto[]>(response.StatusCode.ToString(), content);
        }

        var users = await response.Content.ReadFromJsonAsync<GetUsersResponseDto[]>();
        if (users == null)
        {
            return Result.Failure<GetUsersResponseDto[]>("ERROR", "Unable to Deserialize users");
        }

        return Result.Success(users);
    }
}
