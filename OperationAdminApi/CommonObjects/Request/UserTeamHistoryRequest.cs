using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class UserTeamHistoryRequest
    {
        public int TeamHistoryId { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
