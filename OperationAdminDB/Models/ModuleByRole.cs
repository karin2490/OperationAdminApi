using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public class ModuleByRole
    {
        public ModuleByRole()
        {

        }

        public ModuleByRole(int rolId, int modId, bool status)
        {
            RoleId = rolId;
            ModuleId = modId;
            Status = status;
        }

        public int ModuleByRoleId { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public bool? Status { get; set; }

        public virtual Module Module { get; set; }
        public virtual Role Role { get; set; }
    }

    
      

}
