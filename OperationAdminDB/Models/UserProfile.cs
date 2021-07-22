using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class UserProfile
    {
        public UserProfile()
        {

        }

        public UserProfile(int userid,string englishLevel,string techSkills,string link,bool status)
        {
            UserId = userid;
            EnglishLevel = englishLevel;
            TechnicalSkills = techSkills;
            LinkCv = link;
            Status = status;
        }
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string EnglishLevel { get; set; }
        public string TechnicalSkills { get; set; }
        public string LinkCv { get; set; }
        public bool Status { get; set; }

        public virtual User User { get; set; }
    }
}
