using Api_Identity_JWT.Data;
using Api_Identity_JWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Identity_JWT.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;

        public AuthController(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody]Login model)
        {
            var user = _db.registers.Where(x=>x.Username== model.Username && x.Password==model.Password).FirstOrDefault();
            if(user == null)
            {
                return BadRequest("Username Or Password Invalid");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Username),new Claim (ClaimTypes.Role,"Admin"), new Claim(ClaimTypes.Role, "HR") } ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encrypterToken = tokenHandler.WriteToken(token); 

            return Ok(new {token = encrypterToken,username = user.Username});
        }

      [HttpPost("Register")]
      public IActionResult Register([FromBody]Register model)
        {
            var reg = new Register();
            if (model!=null)
            {
                reg.Username = model.Username;
                reg.Password = model.Password;
                _db.registers.Add(reg);
                _db.SaveChanges();
            }
            else
            {
                return BadRequest("Error! Please Try Again");
            }
           return Ok(reg);
        }
    }
}
