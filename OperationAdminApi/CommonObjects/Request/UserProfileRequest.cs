using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class UserProfileRequest
    {
        public int UserId { get; set; }
        public string EnglishLevel { get; set; }
        public string TechnicalSkills { get; set; }
        public string LinkCv { get; set; }
    }
}
