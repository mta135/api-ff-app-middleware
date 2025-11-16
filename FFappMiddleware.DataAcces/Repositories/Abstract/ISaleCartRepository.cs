using FFAppMiddleware.Model.Models.SaleChart;
using static FFAppMiddleware.Model.Models.SaleChart.PreCheckModel;

namespace FFappMiddleware.DataBase.Repositories.Abstract
{
    public interface ISaleCartRepository
    {
       // Task<List<Cart>> PreCeckCart(List<CartItem> details, int clientId, long cardId);

        //Task<List<LoyaltyUser>> GetUserByPhone(string phone);
        //Task<List<LoyaltyUser>> GetUserByDiscountCardBarcode(string phone);

        Task<List<Cart>> PreCheckCart(PreCheckApiModel preCheck);

        Task<List<Cart>> ConfirmPreCheckCart(Cart preCheck);

    }
}
