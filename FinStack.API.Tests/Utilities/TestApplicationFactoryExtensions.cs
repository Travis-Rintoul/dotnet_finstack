using FinStack.API.Tests.Factories;

public static class TestApplicationFactoryExtensions
{
    public static async Task RunScopedAsync<T>(
        this TestWebApplicationFactory factory,
        Func<T, Task> func)
        where T : notnull
    {
        using var scope = factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        await func(service);
    }

    public static async Task<TResult> RunScopedAsync<T, TResult>(
        this TestWebApplicationFactory factory,
        Func<T, Task<TResult>> func)
        where T : notnull
    {
        using var scope = factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        return await func(service);
    }
}