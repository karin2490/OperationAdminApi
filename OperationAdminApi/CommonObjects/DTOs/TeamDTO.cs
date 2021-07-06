using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public bool Status { get; set; }
    }
}
