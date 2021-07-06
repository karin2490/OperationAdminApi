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
using Microsoft.EntityFrameworkCore;
using OperationAdminApi.Services.Interfaces;

namespace OperationAdminApi.Services.Implementations
{
    public class TeamService:ITeamService
    {
        private readonly TeamsRepository _teamRepository;
        private readonly TeamsLogRepository _teamLogRepository;

        Response response { get; set; }
        public TeamService(TeamsRepository _teamRepository, TeamsLogRepository _teamLogRepository)
        {
            this._teamRepository = _teamRepository;
            this._teamLogRepository = _teamLogRepository;
            response = new Response();
        }

        public async Task<Response> InserTeamAsync(HttpContext context, TeamRequest request)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    if(request.TeamName=="" || request.TeamName == null)
                    {
                        response.Type = ResponseType.NOT_ACCEPTABLE;
                        response.Message =("Data can't be empty or null");
                        return response;
                    }
                    var validation = TeamValidate(request);
                    if(validation!=E.TeamCodeValidation.Succesful)
                    {
                        return response = TeamResponse(validation);
                    }

                    M.Team team = new M.Team(request.TeamName);

                    _teamRepository.Insert(team);
                    await _teamRepository.SaveAsync();

                    //TeamLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "INSERT", "TEAM", request, "Add new Team");
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }

                    return team.ToResponse("Team inserted");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InserTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> UpdateTeamAsync(HttpContext context, TeamRequest request)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    if (request.TeamName == "" || request.TeamName == null)
                    {
                        response.Type = ResponseType.NOT_ACCEPTABLE;
                        return response;
                    }

                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(request.TeamId);
                    if (team == null)
                    {
                        response.Type = ResponseType.NO_FOUND;
                        return response;
                    }

                    var validation = TeamValidate(request);
                    if (validation != E.TeamCodeValidation.Succesful)
                    {
                        return response = TeamResponse(validation);
                    }

                    team.TeamName = request.TeamName;
                    _teamRepository.Update(team);
                    await _teamRepository.SaveAsync();

                    //TeamLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "UPDATE", "TEAM", request, "Update Team: "+request.TeamName);
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }
                    return team.ToResponse("Team Updated");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service UpdateTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> DeleteTeamAsync(HttpContext context, int id)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(id);
                    if (team == null)
                    {
                        return team.ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }

                    TeamRequest teamR = new TeamRequest();
                    teamR.TeamId = team.TeamId;
                    teamR.TeamName = team.TeamName;
                    
                    _teamRepository.Delete(team);
                    await _teamRepository.SaveAsync();
                   
                    //TeamLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "DELETE", "TEAM", teamR, "Delete Team: " + team.TeamName);
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }
                    return team.ToResponse("Team Deleted");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service DeleteTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetAllNumberAsync(HttpContext context)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var teams = await _teamRepository.GetAll()?.AsQueryable()?.ToListAsync();
                    response = teams.ToResponse($"Number of Teams: {teams.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InserTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetAllWithAccountsAsync(HttpContext context)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var teamsWithAccounts =await _teamRepository.GetAllTeamsWAccount();
                    response = teamsWithAccounts.ToResponse($"Number of teams with Accounts: {teamsWithAccounts.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InserTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetByIdAsync(HttpContext context, int teamId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(teamId);
                    if (team == null)
                    {
                        return team.ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }
                    else
                    {
                        response = team.ToResponse("Team Found");
                    }
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InserTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetAllAccountsOfATeamById(HttpContext context, int teamId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var accounts = await _teamRepository.GetAccountsOfTeam(teamId);
                    response = accounts.ToResponse($"Number of accounts by Team: {accounts.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InserTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetDropdownTeamListAsync(HttpContext context)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var teams = await _teamRepository.GetDropdownTeamsAsync();
                    response = teams.ToResponse($"Number of Records in Teams {teams.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InserTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> ActiveTeamAsync(HttpContext context, int teamId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(teamId);
                    if (team == null) {
                        return team.ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }
                    team.Status = true;
                    _teamRepository.Update(team);
                    await _teamRepository.SaveAsync();

                    TeamRequest teamR = new TeamRequest();
                    teamR.TeamId = team.TeamId;
                    teamR.TeamName = team.TeamName;

                    //TeamLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "ACTIVE", "TEAM", teamR,  "Update Team: " + team.TeamName);
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }
                    return team.ToResponse("Active Team");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InserTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> InactiveTeamAsync(HttpContext context, int teamId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _teamRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(teamId);
                    if (team == null)
                    {
                        return team.ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }
                    team.Status = false;

                    TeamRequest teamR = new TeamRequest();
                    teamR.TeamId = team.TeamId;
                    teamR.TeamName = team.TeamName;


                    _teamRepository.Update(team);
                    await _teamRepository.SaveAsync();

                    //TeamLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "INACTIVE", "TEAM", teamR, "Update Team: " + team.TeamName);
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }
                    return team.ToResponse("Inactive Team");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                   
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InserTeamAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        private E.TeamCodeValidation TeamValidate(TeamRequest tRequest, bool isNew=false)
        {
            M.Team team = this._teamRepository.GetAll().Where(W => W.TeamName.ToLower() == tRequest.TeamName.ToLower()).FirstOrDefault();
            if ((isNew && team != null) || (team != null && team.TeamId != tRequest.TeamId))
                return E.TeamCodeValidation.TeamDuplicate;

            return E.TeamCodeValidation.Succesful;
        }

        private Response TeamResponse(E.TeamCodeValidation validation)
        {
            if(validation==E.TeamCodeValidation.TeamDuplicate)
            {
                return "Team Duplicated".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Team is duplicated");
            }
            else
            {
                return "valid".ToResponse("Valid");
            }
        }
    }
}
