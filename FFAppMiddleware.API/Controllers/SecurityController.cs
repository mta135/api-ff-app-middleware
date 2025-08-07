using FFAppMiddleware.API.Core.Security.Authetication;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly JwtAuthenticationManager _jwtAuthManager;

        public SecurityController(JwtAuthenticationManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost]
        public IActionResult GetStaticJwtToken()
        {
            string token = _jwtAuthManager.GenerateStaticJwtToken();
            return Ok(new { token });
        }
    }
}
