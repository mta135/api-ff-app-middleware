using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FFAppMiddleware.API.Core.Security.Authetication
{
    public class JwtAuthManager
    {
        private readonly JwtTokenConfig _config;

        public JwtAuthManager(IOptions<JwtTokenConfig> options)
        {
            _config = options.Value;
        }

        public JwtAuthResult GenerateTokens()
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(_config.Secret);

            List<Claim> claims = new()
            {
                new Claim("Company", "DitaEstFarm"),
                new Claim("HeadOfDepartment", "Denis"),
                new Claim("DotNet Developer", "Eugen"),
                new Claim("DotNet Developer", "Mihai"),
            };

            SymmetricSecurityKey securityKey = new(key);
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken token = new(issuer: _config.Issuer, audience: _config.Audience,
                claims: claims, notBefore: DateTime.UtcNow, expires: null, signingCredentials: credentials);

            string tokenString = tokenHandler.WriteToken(token);

            return new JwtAuthResult
            {
                AccessToken = tokenString
            };
        }
    }
}
