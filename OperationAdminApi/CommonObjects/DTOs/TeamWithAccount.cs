using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class TeamWithAccount
    {
        public int TeamHistoryId { get; set; }
        public int TeamId { get; set; }
        public bool Status { get; set; }
        public List<AccountForTeamDTO> Accounts { get; set; }
    }
}
