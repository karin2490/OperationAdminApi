using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class Role
    {
        public Role(int roleId,string role,bool status)
        {
            RoleId = roleId;
            RoleDescrip = role;
            Status = status;
        }
        public Role()
        {
            ModuleByRoles = new HashSet<ModuleByRole>();
            PermissionOnModuleByRoles = new HashSet<PermissionOnModuleByRole>();
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleDescrip { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<ModuleByRole> ModuleByRoles { get; set; }
        public virtual ICollection<PermissionOnModuleByRole> PermissionOnModuleByRoles { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
