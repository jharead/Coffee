using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coffee.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Coffee.Controllers

{
    [Route("api/auth")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthController : Controller
    {
        private readonly CoffeeContext _context;

        public AuthController(CoffeeContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public ActionResult<Login> CheckUser(Login user)
        {
            var LoginQuery = _context.User.FirstOrDefault(u => u.UserName == user.Username && u.Password == user.Password);
            if (LoginQuery!=null)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, LoginQuery.Role) 
                    },
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost, Route("register")]
        public IActionResult InsertUser([FromBody] Registration registration)
        {
            var LoginQuery = _context.User.FirstOrDefault(u => u.UserName == registration.UserName);
            if (LoginQuery != null)
            {
                return BadRequest();
            }
            else
            { 
                _context.User.Add(registration);
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpPatch, Route("EditUser")]
        public IActionResult ModifyUser([FromBody] Registration registration)
        {
            _context.User.Update(registration);
            _context.SaveChanges();
            return Ok();
        }
    }
}
