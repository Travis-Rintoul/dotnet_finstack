using System.Net.Http.Json;
using FinStack.Common;
using FinStack.Contracts.Users;

public static class FixtureMethods
{
    public static async Task<Result<Guid>> GivenUserAsync(
        this HttpClient client,
        string email,
        string password)
    {
        var request = new
        {
            Email = email,
            Password = password
        };

        var response = await client.PostAsJsonAsync(AuthEndpoints.RegisterUser, request);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseMeta>();

            if (result == null)
            {
                throw new Exception($"{response.StatusCode}: {response.Content}");
            }

            return Result.Failure<Guid>(result.Errors);
        }

        return Result.Success(await response.Content.ReadFromJsonAsync<Guid>());
    }
}
