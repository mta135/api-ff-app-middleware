using FFAppMiddleware.Model.Models.SaleChart;
using FFAppMiddleware.Model.Models.UserManagement;
using static FFAppMiddleware.Model.Models.SaleChart.PreCheckModel;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;

namespace FFappMiddleware.Application.Services.Real
{
    public interface ISaleCartService
    {
        Task<List<Cart>> PreCheck(PreCheckApiModel preCheck);
        Task<List<Cart>> ConfirmPreCheck(Cart preCheck);

    }
}
