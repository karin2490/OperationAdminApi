using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class PermissionOnModuleRequest
    {
        public int ModuleId { get; set; }
        public int PermissionId { get; set; }
       public int RoleId { get; set; }

        public bool Status { get; set; }
    }
}
