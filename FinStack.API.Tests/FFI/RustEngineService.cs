using Xunit; 
using FinStack.API.Tests.Factories;
using FinStack.API.Tests.Helpers;
using FinStack.Contracts.Users;

namespace FinStack.API.Tests.Endpoints;

[Collection(EndpointCollection.Definition)]
public class RustEngineService : IAsyncLifetime
{
    private readonly IRustEngineService _engine;
    private readonly TestWebApplicationFactory _factory;

    public RustEngineService(TestWebApplicationFactory factory)
    {
        _factory = factory;
        using var scope = _factory.Services.CreateScope();
        _engine = scope.ServiceProvider.GetRequiredService<IRustEngineService>();
    }

    public async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync(); 
        var rc = _engine.Configure(new {
            mode = "Test",
            connection_string = "Host=localhost;Port=5432;Database=finstack_api_test_db;Username=postgres",
        });

        if (rc != 0)
        {
            throw new InvalidOperationException($"configure failed: {rc}");
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void SchedulesJob()
    {
        var code = "ImportTransactions";
        var body = "{\"accountId\":\"abc\",\"since\":\"2025-08-01\"}";
        int rc = _engine.ProcessJob(code, body);
        Assert.Equal(0, rc); // or whatever success code you return
    }
}
