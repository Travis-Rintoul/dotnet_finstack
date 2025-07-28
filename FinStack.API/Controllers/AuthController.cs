using FinStack.Application.Interfaces;
using FinStack.Infrastructure.Services;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace FinStack.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginDto dto)
    {
        var result = await authService.RegisterAsync(dto.Email, dto.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await authService.LoginAsync(dto.Email, dto.Password);
        return token.Match<IActionResult>(
            Some: Ok,
            None: () => Unauthorized("Invalid credentials")
        );
    }
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}