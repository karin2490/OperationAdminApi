using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class User
    {
        public User()
        {
            TeamByUsers = new HashSet<TeamByUser>();
            TeamLogs = new HashSet<TeamLog>();
            UserProfiles = new HashSet<UserProfile>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PassEncrypted { get; set; }
        public int RoleId { get; set; }
        public int? AccountId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public bool Status { get; set; }

        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<TeamByUser> TeamByUsers { get; set; }
        public virtual ICollection<TeamLog> TeamLogs { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
