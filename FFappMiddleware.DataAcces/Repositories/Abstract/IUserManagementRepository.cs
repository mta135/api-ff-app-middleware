using FFAppMiddleware.Model.Models.UserManagement;
using System.Threading.Tasks;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;

namespace FFappMiddleware.DataBase.Repositories.Abstract
{
    public interface IUserManagementRepository
    {
        Task<List<LoyaltyUser>> GetUserByPhone(PhoneNumberRequest phone);
        Task<List<LoyaltyUser>> GetUserByDiscountCardBarcode(DiscountCardNumberRequest phone);

        Task<UserRegistrationResult> UserRegistration(LoyaltyUserEdit userInfo);
        Task<List<UserTransaction>> UserTransactions(UserRequest token);
        Task<List<LoyaltyUser>> UpdateClientInformation(LoyaltyUserEdit clientinfo);
        Task<CardEdit> UpdateDiscountCardInformation(CardEdit card);
        Task<List<UserTransaction>> UserOrders(UserRequest token);
        Task<List<LoyaltyUser>> UserInfoByToken(UserRequest token);
        Task<LoyaltyUser> GenerateDiscountCard(UserRequest token);
        Task<List<Localites>> GetLocalites(string key);
        Task<List<GenderTypes>> GetGenderTypes(string key);
        

    }
}
