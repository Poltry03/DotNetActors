using Microsoft.AspNetCore.Mvc;
using WebAPIActors.DTOs;
using WebAPIActors.Helper;
using WebAPIActors.Models;

namespace WebAPIActors.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : Controller
    {
        [HttpPost("/api/login", Name = "Login")]
        public ActionResult Login(LoginDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest();
            }

            var loggedUser = UserHelper.LoginTry(user.Username, user.Password);
            if (loggedUser != null)
            {
                var token = JwtHelper.GenerateToken(loggedUser);
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

        [HttpPost("reset", Name = "ResetUser")]
        public ActionResult Reset(ResetDto reset)
        {
            //var token = JwtHelper.GenerateToken(resetUser + DateTime.Now.ToString);

            if (string.IsNullOrWhiteSpace(reset.Username))
            {
                return BadRequest();
            }

            var existingUser = UserHelper.GetUserByUsername(reset.Username);

            if (existingUser == null)
                return BadRequest($"Unable to reset user {reset.Username}");


            var token = "faketoken";

            var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";


            return Ok($"reset?={token}");
        }

        [HttpPatch("reset", Name = "ChangePwd")]
        public ActionResult ChangePwd(ChangePasswordDto change)
        {

            if (string.IsNullOrEmpty(change.ResetToken))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(change.NewPassword))
                return BadRequest();

            var foundUser = UserHelper.GetUserByResetToken(change.ResetToken);

            if (foundUser == null)
                return BadRequest("Something went wrong");

            foundUser.PasswordHash = PasswordHelper.HashPassword(change.NewPassword, foundUser.Salt);

            bool result = UserHelper.Update(foundUser);

            if (result)
                return Ok();


            return StatusCode(500, "Qaulcosa è andato storto");

        }

    }
}
