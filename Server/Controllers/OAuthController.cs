using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize(
            string response_type,
            string client_id,
            string redirect_uri,
            string scope,
            string state)
        {
            var query = new QueryBuilder();
            query.Add("redirect_uri", redirect_uri);
            query.Add("state", state);

            return View(model: query);
        }

        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirect_uri,
            string state)
        {
            var code = "123456-abcxyz";

            var query = new QueryBuilder();
            query.Add("state", state);
            query.Add("code", code);

            return Redirect(redirect_uri+query);
        }

        [HttpPost]
        public IActionResult Token(
            string grant_type,
            string code,
            string redirect_uri,
            string client_id)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "subject_id"),
                new Claim("content_1", "content 1"),
                new Claim("content_2", "content 2"),
                new Claim("content_3", "content 3"),
                new Claim("content_4", "content 4"),
                new Claim("content_5", "content 5"),
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
                expires: DateTime.Now.AddMilliseconds(1),
                signingCredential
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                access_token = accessToken,
                token_type = "Bearer",
                raw_claim = "OAuthClaim"
            };

            //var responseJson = JsonConvert.SerializeObject(responseObject);
            //var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            //await Response.Body.WriteAsync(responseBytes);

            return Ok(responseObject);
        }

        [HttpGet]
        public IActionResult ValidateToken()
        {
            if (HttpContext.Request.Query.TryGetValue("access_token", out var accessToken))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
