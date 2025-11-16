using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.UserManagement;
using System.Numerics;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;

namespace FFappMiddleware.Application.Services.Real
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userRepository;

        public UserManagementService(IUserManagementRepository userRepository)
        {
           _userRepository = userRepository;
        }

        public async Task<List<LoyaltyUser>> GetUserByPhone(string phone)
        {
            List<LoyaltyUser> users = await _userRepository.GetUserByPhone(phone);
            return users;
        }

        public async Task<List<LoyaltyUser>> GetUserByDiscountCardBarcode(string barcode)
        {
            List<LoyaltyUser> users = await _userRepository.GetUserByDiscountCardBarcode(barcode);
            return users;
        }

        public async Task<UserRegistrationResult> UserRegistration(LoyaltyUser userInfo)
        {
            UserRegistrationResult user = await _userRepository.UserRegistration(userInfo);
            return user;
        }
        public async Task<List<LoyaltyUser>> UserInfoByToken(Guid token)
        {
            List<LoyaltyUser> users = await _userRepository.UserInfoByToken(token);
            return users;
        }
        
        public async Task<List<Localites>> GetLocalites(string key)
        {
            List<Localites> users = await _userRepository.GetLocalites(key);
            return users;
        }
        public async Task<List<GenderTypes>> GetGenderTypes(string key)
        {
            List<GenderTypes> users = await _userRepository.GetGenderTypes(key);
            return users;
        }

    }
}
