using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFappMiddleware.Application.Services.Abstract
{
    public interface ISecurityService
    {
        string GenerateJwtToken();
    }
}
