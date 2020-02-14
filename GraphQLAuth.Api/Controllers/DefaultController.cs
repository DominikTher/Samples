using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GraphQLAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet("gettoken")]
        public IActionResult GetJWTToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zXyi0iWkUcJ2XlCdZ5NscHJBwioXQl"));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "dominik.ther@fake.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("app", "1"),
                new Claim("app", "2"),
                //new Claim("age", "22")
        };

            var token = new JwtSecurityToken(
                "Dominik",
                "SampleApp",
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(300),
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        //[Authorize]
        //[HttpGet("other")]
        //public IActionResult GetOther()
        //{
        //    return Ok("Ok");
        //}
    }
}