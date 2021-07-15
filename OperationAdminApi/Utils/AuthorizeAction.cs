using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using ENUM = OperationAdminDB.Enum;

namespace OperationAdminApi.Utils
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeAction : AuthorizeAttribute, IAuthorizationFilter
    {
        private ENUM.Module Module;
        private ENUM.Permission Permission;

        public AuthorizeAction(ENUM.Module Module, ENUM.Permission Permission)
        {
            this.Module = Module;
            this.Permission = Permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
