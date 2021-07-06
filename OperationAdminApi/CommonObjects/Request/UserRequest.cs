using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class UserRequest
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public int RoleId { get; set; }

        public int AccountId { get; set; }

        public int TeamId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public bool Status { get; set; }

        public UserProfileRequest Profile { get; set; }
    }
}
