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

        public async Task<List<LoyaltyUser>> GetUserByPhone(PhoneNumberRequest phone)
        {
            List<LoyaltyUser> users = await _userRepository.GetUserByPhone(phone);
            return users;
        }

        public async Task<List<LoyaltyUser>> GetUserByDiscountCardBarcode(DiscountCardNumberRequest barcode)
        {
            List<LoyaltyUser> users = await _userRepository.GetUserByDiscountCardBarcode(barcode);
            return users;
        }

        public async Task<UserRegistrationResult> UserRegistration(LoyaltyUserEdit userInfo)
        {
            UserRegistrationResult user = await _userRepository.UserRegistration(userInfo);
            return user;
        }

        public async Task<List<UserTransaction>> UserTransactions(UserRequest token)
        {
            List<UserTransaction> usertr = await _userRepository.UserTransactions(token);
            return usertr;
        }
        public async Task<List<LoyaltyUser>> UpdateClientInformation(LoyaltyUserEdit info)
        {
            List<LoyaltyUser> usertr = await _userRepository.UpdateClientInformation(info);
            return usertr;
        }

        public async Task<CardEdit> UpdateDiscountCardInformation(CardEdit card)
        {
            CardEdit usertr = await _userRepository.UpdateDiscountCardInformation(card);
            return usertr;
        }

        public async Task<List<UserTransaction>> UserOrders(UserRequest token)
        {
            List<UserTransaction> usertr = await _userRepository.UserOrders(token);
            return usertr;
        }

        public async Task<List<LoyaltyUser>> UserInfoByToken(UserRequest token)
        {
            List<LoyaltyUser> users = await _userRepository.UserInfoByToken(token);
            return users;
        }
        public async Task<LoyaltyUser> GenerateDiscountCard(UserRequest userid)
        {
            LoyaltyUser users = await _userRepository.GenerateDiscountCard(userid);
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
