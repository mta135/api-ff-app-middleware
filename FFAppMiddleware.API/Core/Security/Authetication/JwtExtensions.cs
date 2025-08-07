using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FFAppMiddleware.API.Core.Security.Authetication
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            JwtTokenConfig jwtConfig = config.GetSection("JwtTokenConfig").Get<JwtTokenConfig>() ?? throw new Exception("JwtTokenConfig section missing or invalid");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,

                    ValidateLifetime = true, // dacă tokenul nu expiră, setează pe false
                    ClockSkew = TimeSpan.Zero,
                };
            });

            return services;
        }
    }
}
