using System.Data;
using FinStack.API.Tests.Handlers;
using FinStack.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace FinStack.API.Tests.Factories;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private string? dbConnectionString;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddJsonFile("appsettings.Test.json", optional: false);
        });

        builder.ConfigureServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();
            services.RemoveAll<IDbContextFactory<AppDbContext>>();
            services.RemoveAll<Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration<AppDbContext>>();

            var poolDescriptors = services
                .Where(d =>
                    d.ServiceType.FullName == "Microsoft.EntityFrameworkCore.Internal.IDbContextPool`1" ||
                    d.ImplementationType?.FullName == "Microsoft.EntityFrameworkCore.Internal.DbContextPool`1")
                .ToList();

            foreach (var d in poolDescriptors)
            {
                services.Remove(d);
            }

            using (var provider = services.BuildServiceProvider())
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                dbConnectionString = configuration.GetConnectionString("Test")
                    ?? throw new InvalidOperationException("Missing ConnectionStrings:Test in appsettings.Test.json");
            }

            services.AddPooledDbContextFactory<AppDbContext>(opts =>
            {
                opts.UseNpgsql(dbConnectionString);
                opts.EnableDetailedErrors();
                opts.EnableSensitiveDataLogging();
            });

            services.AddScoped(sp =>
                sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

            var hosted = services.Where(s => typeof(IHostedService).IsAssignableFrom(s.ServiceType)).ToList();
            foreach (var svc in hosted)
            {
                services.Remove(svc);
            }
        });
    }

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