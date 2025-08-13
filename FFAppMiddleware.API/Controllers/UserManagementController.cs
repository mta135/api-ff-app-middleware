using FFappMiddleware.Application.Services.Abstract;
using FFAppMiddleware.Model.Models.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisteredUsersApiModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RetrieveRegisteredUsers()
        {
            try
            {
                List<RegisteredUsersApiModel> users = await _userManagementService.RetrieveRegisteredUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
    }
}
