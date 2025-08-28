using System.Net.Http.Json;
using FinStack.Domain.Entities;

public static class TestHelpers
{
    public static async Task<Guid> GivenUserAsync(
        this HttpClient client,
        string email = "test@example.com",
        string password = "Password123!")
    {
        var request = new
        {
            Email = email,
            Password = password
        };

        var response = await client.PostAsJsonAsync("/auth/register", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}
