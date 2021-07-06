using Microsoft.AspNetCore.Http;
using NLog.Fluent;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminApi.Services.Interfaces;
using OperationAdminRepository.Common;
using OperationAdminRepository.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using E = OperationAdminApi.CommonObjects.Enum;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminApi.Infraestructure.Repository;
using OperationAdminRepository;
using AutoMapper;

namespace OperationAdminApi.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly UserProfileRepository _userProfRepository;
        private readonly UsersRepository _userRepository;
        private readonly IMapper _mapper;
        Response response { get; set; }
        public ProfileService(UserProfileRepository _userProfRepository, UsersRepository _userRepository, IMapper mapper)
        {
            this._userProfRepository = _userProfRepository;
            this._userRepository = _userRepository;
            _mapper = mapper;
            response = new Response();
        }
        public async Task<Response> AddProfileAsync(HttpContext context, UserProfileRequest request)
        {
            try
            {
                if(await UserAllowedAsync(context))
                {
                    if (!await ExistActiveUserByIdAsync(request.UserId))
                    {
                        return response.ToResponse(false, ResponseType.NO_FOUND, "User not found.");
                    }

                    M.UserProfile profile = new M.UserProfile(

                         request.UserId,
                         request.EnglishLevel,
                         request.EnglishLevel,
                         request.LinkCv,
                         true
                        );
                    
                    _userProfRepository.Insert(profile);
                    await _userProfRepository.SaveAsync();

                    return profile.ToResponse("Added User Profile");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service AddProfileAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> DeleteProfileAsync(HttpContext context, int userId)
        {
            try
            {
                if (await UserAllowedAsync(context))
                {
                    if (await ExistActiveUserByIdAsync(userId))
                    {
                        return response.ToResponse(false, ResponseType.NO_FOUND, "User not found.");
                    }

                    var profile = await _userProfRepository.FindUserprofileByIdAsync(userId);
                    if (profile == default(D.UserProfileDTO))
                    {
                        return Response.ToResponse(false,ResponseType.NO_FOUND, "User Profile not found.");
                    }

                    _userProfRepository.Delete(profile);
                    await _userProfRepository.SaveAsync();
                    return profile.ToResponse("User Profile Deleted");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service DeleteProfileAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

    

        public async Task<Response> GetProfileByIdAsync(HttpContext context, int userId, int id)
        {
            try
            {
                if (await UserAllowedAsync(context))
                {
                    if (await ExistActiveUserByIdAsync(userId))
                    {
                        return response.ToResponse(false, ResponseType.NO_FOUND, "User not found.");
                    }

                    var profile = await _userProfRepository.FindUserprofileByIdAsync(userId);
                    if (profile == default(D.UserProfileDTO))
                    {
                        return Response.ToResponse(false, ResponseType.NO_FOUND, "User Profile not found.");
                    }


                    return _mapper.Map<D.UserProfileDTO>(profile).ToResponse("User Profile Found");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetProfileAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> UpdateProfileAsync(HttpContext context, UserProfileRequest request)
        {
            try
            {
                if (await UserAllowedAsync(context))
                {
                    if (await ExistActiveUserByIdAsync(request.UserId))
                    {
                        return response.ToResponse(false, ResponseType.NO_FOUND, "User not found.");
                    }

                    if(request.UserId<0 
                        || request.EnglishLevel==default 
                        || request.EnglishLevel==null 
                        || request.TechnicalSkills==default
                        || request.TechnicalSkills==null 
                        || request.LinkCv==default 
                        || request.LinkCv == null)
                    {
                        return "Data can't be null".ToResponse(false, ResponseType.NOT_ACCEPTABLE,
                           "Data can't be null");
                    }

                    var profile = await _userProfRepository.FindUserprofileByIdAsync(request.UserId);
                    if (profile == default(D.UserProfileDTO))
                    {
                        return Response.ToResponse(false, ResponseType.NO_FOUND, "User Profile not found.");
                    }

                    M.UserProfile uProf = new M.UserProfile(
                    request.UserId,
                    request.EnglishLevel,
                    request.TechnicalSkills,
                    request.LinkCv,
                    true);
                    _userProfRepository.Update(uProf);
                    await _userProfRepository.SaveAsync();
                    return profile.ToResponse("User Profile Updated");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service UpdateProfileAsync Ex: {ex}");
                throw ex;
            }
            return response;
        }

        private int GetUserOfContext(HttpContext context)
        {
            return Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
        }
        private async Task<bool> UserAllowedAsync(HttpContext context)
        {
            var userLogin = await _userProfRepository.GetByIdAsync<M.User>(GetUserOfContext(context));
            if (userLogin == default(M.User))
            {
                return false;
            }
            if (userLogin.RoleId == 3)
                return true;

            else
               return false;
            
        }

        private async Task<bool> ExistActiveUserByIdAsync(int userId)
        {
            return await _userRepository.ExistsActiveUserById(userId);
        } 
    }
}
