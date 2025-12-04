using FFappMiddleware.Application.Services.Real;
using FFAppMiddleware.API.Authorization;
using FFAppMiddleware.API.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;


namespace FFAppMiddleware.API.Controllers
{
    [AuthorizeRoles(CustomUserRoleEnum.RoleAleator)]
    [Route("api/User")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(UserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpPost("UserByPhone")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoyaltyUser>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByPhone([FromBody] PhoneNumberRequest phone)
        {
            if (phone == null || string.IsNullOrWhiteSpace(phone.PhoneNumber))
                return BadRequest("Invalid phone number.");

            try
            {
                var users = await _userManagementService.GetUserByPhone(phone);

                if (users == null || users.Count == 0)
                    return NotFound("No users found.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }


        [HttpPost("UserByDiscountCardBarcode")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoyaltyUser>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByDiscountCardBarcode([FromBody] DiscountCardNumberRequest barcode)
        {
            if (barcode == null || string.IsNullOrWhiteSpace(barcode.DiscountCardNumber))
                return BadRequest("Invalid barcode.");

            try
            {
                var users = await _userManagementService.GetUserByDiscountCardBarcode(barcode);

                if (users == null || users.Count == 0)
                    return NotFound("No users found.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }


        [HttpPost("UserRegistration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRegistrationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserRegistration([FromBody] LoyaltyUserEdit userInfo)
        {
            if (userInfo == null)
                return BadRequest("Invalid user data.");

            try
            {
                var result = await _userManagementService.UserRegistration(userInfo);

                if (result == null)
                    return NotFound("User registration failed.");

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
               
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpGet("UserInfoById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoyaltyUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserInfoById([FromQuery] UserRequest userid)
        {
            if (userid == null || userid.UserId == Guid.Empty)
                return BadRequest("Invalid user ID.");

            try
            {
                var user = await _userManagementService.UserInfoByToken(userid);

                if (user == null)
                    return NotFound("User not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpPost("GenerateDiscountCard")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoyaltyUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateDiscountCard([FromBody] UserRequest userid)
        {
            if (userid == null || userid.UserId == Guid.Empty)
                return BadRequest("Invalid user ID.");

            try
            {
                var result = await _userManagementService.GenerateDiscountCard(userid);

                if (result == null)
                    return NotFound("User not found or discount card could not be generated.");

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
               
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data: {ex.Message}");
            }
        }



        [HttpGet("GetLocalites")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Localites>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLocalites([FromQuery] string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return BadRequest("Parameter 'key' is required.");

                List<Localites> users = await _userManagementService.GetLocalites(key);

                if (users == null || users.Count == 0)
                    return NotFound("No localities found.");

                return Ok(users);
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627) // duplicate / conflict example
            {
                return Conflict($"Database conflict: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }



        [HttpGet("GetGenderTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GenderTypes>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGenderTypes([FromQuery] string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return BadRequest("Parameter 'key' is required.");

                List<GenderTypes> users = await _userManagementService.GetGenderTypes(key);

                if (users == null || users.Count == 0)
                    return NotFound("No gender types found.");

                return Ok(users);
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627) // conflict SQL: duplicate key
            {
                return Conflict($"Database conflict occurred: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }



        [HttpPost("UserTransactions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserTransaction>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserTransactions([FromBody] UserRequest userId)
        {
            if (userId == null || userId.UserId == Guid.Empty)
                return BadRequest("Invalid user ID.");

            try
            {
                var transactions = await _userManagementService.UserTransactions(userId);

                if (transactions == null || transactions.Count == 0)
                    return NotFound("No transactions found for this user.");

                return Ok(transactions);
            }
            catch (InvalidOperationException ex)
            {
                
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }


        [HttpPost("UserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserTransaction>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserOrders([FromBody] UserRequest userId)
        {
            if (userId == null || userId.UserId == Guid.Empty)
                return BadRequest("Invalid user ID.");

            try
            {
                var orders = await _userManagementService.UserOrders(userId);

                if (orders == null || orders.Count == 0)
                    return NotFound("No orders found for this user.");

                return Ok(orders);
            }
            catch (InvalidOperationException ex)
            {
                
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }


        [HttpPut("UpdateClientInformation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoyaltyUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateClientInformation([FromBody] LoyaltyUserEdit clientDto)
        {
            if (clientDto == null || clientDto.Id == Guid.Empty)
                return BadRequest("Invalid client data.");

            try
            {
                var updatedClient = await _userManagementService.UpdateClientInformation(clientDto);

                if (updatedClient == null)
                    return NotFound("Client not found.");

                return Ok(updatedClient);
            }
            catch (InvalidOperationException ex)
            {
                
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating client information: {ex.Message}");
            }
        }


        [HttpPut("UpdateDiscountCardInformation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CardEdit))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDiscountCardInformation([FromBody] CardEdit card)
        {
            if (card == null || card.Id >0)
                return BadRequest("Invalid card data.");

            try
            {
                var updatedCard = await _userManagementService.UpdateDiscountCardInformation(card);

                if (updatedCard == null)
                    return NotFound("Card not found.");

                return Ok(updatedCard);
            }
            catch (InvalidOperationException ex)
            {
           
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating card information: {ex.Message}");
            }
        }


    }
}
