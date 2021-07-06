using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class TeamLog
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
        public DateTime? DateActivity { get; set; }

        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
