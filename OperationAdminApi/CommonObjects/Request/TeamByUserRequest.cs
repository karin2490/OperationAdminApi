using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class TeamByUserRequest
    {
        public int TeamByUserId { get; set; }
        public int TeamId { get; set; }
       
        public int UserId { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DateRegister { get; set; }
        public bool Status { get; set; }
    }
}
