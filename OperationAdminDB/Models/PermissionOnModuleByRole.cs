using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class PermissionOnModuleByRole
    {
        public PermissionOnModuleByRole(int moduleId,int roleId,int permId,bool status)
        {
            PermissionId = permId;
            ModuleId = moduleId;
            RoleId = roleId;
            Status = status;
        }
        public int PermissionOnModuleByRoleId { get; set; }
        public int PermissionId { get; set; }
        public int ModuleId { get; set; }
        public int RoleId { get; set; }
        public bool? Status { get; set; }

        public virtual Module Module { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
