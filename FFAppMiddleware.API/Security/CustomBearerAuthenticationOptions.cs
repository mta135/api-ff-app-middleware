using Microsoft.AspNetCore.Authentication;

namespace FFAppMiddleware.API.Security
{
    public class CustomBearerAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Bearer";

        public string Scheme => DefaultScheme;

        public string AuthenticationType = DefaultScheme;
    }



    public interface ICustomAuthenticationManager
    {
        string Authenticate(string username, string password);

        bool ValidateToken(string token);

        AuthenticationParams GetAuthenticationParamsByToken(string token);
    }
}
