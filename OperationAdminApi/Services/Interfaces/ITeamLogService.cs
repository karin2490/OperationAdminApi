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
    public interface ITeamLogService
    {

        Task<Response> GetAllTeamLogAsync(HttpContext context);
        Task<Response> GetTeamLogByTeamId(HttpContext context, int teamId);
        Task<Response> GetTeamLogyByUserName(HttpContext context, int userId);
        Task<Response> GetTeamLogByStartEndDate(HttpContext context, DateTime startDate, DateTime endDate);

    }
}
