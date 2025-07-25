using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // GET: api/user
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetUsers()
        {
            return Ok(new List<string> { "user1", "user2", "user3", "user4" });
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