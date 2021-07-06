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
    public interface ITeamService
    {
        Task<Response> InserTeamAsync(HttpContext context, TeamRequest request);
        Task<Response> UpdateTeamAsync(HttpContext context, TeamRequest request);
        Task<Response> DeleteTeamAsync(HttpContext context, int userId);
        Task<Response> GetAllNumberAsync(HttpContext context);
        Task<Response> GetAllWithAccountsAsync(HttpContext context);
        Task<Response> GetByIdAsync(HttpContext context, int teamId);
        Task<Response> GetAllAccountsOfATeamById(HttpContext context, int teamId);
        Task<Response> GetDropdownTeamListAsync(HttpContext context);
        Task<Response> ActiveTeamAsync(HttpContext context, int teamId);
        Task<Response> InactiveTeamAsync(HttpContext context, int teamId);
    }
}
