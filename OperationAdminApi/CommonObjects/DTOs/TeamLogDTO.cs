using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class TeamLogDTO
    {
        public int TeamLogId { get; set; }
        public string Action { get; set; }
        public string Module { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string DataLog { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DateActivity { get; set; }

    }
}
