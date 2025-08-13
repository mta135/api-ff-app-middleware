using FFAppMiddleware.Model.Models.UserManagement;

namespace FFAppMiddleware.Model.Repositories.Abstract
{
    public interface IUserManagementRepository
    {
        Task<List<RegisteredUsersApiModel>> RetrieveRegisteredUsers();

    }
}
