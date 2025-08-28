using System.Net.Http.Json;

public class GetUsersEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetUsersEndpointTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
    }

    [Fact]
    public async Task GetUsers_ReturnsEmptyListInitially()
    {
        var response = await _client.GetAsync("/api/v1/user");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("[]", content);
    }

    [Fact]
    public async Task GetUsers_ReturnsUsers()
    {
        await _client.GivenUserAsync();

        var response = await _client.GetAsync("/api/v1/user");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("[]", content);
    }
}
