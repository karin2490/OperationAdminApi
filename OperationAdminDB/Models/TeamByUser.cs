using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class TeamByUser
    {
        public TeamByUser(int teamId,int userId,DateTime? startDate,DateTime? enddate,DateTime now,bool status)
        {
            TeamId = teamId;
            UserId = userId;
            StartDate = startDate;
            EndDate = enddate;
            DateRegister = now;
            Status = status;
        }

        public int TeamByUserId { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DateRegister { get; set; }
        public bool Status { get; set; }

        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
