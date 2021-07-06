using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class ModuleByRoleRequest
    {
        public int ModuleByRoleId { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public bool? Status { get; set; }
    }
}
