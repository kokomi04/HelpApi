using Infrastructures.AppConfigs.Model;
using Infrastructures.AppConfigs.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructures.Utilities
{
    public static class JwtUtils
    {
        public static string GenerateJwtToken(AppSetting _appSettings)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.ExternalHelpApiKey));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, _appSettings.Identity.ApiName),
                new Claim(ClaimTypes.Uri, _appSettings.Identity.Endpoint)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),

                SigningCredentials = signingCredentials,

                Issuer = _appSettings.Authentication.ValidIssuer,
                Audience = _appSettings.Authentication.ValidAudience,
            };

            var jwtHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtHandler.CreateJwtSecurityToken(tokenDescriptor);

            return jwtHandler.WriteToken(securityToken);
        }
    }
}
