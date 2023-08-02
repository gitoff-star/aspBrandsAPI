using dotnetJWT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnetJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public static user userObject = new user();

      [HttpPost("register")]
        ActionResult<user> register (userDTO userDTO)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(userDTO.password);

            userObject.username= userDTO.username.ToString();
            userObject.hashPassword= hash;


            return Ok(userObject);
        }
    }
}
