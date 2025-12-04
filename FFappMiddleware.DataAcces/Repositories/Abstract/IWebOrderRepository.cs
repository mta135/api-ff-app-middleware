using FFAppMiddleware.Model.Models.WebOrderModel;

namespace FFappMiddleware.DataBase.Repositories.Abstract
{
    public interface IWebOrderRepository
    {
        Task<WebOrderSaveResult> CreateWebOrderAsync(WebOrderModel order);

        Task<WebOrderSaveResult> CreateWebOrderAsyncV2(WebOrderModel orderModel);
    }
}
