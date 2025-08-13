using FFAppMiddleware.Model.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFappMiddleware.Application.Services.Abstract
{
    public interface IUserManagementService
    {
        Task<List<RegisteredUsersApiModel>> RetrieveRegisteredUsers();
    }
}
