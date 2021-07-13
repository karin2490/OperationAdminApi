using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class UserToken
    {
        public string Token { get; set; }
        public string rTokenStr { get; set; }

        public bool Revoked { get; set; }

        public DateTime Expiration { get; set; }
    }
}
