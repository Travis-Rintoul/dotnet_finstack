using FinStack.API.Tests.Factories;
using FinStack.API.Tests.Helpers;
using FinStack.Common;
using FinStack.Contracts.Auth;
using FinStack.Contracts.Users;
using FinStack.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FinStack.API.Tests.Endpoints;

[Collection(EndpointCollection.Definition)]
public class LoginUserEndpointTests : IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public LoginUserEndpointTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
    }

    public async Task InitializeAsync() => await _factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task LoginUser_ShouldPass_WhenValidLogin()
    {
        await _factory.GivenUserAsync("test@gmail.com", "Abc123!").UnwrapAsync();
        var dto = new LoginUserRequestDto
        {
            Email = "test@gmail.com",
            Password = "Abc123!"
        };

        var request = await _client.PostAsResultAsync<LoginUserResponseDto, LoginUserRequestDto>(AuthEndpoints.LoginUser, dto);
        Assert.True(request.IsSuccess);
        Assert.NotNull(request.Unwrap());
    }

    [Fact]
    public async Task LoginUser_ShouldFail_WhenInvalidPassword()
    {
        await _factory.GivenUserAsync("test@gmail.com", "Abc123!").UnwrapAsync();

        var dto = new LoginUserRequestDto
        {
            Email = "test@gmail.com",
            Password = "BADPASSWORD"
        };

        var request = await _client.LoginUserAsync(dto);
        Assert.True(request.IsFailure);
    }

    [Fact]
    public async Task LoginUser_ShouldFail_WhenInvalidEmail()
    {
        await _factory.GivenUserAsync("test@gmail.com", "Abc123!").UnwrapAsync();

        var dto = new LoginUserRequestDto
        {
            Email = "bad@email.com",
            Password = "Abc123!"
        };

        var request = await _client.LoginUserAsync(dto);
        Assert.True(request.IsFailure);
    }
}