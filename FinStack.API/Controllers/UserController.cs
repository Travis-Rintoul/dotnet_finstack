using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinStack.Application.Queries;
using FinStack.Application.Dtos;

namespace FinStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return Ok(await _mediator.Send(new GetUsersQuery()));
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
    }
}