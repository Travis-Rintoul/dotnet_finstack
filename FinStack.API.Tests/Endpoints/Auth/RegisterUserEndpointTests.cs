using System.Net.Http.Json;
using FinStack.Contracts.Users;

public class RegisterUserEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RegisterUserEndpointTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_ReturnsEmptyListInitially()
    {
        var users = await _client.GetUsersAsync();
        Assert.True(users.IsSuccess);
        Assert.Equal([], users.Unwrap()!);
    }

    [Fact]
    public async Task GetUsers_ReturnsUsers()
    {
        var guid = (await _client.GivenUserAsync("test@gmail.com", "Abc123!")).Unwrap();
        var expected = new List<GetUsersResponseDto>() {
            new()
            {
                UserGuid = guid,
                Email = "test@gmail.com",
                FirstName = "",
                MiddleName = "",
                LastName = "",
            }
        };
        
        var users = await _client.GetUsersAsync();
        Assert.True(users.IsSuccess);
        Assert.Equal(expected, users.Unwrap());
    }
}
