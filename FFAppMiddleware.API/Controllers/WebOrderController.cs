using FFappMiddleware.Application.Services.Abstract;
using FFAppMiddleware.API.Authorization;
using FFAppMiddleware.API.Security;
using FFAppMiddleware.Model.Models.WebOrderModel;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [AuthorizeRoles(CustomUserRoleEnum.ShopifyUser)]
    [Route("api/Order")]
    [ApiController]
    public class WebOrderController: ControllerBase
    {
        private readonly IWebOrderService _webOrderService;

        public WebOrderController(IWebOrderService webOrderService)
        {
            _webOrderService = webOrderService;
        }


        [HttpPost("CreateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebOrderSaveResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateOrder([FromBody] WebOrderModel shopifyWebhookPayload)
        {
            try
            {
                WebOrderSaveResult result = await _webOrderService.CreateWebOrder(shopifyWebhookPayload);
                return Ok(result);
            }
             catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
