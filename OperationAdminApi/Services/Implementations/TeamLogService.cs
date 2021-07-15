using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminApi.Infraestructure.Repository;
using OperationAdminRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using E = OperationAdminApi.CommonObjects.Enum;
using OperationAdminRepository.Common.Enum;
using NLog.Fluent;
using OperationAdminRepository;
using OperationAdminApi.Services.Interfaces;

namespace OperationAdminApi.Services.Implementations
{
    public class TeamLogService:ITeamLogService
    {
        private readonly TeamsLogRepository _teamLogRepository;
        private readonly TeamsRepository _teamRepository;
        Response response { get; set; }
        public TeamLogService(TeamsLogRepository _teamlogRepository, TeamsRepository _teamRepository)
        {
            this._teamLogRepository = _teamlogRepository;
            this._teamRepository = _teamRepository;
            
            response = new Response();
        }

        public async Task<Response> GetAllTeamLogAsync(HttpContext context)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamLogRepository.GetByIdAsync<M.TeamLog>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var teamLog = await _teamLogRepository.GetAllTeamLogsAsync();
                    response = teamLog.ToResponse(string.Format($"Number of Logs{0}", teamLog.Count));
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetAllTeamLogAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetTeamLogByStartEndDate(HttpContext context, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamLogRepository.GetByIdAsync<M.TeamLog>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    if (startDate == null || endDate == null)
                        return response.ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Data can't be empty or null");

                    if (startDate > endDate)
                        return response.ToResponse(false, ResponseType.NOT_ACCEPTABLE, "End date can't be before Start date");

                    var teamLog = await _teamLogRepository.GetTeamsLogByDateRange(startDate,endDate);
                    response = teamLog.ToResponse(string.Format($"Number of Logs{0}", teamLog.Count));
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetTeamLogByStartEndDate Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetTeamLogByTeamId(HttpContext context, int teamId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamLogRepository.GetByIdAsync<M.TeamLog>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Team team = await _teamRepository.GetByIdAsync(teamId);

                    if (team == null)
                    {
                        return "Data not found"
                          .ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }
                    else
                    {
                        var teamLog = await _teamLogRepository.GetTeamsLogByTeamId(teamId);
                        response = teamLog.ToResponse(string.Format($"Number of Logs{0}", teamLog.Count));
                    }
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetTeamLogByTeamId Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetTeamLogyByUserName(HttpContext context, int userId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamLogRepository.GetByIdAsync<M.TeamLog>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    string Uname = userCache.FirtsName + " " + userCache.LastName;
                    var teamLogUsers = await _teamLogRepository.GetTeamsLogByUserName(Uname);
                    response = teamLogUsers.ToResponse(string.Format($"Number of Logs with users{0}", teamLogUsers.Count));
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetTeamLogByUserName Ex: {ex}");
                throw ex;
            }
            return response;
        }

        
    }
}
