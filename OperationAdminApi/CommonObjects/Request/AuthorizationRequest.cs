using OperationAdminApi.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class AuthorizationRequest : IValidatableObject
    {
        public string Pin { get { return SecuredPin.DecodeAndDecrypt(); } }

        public string Email { get { return SecuredEmail.DecodeAndDecrypt(); } }

        public string SecuredPin { get; set; }

        public string SecuredEmail { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Pin.Equals("keyError") || Email.Equals("keyError"))
            {
                yield return new ValidationResult("There was a problem with the credentials and the data could not be decoded.",
                       new[] { "AuthRequest" });
            }
        }
    }
}
