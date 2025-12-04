using FFappMiddleware.Application.Services.Abstract;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.WebOrderModel;
using System.Threading.Tasks;

namespace FFappMiddleware.Application.Services.Real
{
    public class WebOrderService : IWebOrderService
    {
        private readonly IWebOrderRepository _webOrderRepository;

        public WebOrderService(IWebOrderRepository webOrderRepository)
        {
            _webOrderRepository = webOrderRepository;
        }

        public async Task<WebOrderSaveResult> CreateWebOrder(WebOrderModel shopifyWebOrderModel)
        {
            WebOrderSaveResult result = await  _webOrderRepository.CreateWebOrderAsync(shopifyWebOrderModel);

            if(result.Code != 0)
                throw new Exception($"Error creating web order: {result.Message}");

            return result;
        }
    }
}
