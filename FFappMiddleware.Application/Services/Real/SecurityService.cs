using FFappMiddleware.Application.Services.Abstract;

namespace FFappMiddleware.Application.Services.Real
{
    public class SecurityService : ISecurityService
    {
        //private readonly IJwtAuthManager _jwtAuthManager;

        public SecurityService(/*IJwtAuthManager jwtAuthManager*/)
        {
            //_jwtAuthManager = jwtAuthManager;
        }

        public string GenerateJwtToken()
        {
            return "dummy:token";

            //if (_jwtAuthManager == null)
            //{
            //    throw new ArgumentNullException(nameof(_jwtAuthManager));
            //}

            //JwtAuthResult jwtAuthResult = _jwtAuthManager.GenerateTokens();
            //return jwtAuthResult.AccessToken;
        }
    }
}
