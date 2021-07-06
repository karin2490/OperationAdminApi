using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class Team
    {
        public Team()
        {
            Accounts = new HashSet<Account>();
            TeamByUsers = new HashSet<TeamByUser>();
            TeamLogs = new HashSet<TeamLog>();
        }

        public Team(string name)
        {
            TeamName = name;
        }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<TeamByUser> TeamByUsers { get; set; }
        public virtual ICollection<TeamLog> TeamLogs { get; set; }
    }
}
