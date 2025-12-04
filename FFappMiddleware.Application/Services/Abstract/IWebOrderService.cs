using FFAppMiddleware.Model.Models.WebOrderModel;

namespace FFappMiddleware.Application.Services.Abstract
{
    public interface IWebOrderService
    {
        Task<WebOrderSaveResult> CreateWebOrder(WebOrderModel shopifyWebOrderModel);
    }
}
