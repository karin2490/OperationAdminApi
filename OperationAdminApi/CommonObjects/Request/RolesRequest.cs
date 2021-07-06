using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class RoleRequest
    {
        public int RoleId { get; set; }
        public string RoleDescrip { get; set; }
        public bool? Status { get; set; }
    }
   
    
    public class RoleByUserRequest
    {
        public int UserId { get; set; }
    }

    public class PermissionByModuleRequest
    {
        public int ModuleId { get; set; }
        public List<int> PermissionIds { get; set; }

    }

    public class RoleInfoRequest
    {
        public int RoleId { get; set; }
        public string RoleDescrip { get; set; }
        public List<PermissionByModuleRequest> PermissionsModules { get; set; }
    }
}
