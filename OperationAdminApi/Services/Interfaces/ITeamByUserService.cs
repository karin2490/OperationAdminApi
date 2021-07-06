using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationAdminApi.Services.Interfaces
{
    public interface ITeamByUserService
    {
        Task<Response> InsertUsersByTeamAsync(HttpContext context, TeamByUserRequest request);
        Task<Response> GetUsersByTeamAsync(HttpContext context, int TeamId);

        Task<Response> UpateEndDateTeamAsync(HttpContext context, TeamByUserRequest request);

        Task<Response> ActiveTeamByUserAsync(HttpContext context, TeamByUserRequest request);

        Task<Response> InactiveTeamByUserAsync(HttpContext context, TeamByUserRequest request);
    }
}
