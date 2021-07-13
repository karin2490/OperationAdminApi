using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.Services.Interfaces
{
    public interface IRolesService
    {
        Task<Response> InsertRoleNameAsync(HttpContext context, RoleRequest roleRequest);
        Task<Response> UpdateRoleAsync(HttpContext context, RoleRequest roleRequest);
        Task<Response> DeleteRoleAsync(HttpContext context, int roleId);
        Task<Response> ActiveRoleAsync(HttpContext context, int roleId);
        Task<Response> InactiveRoleAsync(HttpContext context, int roleId);

        Task<Response> SetModuleByRole(HttpContext context, ModuleByRoleRequest request);
        Task<Response> SetModulePermissions(HttpContext context, PermissionOnModuleRequest permOnModuleReq);
        Task<Response> GetDropDownRolesAsync(HttpContext context);
        Task<Response> GetModuleByRoleId(HttpContext context, int RoleId);
        
    }
}
