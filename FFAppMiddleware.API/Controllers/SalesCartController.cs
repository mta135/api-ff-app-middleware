using FFappMiddleware.Application.Services.Real;
using FFappMiddleware.DataBase.Logger;
using FFAppMiddleware.Model.Models.SaleChart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FFAppMiddleware.Model.Models.SaleChart.PreCheckModel;

namespace FFAppMiddleware.API.Controllers
{
    [Authorize]
    //[Route("api/[controller]/[action]")]
    [Route("api/SalesCart")]
    [ApiController]
    public class SalesCartController : ControllerBase
    {
        private readonly ISaleCartService _SaleCartService;

        public SalesCartController(ISaleCartService saleChartService)
        {
            _SaleCartService = saleChartService;
        }

        //[HttpPost("PreCeck")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cart))]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //public async Task<IActionResult> GetPreCeck([FromBody] List<CartItem> details, int clientId, long cardId)
        //{
        //    try
        //    {
        //        List<Cart> users = await _SaleCartService.PreCeckCart(details, clientId, cardId);
        //        return Ok(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.Web.Error("Error retrieving data from the database", ex);
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
        //    }
        //}


        [HttpPost("PreCheck")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cart))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PreCheck(PreCheckApiModel preCheckApiModel) // Mihai
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cart))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ConfirmPreCheck(Cart preCheckApiModel) 
        {
            try
            {
                List<Cart> carts = await _SaleCartService.ConfirmPreCheck(preCheckApiModel);
                return Ok(carts);
            }
            catch (Exception ex)
            {
                WriteLog.Web.Error("Error retrieving data from the database", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

    }
}
