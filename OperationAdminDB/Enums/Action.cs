using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminDB.Enum
{
    public enum Action:int
    {
        CREATE = 1,
        READ = 2,
        UPDATE = 3,
        DELETE = 4,
        VIEW_REPORT = 5,
        DISABLE = 6,
        LOGIN = 7
    }
}
