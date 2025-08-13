using FFAppMiddleware.Model.Models.UserManagement;

namespace FFappMiddleware.DataBase.Repositories.Abstract
{
    public interface IUserManagementRepository
    {
        Task<List<RegisteredUsersApiModel>> RetrieveRegisteredUsers();

    }
}
