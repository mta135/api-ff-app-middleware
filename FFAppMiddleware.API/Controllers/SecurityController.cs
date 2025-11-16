using FFappMiddleware.Application.Services.Abstract;
using FFappMiddleware.DataBase.Logger;
using FFAppMiddleware.API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [Route("api/Security")]
    [ApiController]
    [AllowAnonymous]
    public class SecurityController : ControllerBase
    {
        private readonly ICustomAuthenticationManager authManager;

        public SecurityController(ICustomAuthenticationManager authManager)
        {
            this.authManager = authManager;
        }

        //[HttpPost("StaticAuthenticationToken")]
  
        //public IActionResult GetStaticAuthenticationToken()
        //{
        //    string username = "admin";
        //    string password = "password123";

        //    string token = authManager.Authenticate(username, password);

        //    return Ok(new { token });
        //}
    }
}
