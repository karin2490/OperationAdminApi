using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class Account
    {
        public Account()
        {
            Users = new HashSet<User>();
        }

        public Account(string aName, string cliName, string opresp, int teamId)
        {
            AccountName = aName;
            ClientName = cliName;
            OperationResp = opresp;
            TeamId = teamId;

        }

        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string ClientName { get; set; }
        public string OperationResp { get; set; }
        public int TeamId { get; set; }
        public bool Status { get; set; }

        public virtual Team Team { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
