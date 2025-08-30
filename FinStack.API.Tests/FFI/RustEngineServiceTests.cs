using Xunit; 
using FinStack.API.Tests.Factories;
using FinStack.API.Tests.Helpers;
using FinStack.Contracts.Users;
using System.Text.Json;
using FinStack.Infrastructure.Data;
using FinStack.Domain.Repositories;
using System.Threading.Tasks;

namespace FinStack.API.Tests.Endpoints;

[Collection(EndpointCollection.Definition)]
public class RustEngineServiceTests : IAsyncLifetime
{
    private readonly IJobRepository _jobs;
    private readonly IRustEngineService _engine;
    private readonly TestWebApplicationFactory _factory;

    public RustEngineServiceTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        using (var scope = _factory.Services.CreateScope())
        {
            _jobs = scope.ServiceProvider.GetRequiredService<IJobRepository>();
            _engine = scope.ServiceProvider.GetRequiredService<IRustEngineService>();
        }
    }

    public async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();

        var rc = _engine.Configure(new EngineConfig {
            enviroment = "Test",
            user = "postgres",
            password = "postgres",
            host = "localhost",
            port = "5432",
            database = "finstack_api_test_db",
        });

        if (rc != 0)
        {
            throw new InvalidOperationException($"configure failed: {rc}");
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task SchedulesJob()
    {
        var code = "test";
        var bodyObject = new
        {
            command_name = code,
            create_job = true,
            sleep_seconds = 2,
        };

        var guid = _engine.ProcessJob(code, JsonSerializer.Serialize(bodyObject));

        Console.WriteLine(guid);

        var job = await _jobs.PollAsync(guid);

        Assert.Equal(job.Unwrap().Guid, guid);
    }
}
