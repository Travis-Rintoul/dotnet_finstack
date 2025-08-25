using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinStack.Application.Queries;
using FinStack.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using FinStack.Application.Commands;

namespace FinStack.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController(IMediator mediator, IEngineService engine) : ControllerBase
    {
        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return Ok(await mediator.Send(new GetUsersQuery()));
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetUser(Guid guid)
        {
            return Ok(await mediator.Send(new GetUserByIdQuery(guid)));
        }

        // TODO: look at if these are needed, register might do the job
        // POST: api/user
        [HttpPost]
        public ActionResult<string> CreateUser([FromBody] string user)
        {
            return CreatedAtAction(nameof(GetUser), new { id = 1 }, user);
        }

        // PUT: api/user/{guid}
        [HttpPut("{guid}")]
        public async Task<IActionResult> UpdateUser(Guid userGuid, [FromBody] UpdateUserDto dto)
        {
            var result = await mediator.Send(new UpdateUserCommand(userGuid, dto));
            return result.Match<IActionResult>(
                guid => Ok(guid),
                (errors) => BadRequest(new ResponseMeta
                {
                    Code = 400,
                    Message = "Validation Failed",
                    Errors = errors.Where(e => e.Severity == ErrorSeverity.Error),
                    Warnings = errors.Where(e => e.Severity == ErrorSeverity.Warning),
                })
            );
        }

        // DELETE: api/user/{id}
        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteUser(Guid guid)
        {
            var result = await mediator.Send(new DeleteUserCommand(guid));
            return result.Match<IActionResult>(
                guid => Ok(guid),
                (errors) => BadRequest(new ResponseMeta
                {
                    Code = 400,
                    Message = "Error:",
                    Errors = errors.Where(e => e.Severity == ErrorSeverity.Error),
                    Warnings = errors.Where(e => e.Severity == ErrorSeverity.Warning),
                })
            );
        }

        // GET: api/user/{UserGUID}/preferences
        [HttpGet("{guid}/preferences")]
        public ActionResult<string> GetUserPreferences(Guid guid)
        {
            return Ok($"GetUserPreferences");
        }

        // PUT: api/user/preferences
        [HttpPut("{guid}/preferences")]
        public async Task<IActionResult> UpdateUserPreferences(Guid userGuid, [FromBody] CreateUpdateUserPreferenceDto dto)
        {
            var result = await mediator.Send(new CreateUpdateUserPreferenceCommand(userGuid, dto));
            return result.Match<IActionResult>(
                _ => Ok(),
                (errors) => BadRequest(new ResponseMeta
                {
                    Code = 400,
                    Message = "ERROR: ",
                    Errors = errors.Where(e => e.Severity == ErrorSeverity.Error),
                    Warnings = errors.Where(e => e.Severity == ErrorSeverity.Warning),
                })
            );
        }

        // TODO: remove debug call to engine
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            string json = "{ \"command_name\": \"import-file\", \"file_name\": \"test.json\" }";
            return Ok(engine.ProcessJob("import-file", json));
        }
    }
}