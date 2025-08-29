using FinStack.API.Tests.Factories;
using FinStack.API.Tests.Helpers;
using FinStack.Contracts.Users;
using FinStack.Infrastructure.Data;

namespace FinStack.API.Tests.Endpoints.User;

[Collection(EndpointCollection.Definition)]
public class GetUsersEndpointTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public GetUsersEndpointTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
    }

    public async Task InitializeAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.EnsureConnectionAsync();
            await context.TruncateAllTablesAsync();
            await context.EnsureConnectionClosedAsync();
        }
    }

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
