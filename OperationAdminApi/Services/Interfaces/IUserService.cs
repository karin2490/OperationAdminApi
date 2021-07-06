using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response> InsertUserAsync(HttpContext context, UserRequest userRequest);
        Task<Response> UpdateUserAsync(HttpContext context, UserRequest userRequest);
        Task<Response> DeleteUserAsync(HttpContext context, int userId);
        Task<Response> GetAllNumberUserAsync(HttpContext context);
        Task<Response> InactiveUserAsync(HttpContext context, int userId);

        Task<Response> ActiveUserAsync(HttpContext context, int userId);

        //Task<Response> SetRoleToUser(HttpContext context, int userId);
    }
}
