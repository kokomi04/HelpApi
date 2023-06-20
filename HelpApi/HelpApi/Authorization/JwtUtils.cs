using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HelpApi.Models;
using HelpApi.EF;

namespace HelpApi.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _config;

        public JwtUtils(IConfiguration config)
        {
            _config = config;

            if (string.IsNullOrEmpty(_config["Jwt:Issuer"]))
                throw new Exception("JWT secret not configured");
        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //If you've had the login module, you can also use the real user information here
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                     issuer: _config["Jwt:Issuer"],
                     audience: _config["Jwt:Issuer"],
                     claims: claims,
                     expires: DateTime.Now.AddMinutes(30),
                     signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
