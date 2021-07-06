using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.Request
{
    public class AuthorizationRequest
    {
        [Required]
        public string Pass { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
