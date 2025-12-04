using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [Route("api/Check")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        [HttpPost("Index")]
        public IActionResult Index()
        {
            return Ok("Este doar o functie de verificare...");
        }
    }
}
