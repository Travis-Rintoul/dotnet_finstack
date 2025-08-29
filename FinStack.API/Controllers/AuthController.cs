using FinStack.Application.DTOs;
using FinStack.Application.Interfaces;
using FinStack.Contracts.Auth;
using FinStack.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace FinStack.API.Controllers;

[ApiController]
[Route(AuthEndpoints.Controller)]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost(AuthEndpoints.RegisterUser)]
    public async Task<ActionResult<Guid>> Register([FromBody] RegisterUserDto dto)
    {
        return (await authService.RegisterAsync(dto)).Match<ActionResult<Guid>>(
            guid => Ok(guid),
            errors => BadRequest(new ResponseMeta
            {
                Message = "Unable to register user.",
                Errors = errors
            })
        );
    }

    [HttpPost(AuthEndpoints.LoginUser)]
    public async Task<ActionResult<LoginUserResponseDto>> Login([FromBody] LoginUserRequestDto dto)
    {
        return (await authService.LoginAsync(dto.Email, dto.Password)).Match<ActionResult<LoginUserResponseDto>>(
            response => Ok(response),
            errors => BadRequest(new ResponseMeta
            {
                Message = "Unable to authenticate.",
                Errors = errors
            })
        );
    }
}

