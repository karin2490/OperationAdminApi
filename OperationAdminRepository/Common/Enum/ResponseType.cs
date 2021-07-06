using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminRepository.Common.Enum
{
    public enum ResponseType:int
    {
        INFO = 1000,
        WARNING = 1001,
        SUCCESS = 200,
        NO_AUTORIZE = 403,
        NO_FOUND = 404,
        NOT_ACCEPTABLE = 406,
        INVALID_PASSWORD = 407,
        INTERNAL_ERROR = 500,
        UNAUTHORIZED = 401,
        BAD_REQUEST = 400
    }
}
