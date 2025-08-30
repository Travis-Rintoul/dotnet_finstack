using Xunit; 
using FinStack.API.Tests.Factories;
using FinStack.API.Tests.Helpers;
using FinStack.Contracts.Users;

namespace FinStack.API.Tests.Endpoints;

[Collection(EndpointCollection.Definition)]
public class GetUsersEndpointTests : IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GetUsersEndpointTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
    }

    public async Task InitializeAsync() => await _factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

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
