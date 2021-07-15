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
using D = OperationAdminApi.CommonObjects.DTOs;
using M = OperationAdminDB.Models;
using E= OperationAdminApi.CommonObjects.Enum;
using Microsoft.EntityFrameworkCore;
using U = OperationAdminRepository.Utils;
using AutoMapper;

namespace OperationAdminApi.Services.Implementations
{
    public class UserService:IUserService
    {
        private readonly UsersRepository _userRepository;
        private readonly IMapper _mapper;
        Response response { get; set; }

        public UserService(UsersRepository _userRepository, IMapper mapper) 
        {
            this._userRepository = _userRepository;
            this._mapper = mapper;
            response = new Response();
        }

        public async Task<Response> ActiveUserAsync(HttpContext context, int userId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _userRepository.GetByIdAsync(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.User user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }

                    user.Status = true;
                     _userRepository.Update(user);
                    await _userRepository.SaveAsync();

                    var data = _userRepository.ResponseDataUser(user);
                    return data.ToResponse("User is active");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service ActiveUserAsync  Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> DeleteUserAsync(HttpContext context, int userId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _userRepository.GetByIdAsync(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.User user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }
                    if (user.UserId == 1)
                    {
                        return "It's not possible delete this user".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Delete user not allowed");
                    }

                     _userRepository.Delete(user);
                    await _userRepository.SaveAsync();

                    var data = _userRepository.ResponseDataUser(user);
                    return data.ToResponse("User Deleted");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service DeleteUserAsync  Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> GetUserAsync(HttpContext context, int Id)
        {
            try
            {
                var UserId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _userRepository.GetByIdAsync(UserId);

                if(userLogin.UserId != 0)
                {
                    M.User user = await _userRepository.FindUserById(Id);
                    if (user!= default(M.User))
                    {
                        return _mapper.Map<D.UsersDTO>(user).ToResponse("User found.");
                    }
                    else
                    {
                        return response.ToResponse("User not found.");
                    }
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetUserAsync Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> GetAllNumberUserAsync(HttpContext context)
        {
            try
            {
                var Id = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _userRepository.GetByIdAsync(Id);

                if (userLogin.UserId !=0)
                {
                    List<D.UsersDTO> users = await _userRepository.GetUsersInfo();
                    return users.ToResponse(users.Count.ToString());
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetAllNumberUserAsync Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> InactiveUserAsync(HttpContext context, int userId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _userRepository.GetByIdAsync(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.User user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }

                    user.Status = false;
                    _userRepository.Update(user);
                    await _userRepository.SaveAsync();

                    var data = _userRepository.ResponseDataUser(user);
                    return data.ToResponse("User is inactive");

                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InactiveUserAsync Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> InsertUserAsync(HttpContext context, UserRequest userRequest)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _userRepository.GetByIdAsync(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var validation = await UserValidate(userRequest.Email, true);
                    if (validation != E.UserValidation.Success)
                    {
                        return userResponse(validation);
                    }

                    var user = new M.User();
                    user.Email = userRequest.Email;
                    user.PassEncrypted = U.MD5.Encrypt(userRequest.Pass);
                    user.FirstName = userRequest.FirstName;
                    user.LastName = userRequest.LastName;
                    user.AdmissionDate = userRequest.AdmissionDate;
                    user.Status = userRequest.Status;
                    user.AccountId = userRequest.AccountId;
                    user.RoleId = userRequest.RoleId;

                     _userRepository.Insert(user);
                    await _userRepository.SaveAsync();

                    var data = _userRepository.ResponseDataUser(user);
                    return data.ToResponse("User Added");

                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service InsertUserAsync Ex: {ex}");
                throw ex;
            }
        }

        private async Task<E.UserValidation> UserValidate(string Email,bool newUser = false)
        {
            M.User User = await _userRepository.GetAll()?.AsQueryable().Where(W => W.Email.ToLower() == Email.ToLower())?.FirstOrDefaultAsync() ?? null;

            return E.UserValidation.Success;
        }

        private Response userResponse(E.UserValidation validation)
        {
            switch (validation)
            {
                case E.UserValidation.InvalidEmail:
                    return "Email invalid".ToResponse("The email is invalid");
                case E.UserValidation.InvalidPass:
                    return "Password isn invalid".ToResponse("The password has an invalid format");
                case E.UserValidation.DuplicateOnEmail:
                    return "The Email is duplicate".ToResponse("Already exists an Email like this one");
                default:
                    return "Valido".ToResponse("Valido");
            }
        }

        //public async Task<Response> SetRoleToUser(HttpContext context, int userId)
        //{
        //    try
        //    {
        //        var Id = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
        //        var userLogin = await _userRepository.GetByIdAsync(Id);

        //        if (userLogin.UserId != 0)
        //        {

        //        }
        //        else
        //        {
        //            response.Type = ResponseType.UNAUTHORIZED;
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"An unhandled exception occured in User Service  Ex: {ex}");
        //        throw ex;
        //    }
        //}

        public async Task<Response> UpdateUserAsync(HttpContext context, UserRequest userRequest)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _userRepository.GetByIdAsync(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.User user = await _userRepository.GetByIdAsync(userRequest.UserId);
                    if (user == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NO_FOUND, "Data not found");
                    }

                    user.FirstName = userRequest.FirstName;
                    user.LastName = userRequest.LastName;
                    user.Email = userRequest.Email;
                    user.PassEncrypted = userRequest.Pass != null ? U.MD5.Encrypt(userRequest.Pass) : user.PassEncrypted;
                    user.AccountId = userRequest.AccountId;
                    user.AdmissionDate = userRequest.AdmissionDate;
                    user.RoleId = userRequest.RoleId;

                     _userRepository.Update(user);
                    await _userRepository.SaveAsync();

                    var data = _userRepository.ResponseDataUser(user);
                    return data.ToResponse("User Updated");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service UpdateUserAsync  Ex: {ex}");
                throw ex;
            }
        }
    }
}
