using FFAppMiddleware.API.Security;
using Microsoft.AspNetCore.Authorization;

namespace FFAppMiddleware.API.Authorization
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params CustomUserRoleEnum[] roles)
        {
            Roles = string.Join(",", roles.Select(r => r.ToString()));
        }
    }
}
