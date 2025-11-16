using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.SaleChart;
using System.Threading.Tasks;
using static FFAppMiddleware.Model.Models.SaleChart.PreCheckModel;

namespace FFappMiddleware.Application.Services.Real
{
    public partial class SaleCartService : ISaleCartService
    {
        private readonly ISaleCartRepository _SaleCartRepository;

        public SaleCartService(ISaleCartRepository saleCartRepositorypository)
        {
            _SaleCartRepository = saleCartRepositorypository;
        }

        

        public async Task<List<Cart>> PreCheck(PreCheckApiModel preCheck)
        {
            List<Cart> carts = await _SaleCartRepository.PreCheckCart(preCheck);

            return carts;
        }

        public async Task<List<Cart>> ConfirmPreCheck(Cart preCheck)
        {
            List<Cart> carts = await _SaleCartRepository.ConfirmPreCheckCart(preCheck);

            return carts;
        }

    }
}
