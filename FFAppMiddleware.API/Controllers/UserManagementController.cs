using FFAppMiddleware.Model.Models.UserManagement;
using FFAppMiddleware.Model.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UserManagementController(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserApiModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult RetrieveRegisteredUsers()
        {
            try
            {
                List<UserApiModel> users = _userManagementRepository.RetrieveRegisteredUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
    }
}
