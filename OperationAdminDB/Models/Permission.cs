using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class Permission
    {
        public Permission()
        {
            PermissionOnModuleByRoles = new HashSet<PermissionOnModuleByRole>();
        }

        public int PermissionId { get; set; }
        public string PermissionDescrip { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<PermissionOnModuleByRole> PermissionOnModuleByRoles { get; set; }
    }
}
