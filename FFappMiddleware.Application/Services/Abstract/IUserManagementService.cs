using FFAppMiddleware.Model.Models.UserManagement;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;

namespace FFappMiddleware.Application.Services.Real
{
    public interface IUserManagementService
    {
        Task<List<LoyaltyUser>> GetUserByPhone(string phone);
        Task<List<LoyaltyUser>> GetUserByDiscountCardBarcode(string barcode);

        Task<UserRegistrationResult> UserRegistration(LoyaltyUser userInfo);
        Task<List<LoyaltyUser>> UserInfoByToken(Guid token);
        Task<List<Localites>> GetLocalites(string key);
        Task<List<GenderTypes>> GetGenderTypes(string key);
    }
}
