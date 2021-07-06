using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
   public  class AccountRequest
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string ClientName { get; set; }

        public string OperationResp { get; set; }
        public int TeamId { get; set; }

        public byte Status { get; set; }
    }
}
