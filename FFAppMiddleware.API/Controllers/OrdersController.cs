using FFAppMiddleware.Model.Models.WebOrderModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFAppMiddleware.API.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] ShopifyWebhookPayload shopifyWebhookPayload)
        {
            return Ok();
        }

      
    }
}
