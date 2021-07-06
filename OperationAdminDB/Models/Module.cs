using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class Module
    {
        public Module()
        {
            ModuleByRoles = new HashSet<ModuleByRole>();
            PermissionOnModuleByRoles = new HashSet<PermissionOnModuleByRole>();
        }

        public int ModuleId { get; set; }
        public string ModuleDescrip { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<ModuleByRole> ModuleByRoles { get; set; }
        public virtual ICollection<PermissionOnModuleByRole> PermissionOnModuleByRoles { get; set; }
    }
}
