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
            errors => BadRequest(new ResponseMeta
            {
                Code = 400,
                Message = "There was an error register...",
                Errors = errors
            })
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        return (await authService.LoginAsync(dto.Email, dto.Password)).Match<IActionResult>(
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