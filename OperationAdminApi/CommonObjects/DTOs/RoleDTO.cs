using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string RoleDescrip { get; set; }
        public bool? Status { get; set; }
    }
}
