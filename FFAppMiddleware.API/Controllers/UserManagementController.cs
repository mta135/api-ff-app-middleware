using FFappMiddleware.Application.Services.Real;
using FFAppMiddleware.Model.Models.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;


namespace FFAppMiddleware.API.Controllers
{
    [Authorize]
    // [Route("api/[controller]/[action]")]
    [Route("api/User")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(UserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet("UserByPhone")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoyaltyUser))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserByPhone(string phone)
        {
            try
            {
                List<LoyaltyUser> users = await _userManagementService.GetUserByPhone(phone);
                return Ok(users);
    
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpGet("UserByDiscountCardBarcode")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoyaltyUser))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserByDiscountCardBarcode(string barcode)
        {
            try
            {
                List<LoyaltyUser> users = await _userManagementService.GetUserByDiscountCardBarcode(barcode);
                return Ok(users);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpPost("UserRegistration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRegistrationResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UserRegistration(LoyaltyUser userInfo)
        {
            try
            {
                UserRegistrationResult users = await _userManagementService.UserRegistration(userInfo);
                return Ok(users);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpGet("UserInfoByToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoyaltyUser))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UserInfoByToken(Guid token)
        {
            try
            {
                List<LoyaltyUser> users = await _userManagementService.UserInfoByToken(token);
                return Ok(users);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }


        [HttpGet("GetLocalites")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Localites))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLocalites(string key)
        {
            try
            {
                List<Localites> users = await _userManagementService.GetLocalites(key);
                return Ok(users);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpGet("GetGenderTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenderTypes))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetGenderTypes(string key)
        {
            try
            {
                List<GenderTypes> users = await _userManagementService.GetGenderTypes(key);
                return Ok(users);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }






    }
}
