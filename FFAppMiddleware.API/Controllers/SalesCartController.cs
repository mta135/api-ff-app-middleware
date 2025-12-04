using FFappMiddleware.Application.Services.Real;
using FFappMiddleware.DataBase.Logger;
using FFAppMiddleware.API.Authorization;
using FFAppMiddleware.API.Security;
using FFAppMiddleware.Model.Models.SaleChart;
using Microsoft.AspNetCore.Mvc;
using static FFAppMiddleware.Model.Models.SaleChart.PreCheckModel;

namespace FFAppMiddleware.API.Controllers
{
    [AuthorizeRoles(CustomUserRoleEnum.RoleAleator)]
    [Route("api/SalesCart")]
    [ApiController]
    public class SalesCartController : ControllerBase
    {
        private readonly ISaleCartService _SaleCartService;

        public SalesCartController(ISaleCartService saleChartService)
        {
            _SaleCartService = saleChartService;
        }       

        [HttpPost("PreCheck")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cart))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PreCheck(PreCheckApiModel preCheckApiModel) 
        {
            try
            {
                List<Cart> carts = await _SaleCartService.PreCheck(preCheckApiModel);
                return Ok(carts);
            }
            catch (Exception ex)
            {
                WriteLog.Web.Error("Error retrieving data from the database", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }


        [HttpPost("ConfirmPreCheck")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Cart>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ConfirmPreCheck([FromBody] Cart preCheckApiModel)
        {
            if (preCheckApiModel == null)
                return BadRequest("Request body is required.");

            try
            {
                List<Cart> carts = await _SaleCartService.ConfirmPreCheck(preCheckApiModel);
                return Ok(carts);
            }
            catch (Exception ex)
            {
                WriteLog.Web.Error("Error retrieving data from the database", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {ex.Message}");
            }
        }


    }
}
