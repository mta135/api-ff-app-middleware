using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.UserManagement;

namespace FFappMiddleware.ApplicationServices.Services.Real
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userRepository;

        public UserManagementService(IUserManagementRepository userRepository)
        {
           _userRepository = userRepository;
        }

        public async Task<List<RegisteredUsersApiModel>> RetrieveRegisteredUsers()
        {
            List<RegisteredUsersApiModel> users = await _userRepository.RetrieveRegisteredUsers();
            return users;
        }

    }
}
