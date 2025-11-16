using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FFAppMiddleware.API.Security
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomBearerAuthenticationOptions>
    {
        private readonly ICustomAuthenticationManager customAuthenticationManager;

        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomBearerAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ICustomAuthenticationManager customAuthenticationManager)
            : base(options, logger, encoder)
        {
            this.customAuthenticationManager = customAuthenticationManager;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Endpoint endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
            }

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization scheme"));
            }

            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing token"));
            }

            try
            {
                AuthenticateResult result = ValidateToken(token);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Token validation failed");

                return Task.FromResult(AuthenticateResult.Fail("Token validation failed"));
            }
        }

        private AuthenticateResult ValidateToken(string token)
        {
            if (!customAuthenticationManager.ValidateToken(token))
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            AuthenticationParams _authenticationParams = customAuthenticationManager.GetAuthenticationParamsByToken(token);
            if (_authenticationParams == null)
            {
                return AuthenticateResult.Fail("Token validation failed");
            }

            List<Claim> claims = new()
            {
                new Claim("Id", _authenticationParams.Id),
                new Claim("UserName", _authenticationParams.Username),
                new Claim("Role", _authenticationParams.Role),
                new Claim("Description", _authenticationParams.Description),

                new Claim(ClaimTypes.AuthenticationMethod, Options.AuthenticationType),
                new Claim("token", token)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            AuthenticationTicket ticket = new AuthenticationTicket(principal, Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            AuthenticateResult authenticateResult = await Context.AuthenticateAsync(Scheme.Name);

            string detailMessage = authenticateResult?.Failure?.Message ?? "Authentication failed. A valid token is required.";

            Response.StatusCode = StatusCodes.Status401Unauthorized;
            Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = detailMessage, 
                Instance = Context.Request.Path
            };

            await Response.WriteAsJsonAsync(problemDetails);

        }
    }
}
