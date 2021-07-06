using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class UserProfileDTO
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string EnglishLevel { get; set; }
        public string TechnicalSkills { get; set; }
        public string LinkCv { get; set; }
        public bool Status { get; set; }
    }
}
