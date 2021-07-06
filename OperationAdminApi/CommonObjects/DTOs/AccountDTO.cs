using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string ClientName { get; set; }
        public string OperationResp { get; set; }
        public int TeamId { get; set; }
        public bool Status { get; set; }
    }
}
