using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Enum
{
    public enum UserValidation
    {
        Success=0,
        InvalidEmail=1,
        DuplicateOnEmail=2,
        InvalidPass=3
    }
}
