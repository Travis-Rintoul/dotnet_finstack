using System.Data;
using FinStack.API.Tests.Handlers;
using FinStack.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FinStack.API.Tests.Factories;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private string? dbConnectionString;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddJsonFile("appsettings.Test.json");
        });

        builder.ConfigureServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            // Deregister existing service
            var AppDbContextService = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(AppDbContextService);

            using (var provider = services.BuildServiceProvider())
            {
                var configuration = provider.GetRequiredService<IConfiguration>();

                dbConnectionString = configuration.GetConnectionString("Test")
                    ?? throw new InvalidOperationException("Missing ConnectionStrings:Test in appsettings.Test.json");
            }

            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseNpgsql(dbConnectionString);
                opts.EnableDetailedErrors();
                opts.EnableSensitiveDataLogging();
            });

            var hosted = services.Where(s => typeof(IHostedService).IsAssignableFrom(s.ServiceType)).ToList();
            foreach (var svc in hosted)
            {
                services.Remove(svc);
            }
        });
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;

    public async Task ResetDatabaseAsync()
    {
        if (string.IsNullOrWhiteSpace(dbConnectionString))
        {
            throw new InvalidOperationException("Factory not initialized.");
        }

        await using (var connection = new NpgsqlConnection(dbConnectionString))
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            const string sql = @"
                DO $$
                DECLARE
                    stmt text;
                BEGIN
                    SELECT
                        'TRUNCATE TABLE ' ||
                        string_agg(format('%I.%I', schemaname, tablename), ', ')
                        || ' RESTART IDENTITY CASCADE;'
                    INTO stmt
                    FROM pg_tables
                    WHERE schemaname = 'public'
                    AND tablename <> '__EFMigrationsHistory';

                    IF stmt IS NOT NULL THEN
                        EXECUTE stmt;
                    END IF;
                END $$;";

            await using (var command = new NpgsqlCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}