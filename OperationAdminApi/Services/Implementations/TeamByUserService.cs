using Microsoft.AspNetCore.Http;
using NLog.Fluent;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminApi.Infraestructure.Repository;
using OperationAdminApi.Services.Interfaces;
using OperationAdminRepository;
using OperationAdminRepository.Common;
using OperationAdminRepository.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using E = OperationAdminApi.CommonObjects.Enum;


namespace OperationAdminApi.Services.Implementations
{
    public class TeamByUserService : ITeamByUserService
    {
        private readonly TeamsRepository _teamRepository;
        private readonly TeamByUserRepository _teamByUserRepository;
        private readonly TeamsLogRepository _teamLogRepository;
        private readonly UsersRepository _usersRepository;
        Response response { get; set; }

        public TeamByUserService(TeamByUserRepository _teamByUserRepository, TeamsRepository _teamRepository,
            TeamsLogRepository _teamLogRepository, UsersRepository _usersRepository)
        {
            this._teamByUserRepository = _teamByUserRepository;
            this._teamRepository = _teamRepository;
            this._teamLogRepository = _teamLogRepository;
            this._usersRepository = _usersRepository;
            response = new Response();
        }

        public async Task<Response> InsertUsersByTeamAsync(HttpContext context, TeamByUserRequest request)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamByUserRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    if( request.TeamId<0 || request.UserId<0 
                        || request.StartDate ==default 
                        || request.StartDate==null
                        || request.EndDate==default 
                        || request.EndDate == null)
                    {
                        return "Data can't be null".ToResponse(false, ResponseType.NOT_ACCEPTABLE,
                            "Data can't be null");
                    }

                    var validaStartDate = await TeamStartDateValidate(request);
                    var validaEndDate = await TeamEndDateValidate(request);
                    if(validaStartDate!=E.TeamByUserValidation.Succesful && validaEndDate != E.TeamByUserValidation.Succesful)
                    {
                        return TeamByUserResponse(validaStartDate);
                    }
                    M.TeamByUser teamByUser = new M.TeamByUser(
                        request.TeamId,
                        request.UserId,
                        request.StartDate,
                        request.EndDate,
                        DateTime.Now,
                        true
                        );

                    //get object request team
                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(request.TeamId);

                    TeamRequest teamR = new TeamRequest();
                    teamR.TeamId = team.TeamId;
                    teamR.TeamName = team.TeamName;

                    _teamByUserRepository.Insert(teamByUser);
                    await _teamByUserRepository.SaveAsync();

                    // TeamsLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "SET USER INTO A TEAM", "TEAMBYUSER", teamR, "Set user on a team");
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }

                    return teamByUser.ToResponse("User on a Team");

                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InsertUsersByTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }
        public async Task<Response> GetUsersByTeamAsync(HttpContext context, int TeamId)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _teamByUserRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    var usersByTeam = await _teamByUserRepository.GetUsersOnTeam(TeamId);
                    response = usersByTeam.ToResponse($"Number of Users on Team:{usersByTeam.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetUsersByTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> UpateEndDateTeamAsync(HttpContext context, TeamByUserRequest request)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamByUserRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    if (request.EndDate == default
                         || request.EndDate == null)
                    {
                        return "Data can't be null".ToResponse(false, ResponseType.NOT_ACCEPTABLE,
                          "Data can't be null");
                    }

                    M.TeamByUser teamByUser = _teamByUserRepository.GetById(request.TeamByUserId);
                    if (teamByUser == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }

                    var validation = await TeamEndDateValidate(request);
                    if(validation != E.TeamByUserValidation.Succesful)
                    {
                        return TeamByUserResponse(validation);
                    }

                    teamByUser.EndDate = request.EndDate;

                    //get object request team
                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(request.TeamId);

                    TeamRequest teamR = new TeamRequest();
                    teamR.TeamId = team.TeamId;
                    teamR.TeamName = team.TeamName;

                    _teamByUserRepository.Update(teamByUser);
                    await _teamByUserRepository.SaveAsync();

                    //TeamsLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "UPDATE", "TEAMBYUSER", teamR, "Update EndDate in TeamByUser:" + teamR.TeamName);
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }

                    return teamByUser.ToResponse("Update EndDate in TeamByUser");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service UpateEndDateTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }



        public async Task<Response> ActiveTeamByUserAsync(HttpContext context, TeamByUserRequest request)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamByUserRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var usersByTeam = await _teamByUserRepository.GetByIdAsync(request.TeamByUserId);
                    if (usersByTeam == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }

                    usersByTeam.Status = true;

                    _teamByUserRepository.Update(usersByTeam);
                    await _teamByUserRepository.SaveAsync();
                    return _teamByUserRepository.ToResponse("Active TeamByUser");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service ActiveTeamByUserAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> InactiveTeamByUserAsync(HttpContext context, TeamByUserRequest request)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamByUserRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var usersByTeam = await _teamByUserRepository.GetByIdAsync(request.TeamByUserId);
                    if (usersByTeam == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }

                    usersByTeam.Status = true;

                    _teamByUserRepository.Update(usersByTeam);
                    await _teamByUserRepository.SaveAsync();
                    return _teamByUserRepository.ToResponse("Inactive TeamByUser");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InactiveTeamByUserAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        private async Task<E.TeamByUserValidation> TeamStartDateValidate(TeamByUserRequest teamByUserReq, bool isNew = false)
        {
            var teamByUser = (await _teamByUserRepository.GetUsersOnTeam(teamByUserReq.TeamByUserId))
                .Where(W => W.StartDate == teamByUserReq.StartDate)
               .FirstOrDefault();

            if ((isNew && teamByUser != null) || (teamByUser != null && teamByUser.StartDate != teamByUserReq.StartDate))
                return E.TeamByUserValidation.StartDateDuplicate;

            return E.TeamByUserValidation.Succesful;
        }
        private async Task<E.TeamByUserValidation> TeamEndDateValidate(TeamByUserRequest teamByUserReq, bool isNew = false)
        {
            var teamByUser = (await _teamByUserRepository.GetUsersOnTeam(teamByUserReq.TeamByUserId))
                .Where(W => W.EndDate == teamByUserReq.EndDate)
               .FirstOrDefault();

            if ((isNew && teamByUser != null) || (teamByUser != null && teamByUser.EndDate != teamByUserReq.EndDate))
                return E.TeamByUserValidation.EndDateDuplicate;

            return E.TeamByUserValidation.Succesful;
        }

        private Response TeamByUserResponse(E.TeamByUserValidation validation)
        {
            if (validation == E.TeamByUserValidation.EndDateDuplicate)
            {
                return "End-Date Duplicated".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "End-Date  duplicated");
            }
            else
            {
                return "valid".ToResponse("Valid");
            }
        }


    }
}
