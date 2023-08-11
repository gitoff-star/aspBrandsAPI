using dotnetJWT.Models;
using JWTwebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWTwebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        IConfiguration configuration;
        DBContextcs _db;
        public LoginsController(DBContextcs db, IConfiguration _configuration)
        {
            _db = db;
            configuration = _configuration;
        }

        
     
        private user validateUser(user userDTO)
        {


            var LUser = _db.Users.Where(u => u.username.Equals(userDTO.username) 
            && u.hashPassword.Equals(userDTO.hashPassword)).FirstOrDefault();

            if (LUser !=null )
            {
                LUser = new user { username = LUser.username };
            }

            return LUser;
        }

        private string GenerateToken(user user)
        {
            var secuirtykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(secuirtykey, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:audience"], null,
                expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }


        [HttpPost("login")]
        public IActionResult loginNow(user user)
        {

            IActionResult response = Unauthorized();

            if (validateUser(user) != null)
            {
                var token = GenerateToken(user);
                response = Ok(new { token = token });
            }

            return response;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(user user)
        {

          

            if (user.username != null && user.hashPassword!= null)
            {
                 _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
