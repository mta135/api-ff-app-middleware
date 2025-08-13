using FFAppMiddleware.Model.Models.UserManagement;

namespace FFappMiddleware.Application.Services.Real
{
    public interface IUserManagementService
    {
        Task<List<RegisteredUsersApiModel>> RetrieveRegisteredUsers();
    }
}
