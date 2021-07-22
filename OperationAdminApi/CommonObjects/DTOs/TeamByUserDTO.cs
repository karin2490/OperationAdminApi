using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class TeamByUserDTO
    {
        public int TeamByUserId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DateRegister { get; set; }
        public bool Status { get; set; }
    }


}
