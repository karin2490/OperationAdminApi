using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.CommonObjects.DTOs
{
    public class TokensDTO
    {
        public int Tokenid { get; set; }
        public string Email { get; set; }
        public string TokenStr { get; set; }
        public bool Revoked { get; set; }
    }
}
