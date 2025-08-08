using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.UserManagement
{
    public class UserApiModel
    {
        public long Id { get; set; }

        public string UserLogin { get; set; }

        public string UserPassword { get; set; }

        public string Idnp { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
