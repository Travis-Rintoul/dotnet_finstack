using FinStack.API.Tests.Factories;
using FinStack.API.Tests.Helpers;
using FinStack.Contracts.Auth;
using FinStack.Contracts.Users;
using FinStack.Infrastructure.Data;

namespace FinStack.API.Tests.Endpoints.Auth;

[Collection(EndpointCollection.Definition)]
public class RegisterUserEndpointTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly HttpClient _client;

    public RegisterUserEndpointTests(TestWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
    }

    public async Task InitializeAsync()
    {
        var context = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.EnsureConnectionAsync();
        await context.TruncateAllTablesAsync();
        await context.EnsureConnectionClosedAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task RegisterUser_ShouldPass()
    {
        var dto = new RegisterUseRequestDto
        {
            Email = "test@gmail.com",
            Password = "Abc123!"
        };

        var request = await _client.PostAsResultAsync<Guid, RegisterUseRequestDto>(AuthEndpoints.RegisterUser, dto);

        Assert.True(request.IsSuccess);
        Assert.NotEqual(Guid.Empty, request.Unwrap());
    }

    [Fact]
    public async Task RegisterUser_ShouldFail_WhenPasswordNotComplex()
    {
        var dto = new RegisterUseRequestDto
        {
            Email = "test@gmail.com",
            Password = "1234"
        };

        var request = await _client.RegisterUserAsync(dto);
        var expected = new List<string>
        {
            PasswordErrors.PasswordTooShort.Code,
            PasswordErrors.PasswordRequiresNonAlphanumeric.Code,
            PasswordErrors.PasswordRequiresLower.Code,
            PasswordErrors.PasswordRequiresUpper.Code
        };

        Assert.True(request.IsFailure);
        Assert.Equal(expected, request.ErrorCodes);
    }
}
