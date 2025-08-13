using FFappMiddleware.Application.Services.Abstract;
using FFAppMiddleware.Model.Models.UserManagement;
using FFAppMiddleware.Model.Repositories.Abstract;

namespace FFappMiddleware.Application.Services.Real
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
