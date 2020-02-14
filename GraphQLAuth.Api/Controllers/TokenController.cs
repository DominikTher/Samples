using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GraphQLAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpGet("app1")]
        public IActionResult GetJWTTokenApp1()
        {
            return Ok(GetToken("SampleApp1"));
        }

        [HttpGet("app2")]
        public IActionResult GetJWTTokenApp2()
        {
            return Ok(GetToken("SampleApp2"));
        }

        private string GetToken(string audience)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zXyi0iWkUcJ2XlCdZ5NscHJBwioXQl"));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "dominik.ther@fake.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("reg", "1"),
                new Claim("reg", "2"),
                new Claim("age", "22")
            };

            var token = new JwtSecurityToken(
                "Dominik",
                audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(300),
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //[Authorize]
        //[HttpGet("other")]
        //public IActionResult GetOther()
        //{
        //    return Ok("Ok");
        //}
    }
}