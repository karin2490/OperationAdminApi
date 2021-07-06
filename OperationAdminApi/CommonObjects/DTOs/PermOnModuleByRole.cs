﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class PermOnModuleByRole
    {
        public int PermissionOnModuleByRole { get; set; }
        public int PermissionId { get; set; }
        public string PermissionDescrip { get; set; }
        public int ModuleId { get; set; }
        public string ModuleDescrip { get; set; }
        public int RoleId { get; set; }
        public string RoleDescript { get; set; }
       public bool Status { get; set; }
       

        
    }
}
