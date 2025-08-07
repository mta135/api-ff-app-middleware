using FFAppMiddleware.API.Core.Security.Authetication;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly JwtAuthManager _jwtAuthManager;

        public SecurityController(JwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost]
        public IActionResult GenerateToken()
        {
            var token = _jwtAuthManager.GenerateTokens();

            return Ok(token);
        }
    }
}
