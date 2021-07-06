using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class PermissionDTO
    {
        public int PermissionId { get; set; }
        public string PermissionDescrip { get; set; }
        public bool? Status { get; set; }
    }
}
