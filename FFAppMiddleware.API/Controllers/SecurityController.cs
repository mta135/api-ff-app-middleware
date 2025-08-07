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
        public IActionResult GetStaticToken()
        {
            string token = _jwtAuthManager.GenerateStaticJwtToken();
            return Ok(new { token });
        }
    }
}
