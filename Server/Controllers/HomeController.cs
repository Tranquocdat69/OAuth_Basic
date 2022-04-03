using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Secret()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "subject_id"),
                new Claim("content_1", "content_1"),
                new Claim("content_2", "content_2"),
            };

            var secretKeyByte = Encoding.UTF8.GetBytes(ConfigJwt.SecretKey);
            var symetricKey = new SymmetricSecurityKey(secretKeyByte);

            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredential = new SigningCredentials(symetricKey, algorithm);

            var token = new JwtSecurityToken(
                ConfigJwt.Issuer,
                ConfigJwt.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(1),
                signingCredential
                );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {access_token = tokenJson});
        }

        [HttpGet]
        public IActionResult Decode(string part)
        {
            byte[] byteResult = Convert.FromBase64String(part);
            string result = Encoding.UTF8.GetString(byteResult);

            return Ok(result);
        }
    }
}
