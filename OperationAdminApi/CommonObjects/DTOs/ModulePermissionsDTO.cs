using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class ModulePermissionsDTO
    {
        public int ModuleId { get; set; }
        public List<int> PermissionsId { get; set; }
    }
}
