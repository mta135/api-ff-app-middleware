using FFAppMiddleware.API.Core.Security.Authetication;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;

        public SecurityController(JwtAuthenticationManager jwtAuthenticationManager)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost]
        public IActionResult GetStaticJwtAuthenticationToken()
        {
            string token = _jwtAuthenticationManager.GenerateStaticJwtAuthenticationToken();
            return Ok(new { token });
        }
    }
}
