using FinStack.Application.DTOs;
using FinStack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinStack.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        return (await authService.RegisterAsync(dto)).Match<IActionResult>(
            guid => Ok(guid),
            BadRequest
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await authService.LoginAsync(dto.Email, dto.Password);
        return token.Match<IActionResult>(
            Ok,
            BadRequest
        );
    }
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}