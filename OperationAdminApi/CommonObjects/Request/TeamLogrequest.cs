using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class TeamLogrequest
    {
        public int TeamHistoryId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public List<TeamHistoryByUserRequest> UsersList { get; set; }
    }
}
