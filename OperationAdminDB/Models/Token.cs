using System;
using System.Collections.Generic;

#nullable disable

namespace OperationAdminDB.Models
{
    public partial class Token
    {
        public int TokenId { get; set; }
        public string Email { get; set; }
        public string TokenStr { get; set; }
        public bool Revoked { get; set; }
    }
}
