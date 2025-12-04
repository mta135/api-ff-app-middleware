using FFAppMiddleware.Model.Models.UserManagement;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;

namespace FFappMiddleware.Application.Services.Real
{
    public interface IUserManagementService
    {
        Task<List<LoyaltyUser>> GetUserByPhone(PhoneNumberRequest phone);
        Task<List<LoyaltyUser>> GetUserByDiscountCardBarcode(DiscountCardNumberRequest barcode);

        Task<UserRegistrationResult> UserRegistration(LoyaltyUserEdit userInfo);
        Task<List<LoyaltyUser>> UpdateClientInformation(LoyaltyUserEdit userInfo);
        Task<CardEdit> UpdateDiscountCardInformation(CardEdit cardinfo);
        Task<List<UserTransaction>> UserTransactions(UserRequest userid);
        Task<List<UserTransaction>> UserOrders(UserRequest userid);
        Task<List<LoyaltyUser>> UserInfoByToken(UserRequest userid);

        Task<LoyaltyUser> GenerateDiscountCard(UserRequest userid);
        Task<List<Localites>> GetLocalites(string key);
        Task<List<GenderTypes>> GetGenderTypes(string key);
    }
}
