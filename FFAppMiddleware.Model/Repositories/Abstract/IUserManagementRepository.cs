using FFAppMiddleware.Model.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Repositories.Abstract
{
    public interface IUserManagementRepository
    {

        Task<List<RegisteredUsersApiModel>> RetrieveRegisteredUsers();
    }
}
