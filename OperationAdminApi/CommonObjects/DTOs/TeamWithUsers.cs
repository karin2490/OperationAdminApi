using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class TeamWithUsers
    {
        public int TeamHistoryId { get; set; }
        public int TeamId { get; set; }
        public bool Status { get; set; }
        public List<TeamLogUserDTO> UserList { get; set; }
       
    }
}
