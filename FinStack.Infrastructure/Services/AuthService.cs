using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinStack.Application.Commands;
using FinStack.Application.DTOs;
using FinStack.Application.Interfaces;
using FinStack.Common;
using FinStack.Domain.Enums;
using FinStack.Infrastructure.Commands;
using FinStack.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static FinStack.Common.Result;

namespace FinStack.Infrastructure.Services;

public class AuthService(
    UserManager<AuthUser> userManager,
    SignInManager<AuthUser> signInManager,
    IConfiguration configuration,
    IMediator mediator)
    : IAuthService
{
    public async Task<Option<AuthResponseDto>> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Option<AuthResponseDto>.None();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return Option<AuthResponseDto>.None();
        }

        var dto = new AuthUserDto { Id = Guid.Parse(user.Id), Email = user.Email };

        string? accessToken = GenerateJwtToken(dto).Match<string?>(
            token => token,
            error => {
                return null;
            }
        );

        if (accessToken == null)
        {
            return Option<AuthResponseDto>.None();
        }
        
        string refreshToken = GenerateRefreshToken();
        
        await userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", refreshToken);
        
        return Option<AuthResponseDto>.Some(new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
    }

    public async Task<Result<Guid>> RegisterAsync(RegisterUserDto dto)
    {
        var userDto = new UserDto()
        {
            Email = dto.Email,
            UserName = dto.Email,
        };

        var userResult = await mediator.Send(new CreateUserCommand(userDto));
        if (userResult.Failed(out var errors))
        {
            return Failure<Guid>(errors);
        }

        var userGuid = userResult.Value;
        var authUserDto = new AuthUserDto()
        {
            Id = userGuid,
            Email = dto.Email,
            Password = dto.Password,
            Type = UserType.User,
        };
        
        var authUserResult = await mediator.Send(new CreateAuthUserCommand(authUserDto));
        if (userResult.Failed(out var authErrors))
        {
            return Failure<Guid>(authErrors);
        }

        return Success(authUserResult.Value);
    }

    public async Task<Result<Guid>> UpdateAsync(AuthUserDto userDto)
    {
        var user = await userManager.FindByEmailAsync(userDto.Email);
        if (user == null)
        {
            return Failure<Guid>("User not found.");
        }

        if (userDto.Type != user.Type)
        {
            user.Type = userDto.Type;
        }
        
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Failure<Guid>(updateResult.Errors);
        }
        
        return Success(Guid.Parse(user.Id));
    }

    private Result<string> GenerateJwtToken(AuthUserDto user)
    {
        var jwtKey = configuration["Jwt:Key"];
        if (String.IsNullOrWhiteSpace(jwtKey))
        {
            return Failure<string>("Signing key is missing in configuration.");
        }
        
        var issuer = configuration["JWT:ValidIssuer"];
        if (String.IsNullOrWhiteSpace(issuer))
        {
            return Failure<string>("Signing issuer is missing in configuration.");
        }
        
        var audience = configuration["JWT:ValidAudience"];
        if (String.IsNullOrWhiteSpace(audience))
        {
            return Failure<string>("Signing audience is missing in configuration.");
        }
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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
