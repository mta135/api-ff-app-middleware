using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FFAppMiddleware.API.Core.Security.Authetication
{
    public class JwtAuthManager
    {
        private readonly JwtTokenConfig _config;

        public JwtAuthManager(IOptions<JwtTokenConfig> options)
        {
            _config = options.Value;
        }

        public string GenerateStaticJwtToken()
        {
            DateTime issuedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            byte[] key = Encoding.UTF8.GetBytes(_config.Secret);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            JwtHeader header = new JwtHeader(credentials);

            List<Claim> claims = new List<Claim>
            {
                new Claim("Company", "DitaEstFarm"),
                new Claim("HeadOfDepartment", "Denis"),
                new Claim("DotNet Developer", "Eugen"),
                new Claim("DotNet Developer", "Mihai")
            };

            JwtPayload payload = new JwtPayload(issuer: _config.Issuer, audience: _config.Audience,
                claims: claims, notBefore: issuedAt, expires: null, issuedAt: issuedAt);

            JwtSecurityToken token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
