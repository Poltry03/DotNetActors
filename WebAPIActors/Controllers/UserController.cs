using Microsoft.AspNetCore.Mvc;
using WebAPIActors.Helper;
using WebAPIActors.Models;

namespace WebAPIActors.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : Controller
    {
        [HttpPost("/api/login", Name = "Login")]
        public ActionResult Login(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest();
            }

            var loggedUser = UserHelper.LoginTry(user.Username, user.Password);
            if (loggedUser != null)
            {
                var token = "faketoken";
                return Ok(new { token = token, name = loggedUser.Username });
            }
            else
                return Unauthorized();
        }

        [HttpPost("new", Name = "CreateUser")]
        public ActionResult NewUser(User user)
        {
            if (string.IsNullOrEmpty(user.Username))
                return BadRequest("Username necessario");

            if (string.IsNullOrEmpty(user.Password))
                return BadRequest("Password necessaria");

            var existingUser = UserHelper.GetUserByUsername(user.Username);


            if (existingUser != null)
                return BadRequest("Utente già inserito");

            user.Salt = PasswordHelper.GenerateSalt();
            user.PasswordHash = PasswordHelper.HashPassword(user.Password, user.Salt);

            int result = UserHelper.Insert(user);

            if (result != 0)
                return NoContent();
            else
                return StatusCode(500, "Qualcosa è andato storto");
        }

    }
}
