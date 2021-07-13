using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class AuthDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}
