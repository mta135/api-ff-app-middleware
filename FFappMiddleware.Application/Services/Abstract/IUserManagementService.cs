using FFAppMiddleware.Model.Models.UserManagement;

namespace FFappMiddleware.ApplicationServices.Services.Real
{
    public interface IUserManagementService
    {
        Task<List<RegisteredUsersApiModel>> RetrieveRegisteredUsers();
    }
}
