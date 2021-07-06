using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class ModuleRequest
    {
        public int FunctionalityId { get; set; }
        public string FunctionalityDescrip { get; set; }
        public List<int> ActionIds { get; set; }
    }
}
