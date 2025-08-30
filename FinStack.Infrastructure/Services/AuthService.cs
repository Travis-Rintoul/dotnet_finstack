using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinStack.Application.Commands;
using FinStack.Application.DTOs;
using FinStack.Application.Interfaces;
using FinStack.Common;
using FinStack.Contracts.Auth;
using FinStack.Domain.Entities;
using FinStack.Domain.Enums;
using FinStack.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static FinStack.Common.Result;

namespace FinStack.Infrastructure.Services;

public class AuthService(
    UserManager<AuthUser> userManager,
    SignInManager<AuthUser> signInManager,
    IConfiguration configuration,
    IMediator mediator,
    AppDbContext dbContext)
    : IAuthService
{
    public async Task<Result<LoginUserResponseDto>> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Failure<LoginUserResponseDto>(UserErrors.NotFound);
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return Failure<LoginUserResponseDto>(AuthErrors.LoginFailed);
        }

        var dto = new AuthUserDto { UserGuid = user.Id, Email = user.Email };

        string? accessToken = GenerateJwtToken(dto).Match<string?>(
            token => token,
            errors =>
            {
                return null;
            }
        );

        if (accessToken == null)
        {
            return Failure<LoginUserResponseDto>(AuthErrors.LoginFailed);
        }

        string refreshToken = GenerateRefreshToken();

        await userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", refreshToken);

        return Success(new LoginUserResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
    }

    public async Task<Result<Guid>> RegisterAsync(RegisterUserDto dto)
    {
        var exists = await dbContext.Users.AnyAsync(x => x.Email == dto.Email);
        if (exists)
        {
            return Failure<Guid>(UserErrors.AlreadyExists);
        }

        var authUserDto = new AuthUserDto
        {
            Email = dto.Email,
            Password = dto.Password,
            UserType = UserType.Individual,
        };

        var authUserResult = await mediator.Send(new CreateAuthUserCommand(authUserDto));
        if (authUserResult.Failed(out var authErrors))
            return Failure<Guid>(authErrors);

        var userDto = new UserDto
        {
            UserGuid = authUserResult.Unwrap(),
            Email = dto.Email,
            FirstName = "",
            LastName = ""
        };

        var userResult = await mediator.Send(new CreateUserCommand(userDto));
        return userResult.Match(Success, Failure<Guid>);
    }

    public async Task<Result<Guid>> UpdateAsync(AuthUserDto userDto)
    {
        var user = await userManager.FindByEmailAsync(userDto.Email);
        if (user == null)
        {
            return Failure<Guid>(Error.UserNotFound(userDto.UserGuid));
        }

        if (userDto.UserType != user.UserType)
        {
            user.UserType = userDto.UserType;
        }

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Failure<Guid>(updateResult.Errors);
        }

        return Success(user.Id);
    }

    private Result<string> GenerateJwtToken(AuthUserDto user)
    {
        var jwtKey = configuration["Jwt:Key"];
        var errors = new List<Error>();
        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            errors.Add(AuthErrors.JWT_KEY_MISSING);
        }

        var issuer = configuration["JWT:ValidIssuer"];
        if (string.IsNullOrWhiteSpace(issuer))
        {
            errors.Add(AuthErrors.JWT_ISSUER_MISSING);
        }

        var audience = configuration["JWT:ValidAudience"];
        if (string.IsNullOrWhiteSpace(audience))
        {
            errors.Add(AuthErrors.JWT_AUDIENCE_MISSING);
        }

        if (errors.Any())
        {
            return Failure<string>(errors);
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserGuid.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.UserGuid.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(1),
            claims: claims,
            audience: audience,
            issuer: issuer,
            signingCredentials: creds);

        return Success(new JwtSecurityTokenHandler().WriteToken(token));
    }

    private static string GenerateRefreshToken(int size = 32)
    {
        var randomBytes = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
