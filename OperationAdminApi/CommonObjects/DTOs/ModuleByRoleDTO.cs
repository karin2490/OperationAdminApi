using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class ModuleByRoleDTO
    {
        public int ModuleByRoleId { get; set; }

        public int ModuleId { get; set; }
        public string ModuleDescrip { get; set; }
        public int RoleId { get; set; }
        public string RoleDescrip { get; set; }
       
        public bool? Status { get; set; }
    }
}
