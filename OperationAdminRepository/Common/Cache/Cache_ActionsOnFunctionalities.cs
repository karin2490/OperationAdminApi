using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpAdminRepository.Common.Cache
{
    public class Cache_ActionsOnFunctionalities
    {
        public int FunctionalityId { get; set; }
        public List<int> ActionsId { get; set; }

        public Cache_ActionsOnFunctionalities(int FunctionalityId, List<int> ActionsId)
        {
            this.FunctionalityId = FunctionalityId;
            this.ActionsId = ActionsId;
        }
    }
}
