using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpAdminRepository.Common.Cache
{
    public class Cache_PermissionsOnModule
    {
        public int ModuleId { get; set; }
        public List<int> PermissionsId { get; set; }

        public Cache_PermissionsOnModule(int ModuleId, List<int> PermissionsId)
        {
            this.ModuleId = ModuleId;
            this.PermissionsId = PermissionsId;
        }
    }
}
