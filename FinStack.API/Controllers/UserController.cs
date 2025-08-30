using MediatR;
using Microsoft.AspNetCore.Mvc;
using FinStack.Application.Queries;
using FinStack.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using FinStack.Application.Commands;
using FinStack.Contracts.Users;
using System.Text.Json;

namespace FinStack.API.Controllers;

[Authorize]
[ApiController]
[Route(UserEndpoints.Controller)]
public class UserController(IMediator mediator, IRustEngineService engine) : ControllerBase
{
    [HttpGet(UserEndpoints.GetUsers)]
    public async Task<ActionResult<IEnumerable<GetUsersResponseDto>>> GetUsers()
    {
        return Ok(await mediator.Send(new GetUsersQuery()));
    }

    [HttpGet(UserEndpoints.GetUserByID)]
    public async Task<ActionResult<string>> GetUser(Guid guid)
    {
        return Ok(await mediator.Send(new GetUserByIdQuery(guid)));
    }

    [HttpPut(UserEndpoints.UpdateUser)]
    public async Task<IActionResult> UpdateUser(Guid userGuid, [FromBody] UpdateUserDto dto)
    {
        var result = await mediator.Send(new UpdateUserCommand(userGuid, dto));
        return result.Match<IActionResult>(
            guid => Ok(guid),
            (errors) => BadRequest(new ResponseMeta
            {
                Message = "Validation Failed",
                Errors = errors.Where(e => e.Severity == ErrorSeverity.Error),
                Warnings = errors.Where(e => e.Severity == ErrorSeverity.Warning),
            })
        );
    }

    [HttpDelete(UserEndpoints.DeleteUser)]
    public async Task<IActionResult> DeleteUser(Guid guid)
    {
        var result = await mediator.Send(new DeleteUserCommand(guid));
        return result.Match<IActionResult>(
            guid => Ok(guid),
            (errors) => BadRequest(new ResponseMeta
            {
                Message = "Error:",
                Errors = errors.Where(e => e.Severity == ErrorSeverity.Error),
                Warnings = errors.Where(e => e.Severity == ErrorSeverity.Warning),
            })
        );
    }

    [HttpGet(UserPreferenceEndpoints.GetUserPreferences)]
    public ActionResult<string> GetUserPreferences(Guid guid)
    {
        return Ok($"GetUserPreferences");
    }

    [HttpPut(UserPreferenceEndpoints.UpdateUserPreferences)]
    public async Task<IActionResult> UpdateUserPreferences(Guid userGuid, [FromBody] CreateUpdateUserPreferenceDto dto)
    {
        var result = await mediator.Send(new CreateUpdateUserPreferenceCommand(userGuid, dto));
        return result.Match<IActionResult>(
            _ => Ok(),
            (errors) => BadRequest(new ResponseMeta
            {
                Message = "ERROR: ",
                Errors = errors.Where(e => e.Severity == ErrorSeverity.Error),
                Warnings = errors.Where(e => e.Severity == ErrorSeverity.Warning),
            })
        );
    }

    [AllowAnonymous]
    [HttpGet("test")]
    public async Task<ActionResult> Test()
    {
        var code = "test";
        var bodyObject = new
        {
            command_name = code,
            create_job = true,
            sleep_seconds = 2,
        };

        engine.Configure(new EngineConfig {
            enviroment = "Test",
            user = "postgres",
            password = "postgres",
            host = "localhost",
            port = "5432",
            database = "finstack_api_test_db",
        });

        return Ok(engine.ProcessJob(code, JsonSerializer.Serialize(bodyObject)));
    }
}