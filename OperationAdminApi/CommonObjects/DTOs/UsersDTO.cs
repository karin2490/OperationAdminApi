using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class UsersDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
       
        public int AccountId { get; set; }

        public int TeamId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public bool Status { get; set; }

    }
}
