using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminRepository.Common;
using System;
using System.Threading.Tasks;
using NLog.Fluent;
using M = OperationAdminDB.Models;
using OperationAdminApi.Services.Interfaces;
using OperationAdminApi.Infraestructure.Repository;
using OperationAdminRepository;
using E = OperationAdminApi.CommonObjects.Enum;
using OperationAdminRepository.Common.Enum;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OperationAdminApi.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository _accountRepository;
        private readonly TeamsLogRepository _teamLogRepository;
        private readonly TeamsRepository _teamRepository;

        Response response { get; set; }
        public AccountService(AccountRepository _accountRepository,TeamsLogRepository _teamLogRepository, TeamsRepository _teamRepository)
        {
            this._accountRepository = _accountRepository;
            this._teamLogRepository = _teamLogRepository;
            this._teamRepository = _teamRepository;
           
            this.response = new Response();
        }

        public async Task<Response> InsertAccountAsync(HttpContext context, AccountRequest request)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _accountRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    if(request.AccountName==default 
                        || request.AccountName == null 
                        || request.ClientName == default
                        || request.ClientName == null
                        || request.OperationResp==default
                        || request.OperationResp==null
                        || request.TeamId<0)
                    {
                        return "Data can't be null".ToResponse(false, ResponseType.NOT_ACCEPTABLE,
                            "Data can't be null");
                    }

                    var validation = await AccountValidate(request);
                    if (validation != E.AccountCodeValidation.Succesful)
                    {
                        return AccountResponse(validation);
                    }

                    M.Account account = 
                        new M.Account(
                            request.AccountName, 
                            request.ClientName, 
                            request.OperationResp, 
                            request.TeamId);

                    //get object request team
                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(request.TeamId);

                    TeamRequest teamR = new TeamRequest();
                    teamR.TeamId = team.TeamId;
                    teamR.TeamName = team.TeamName;

                    _accountRepository.Insert(account);
                    await _accountRepository.SaveAsync();

                    //TeamsLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "SET TEAM TO ACCOUNT", "ACCOUNT", teamR, "Add teamId to Account");
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }

                    return account.ToResponse("Added account");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                   
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InsertAccountAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> UpdateAccountAsync(HttpContext context, AccountRequest request)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _accountRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    if (request.AccountName == default
                        || request.AccountName == null
                        || request.ClientName == default
                        || request.ClientName == null
                        || request.OperationResp == default
                        || request.OperationResp == null
                        || request.TeamId < 0)
                    {
                        return "Data can't be null".ToResponse(false, ResponseType.NOT_ACCEPTABLE,
                            "Data can't be null");
                    }


                    M.Account account = _accountRepository.GetById(request.AccountId);
                    if(account==null)
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");

                    var validation = await AccountValidate(request);
                    if (validation != E.AccountCodeValidation.Succesful)
                    {
                        return AccountResponse(validation);
                    }

                    account.AccountName = request.AccountName;
                    account.ClientName = request.ClientName;
                    account.OperationResp = request.OperationResp;
                    account.TeamId = request.TeamId;

                    //get object request team
                    M.Team team = await _teamRepository.GetByIdAsync<M.Team>(request.TeamId);

                    TeamRequest teamR = new TeamRequest();
                    teamR.TeamId = team.TeamId;
                    teamR.TeamName = team.TeamName;

                    _accountRepository.Update(account);
                    await _accountRepository.SaveAsync();

                    //TeamsLog
                    if (userCache != null)
                    {
                        var teamActivity = _teamLogRepository.Teamlogger(userCache, "UPDATE", "ACCOUNT", teamR,  "Update teamId in Account:"+request.AccountName);
                        await _teamLogRepository.InsertTeamLog(teamActivity);
                    }

                    return account.ToResponse("Update account");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service UpdateAccountAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> DeleteAccountAsync(HttpContext context, int accountId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _accountRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Account account = await _accountRepository.GetByIdAsync(accountId);
                    if(account==null)
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");

                    var hasmembers = (await _accountRepository.GetUsersByAccountAsync(accountId)).Count();
                    if (hasmembers > 0)
                    {
                        return account.AccountName.ToResponse("It's not possible to delete this account, it's references with some users");
                    }

                    _accountRepository.Delete(account);
                    await _accountRepository.SaveAsync();

                    return account.ToResponse("Delete account");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service DeleteAccountAsync  Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> GetAllNumberAccountAsync(HttpContext context)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _accountRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    var accounts = await _accountRepository.GetAll()?.AsQueryable()?.ToListAsync();
                    response = accounts.ToResponse($"Number of Accounts: {accounts.Count}"); 
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                   
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetAllNumberAccountAsync  Ex: {ex}");
                throw ex;
            }
            return response;
        }

        //public async Task<Response> GetAccountByUserAsync(HttpContext context)
        //{
        //    try
        //    {
        //        var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
        //        var userLogin = await _accountRepository.GetByIdAsync<M.User>(userId);

        //        if (userLogin.UserId != 0)
        //        {
        //            var account = await _accountRepository.GetAccountByIdAsync(userId);
        //        }
        //        else
        //        {
        //            response.Type = ResponseType.UNAUTHORIZED;
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"An unhandled exception occured in User Service GetAccountByUserAsync  Ex: {ex}");
        //        throw ex;
        //    }
        //}

        public async  Task<Response> GetAllUserByAccountAsync(HttpContext context, int accountId)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _accountRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    var users = await _accountRepository.GetUsersByAccountAsync(accountId);
                    response = users.ToResponse($"Number of Accounts:{users.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                   
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetAllUserByAccountAsync  Ex: {ex}");
                throw ex;
            }

            return response;
        }

        public async Task<Response> GetDropdownAccountListAsync(HttpContext context)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _accountRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    var accounts = await _accountRepository.GetDropdownAsync();
                    response = accounts.ToResponse($"Number of accounts {accounts.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InsertAccountAsync  Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> ActiveAccountAsync(HttpContext context, int id)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _accountRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Account account = await _accountRepository.GetByIdAsync(id);
                    if (account == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }
                    account.Status = true;

                    _accountRepository.Update(account);
                    await _accountRepository.SaveAsync();
                    return account.ToResponse("Active Account");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InsertAccountAsync  Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> InactiveAccountAsync(HttpContext context, int id)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _accountRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Account account = _accountRepository.GetById(id);
                    if (account == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }

                    account.Status = false;

                    _accountRepository.Update(account);
                    await _accountRepository.SaveAsync();
                    return account.ToResponse("Inactive Account");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
               
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InsertAccountAsync  Ex: {ex}");
                throw ex;
            }
            return response;
        }

        private async Task<E.AccountCodeValidation> AccountValidate(AccountRequest accRequest,bool isNew=false)
        {
            var account=(await _accountRepository.GetAccountsOfTeam(accRequest.TeamId))
                .Where(W=>W.AccountName.ToLower()==accRequest.AccountName.ToLower()).FirstOrDefault();

            if ((isNew && account != null) || (account != null && account.AccountId != accRequest.AccountId))
                return E.AccountCodeValidation.AccountDuplicate;

            return E.AccountCodeValidation.Succesful;
        }

        private Response AccountResponse(E.AccountCodeValidation validation)
        {
            if (validation == E.AccountCodeValidation.AccountDuplicate)
            {
                return "Account Duplicated".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Account duplicated");
            }
            else
            {
                return "valid".ToResponse("Valid");
            }
        }
    }
}
