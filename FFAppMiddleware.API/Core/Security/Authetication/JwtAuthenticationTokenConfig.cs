namespace FFAppMiddleware.API.Core.Security.Authetication
{
    public class JwtAuthenticationTokenConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; } 
        public string Audience { get; set; }
    }
}
