using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using art_gallery_api.Models;
using art_gallery_api.Persistence;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserDataAccess _userDataAccess;

        public UsersController(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        // GET: api/users
        [HttpGet(), Authorize(Policy = "UserOnly")]
        public IActionResult GetUsers()
        {
            var users = _userDataAccess.GetUsers();
            return Ok(users);
        }

        // GET: api/users/admin
        [HttpGet("admin"), Authorize(Policy = "AdminOnly")]
        public IActionResult GetAdminUsers()
        {
            var adminUsers = _userDataAccess.GetAdminUsers();
            return Ok(adminUsers);
        }

        // GET: api/users/{id}
        [HttpGet("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult GetUser(int id)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost, Authorize(Policy = "AdminOnly")]
        // [HttpPost]
        public IActionResult PostUser([FromBody] UserModel userModel)
        {
            // Hash the password with BCrypt
            userModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.PasswordHash);

            // Set creation and modification dates
            userModel.CreatedDate = DateTime.Now;
            userModel.ModifiedDate = DateTime.Now;

            _userDataAccess.AddUser(userModel);

            // Return the created user with a reference to GET by ID
            return CreatedAtAction("GetUser", new { id = userModel.Id }, userModel);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult PutUser(int id, [FromBody] UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return BadRequest();
            }

            // Preserve the password hash and email
            var existingUser = _userDataAccess.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            userModel.PasswordHash = existingUser.PasswordHash;
            userModel.Email = existingUser.Email;

            _userDataAccess.UpdateUser(id, userModel);
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            _userDataAccess.DeleteUser(id);
            return NoContent();
        }

        // PATCH: api/users/{id}
        [HttpPatch("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult PatchUser(int id, [FromBody] LoginModel loginModel)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user's email and hash new password
            user.Email = loginModel.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Password);

            // Update the user in the database
            _userDataAccess.UpdateUser(id, user);

            // Confirm the update
            return NoContent();
        }
    }
}
