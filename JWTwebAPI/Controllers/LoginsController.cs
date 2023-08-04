using dotnetJWT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWTwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        IConfiguration configuration;
        public LoginsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public userDTO validateUser(userDTO userDTO)
        {
            userDTO user = null;
            if (userDTO.username == "admin" && userDTO.password == "1234")
            {
                user = new userDTO { username = "admin " };
            }

            return user;
        }

        public string GenerateToken(userDTO user)
        {
            var secuirtykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(secuirtykey, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:audience"], null,
                expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }


        [HttpPost]
        public IActionResult loginNow(userDTO user)
        {

            IActionResult response = Unauthorized();

            if (validateUser(user) != null)
            {
                var token = GenerateToken(user);
                response = Ok(new { token = token });
            }

            return response;
        }
    }
}
