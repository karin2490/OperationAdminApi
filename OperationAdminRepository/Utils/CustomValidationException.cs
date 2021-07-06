using System;
using System.Collections.Generic;
using System.Text;

namespace OperationAdminRepository.Utils
{
    public class CustomValidationException:Exception
    {
        public CustomValidationException() : base()
        { }

        public CustomValidationException(string message) : base(message)
        { }

        public CustomValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
