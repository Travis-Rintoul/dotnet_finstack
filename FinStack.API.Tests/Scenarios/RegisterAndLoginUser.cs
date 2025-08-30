using Xunit;
using FinStack.API.Tests.Factories;
using FinStack.API.Tests.Helpers;
using FinStack.Contracts.Users;
using FinStack.Common;
using FinStack.Contracts.Auth;
using System.Net.Http.Headers;

namespace FinStack.API.Tests.Endpoints;

[Collection(EndpointCollection.Definition)]
public class RegisterAndLoginUserScenarioTests : IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RegisterAndLoginUserScenarioTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync() => await _factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task UserRegistrationScenario_ShouldPass()
    {
        var registerReponse = await _client.RegisterUserAsync(new RegisterUseRequestDto
        {
            Email = "test@email.com",
            Password = "Abc123!"
        });

        Assert.True(registerReponse.IsSuccess);

        Guid userGuid = registerReponse.Unwrap();

        var loginResponse = await _client.LoginUserAsync(new LoginUserRequestDto
        {
            Email = "test@email.com",
            Password = "Abc123!"
        });

        Assert.True(loginResponse.IsSuccess);

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResponse.Unwrap().AccessToken);

        var usersResponse = await _client.GetUsersAsync();
        Assert.True(usersResponse.IsSuccess);
        Assert.Contains(usersResponse.Unwrap(), u => u.UserGuid == userGuid);

        _client.DefaultRequestHeaders.Authorization = null;
    }
}
