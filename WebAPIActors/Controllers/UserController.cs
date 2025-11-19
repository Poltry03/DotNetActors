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
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
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

    }
}
