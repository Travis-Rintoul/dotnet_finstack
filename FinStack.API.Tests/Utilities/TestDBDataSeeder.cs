using FinStack.API.Tests.Factories;
using FinStack.Application.DTOs;
using FinStack.Application.Interfaces;
using FinStack.Common;

public static class TestDBDataSeeder
{
    public static async Task<Result<Guid>> GivenUserAsync(this TestWebApplicationFactory factory, string email, string password)
    {
        return await factory.RunScopedAsync<IAuthService, Result<Guid>>(authService =>
        {
            var dto = new RegisterUserDto
            {
                Email = email,
                Password = password
            };

            return authService.RegisterAsync(dto);
        });
    }
}