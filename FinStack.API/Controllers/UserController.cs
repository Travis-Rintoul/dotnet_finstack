using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinStack.Application.Queries;
using FinStack.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

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
        public ActionResult<string> GetUser(int id)
        {
            return Ok($"user{id}");
        }

        // POST: api/user
        [HttpPost]
        public ActionResult<string> CreateUser([FromBody] string user)
        {
            return CreatedAtAction(nameof(GetUser), new { id = 1 }, user);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] string user)
        {
            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            return NoContent();
        }

        // TODO: remove debug call to engine
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            string json = "{ \"file_name\": \"test.json\" }";
            return Ok(engine.ProcessJob("import-file", json));
        }

    }
}