using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpAdminRepository.Common.Cache
{
    public class UserCache
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirtsName { get; set; }
        public string LastName { get; set; }

        public List<string> Roles { get; set; }
        public List<Cache_ActionsOnFunctionalities> FunctCache { get; set; }
    }
}
