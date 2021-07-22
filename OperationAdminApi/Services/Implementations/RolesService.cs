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
using E = OperationAdminApi.CommonObjects.Enum;
using M = OperationAdminDB.Models;


namespace OperationAdminApi.Services.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly RolesRepository _rolesRepository;
        private readonly ModuleByRoleRepository _modByRoleRepository;
        Response response { get; set; }
        public RolesService(RolesRepository _rolesRepository, ModuleByRoleRepository _modByRoleRepository)
        {
            this._rolesRepository = _rolesRepository;
            this._modByRoleRepository = _modByRoleRepository;
            this.response = new Response();
        }

        public async Task<Response> InsertRoleNameAsync(HttpContext context, RoleRequest roleRequest)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    if (roleRequest.RoleDescrip == default || roleRequest.RoleDescrip == null)
                    {
                        return "Data can't be null".ToResponse(false, ResponseType.NOT_ACCEPTABLE,
                           "Data can't be null");
                    }
                    var validation = await RoleValidate(roleRequest);
                    if (validation != E.RoleValidation.Succesfull)
                    {
                        return RoleResponse(validation);
                    }

                    M.Role role = new M.Role(
                        roleRequest.RoleId,
                        roleRequest.RoleDescrip,
                        true
                        );

                    _rolesRepository.Insert(role);
                    await _rolesRepository.SaveAsync();

                    return role.ToResponse("Role Added");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetModuleByRoleId  Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> UpdateRoleAsync(HttpContext context, RoleRequest roleRequest)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin =await _rolesRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    if (roleRequest.RoleDescrip == default || roleRequest.RoleDescrip == null) { 
                    return "Data can't be empty or null"
                            .ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Data can't be empty or null");
                    }

                    M.Role role = _rolesRepository.GetById(roleRequest.RoleId);

                    if (role == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Data not found");
                    }

                    var validation = await RoleValidate(roleRequest);
                    if (validation != E.RoleValidation.Succesfull)
                    {
                        return RoleResponse(validation);
                    }

                    role.RoleDescrip = roleRequest.RoleDescrip;
                    _rolesRepository.Update(role);
                    await _rolesRepository.SaveAsync();

                    return role.ToResponse("Updated Role");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Some Error happened on Account Service GetRoleById Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> DeleteRoleAsync(HttpContext context, int roleId)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    M.Role role = await _rolesRepository.GetByIdAsync(roleId);
                    if (role == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Data not found");
                    }

                    var hasModules = (await _rolesRepository.GetModulesByRol(roleId)).Count();
                    if (hasModules > 0)
                    {
                        return role.RoleDescrip.ToResponse("It's not possible to delete this role because it's referenced in one or multiple modules");
                    }

                    _rolesRepository.Delete(role);
                    await _rolesRepository.SaveAsync();
                    return role.ToResponse("Deleted Role");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Some Error happened on Account Service GetRoleById Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> ActiveRoleAsync(HttpContext context, int roleId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Role role = _rolesRepository.GetById(roleId);
                    if (role == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Data not found");
                    }

                    role.Status = true;
                    _rolesRepository.Update(role);
                     await _rolesRepository.SaveAsync();

                    return role.ToResponse("Active Role");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Some Error happened on Account Service GetRoleById Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> InactiveRoleAsync(HttpContext context, int roleId)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    M.Role role = _rolesRepository.GetById(roleId);
                    if (role == null)
                    {
                        return "Data not found".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Data not found");
                    }

                    role.Status = false;
                    _rolesRepository.Update(role);
                    await _rolesRepository.SaveAsync();

                    return role.ToResponse("Active Role");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Some Error happened on Account Service GetRoleById Ex: {ex}");
                throw ex;
            }
        }

        public async Task<Response> SetModuleByRole(HttpContext context, ModuleByRoleRequest request)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    if (request.ModuleId < 0 || request.RoleId < 0)
                    {
                        return "Data can't be null".ToResponse(false, ResponseType.NOT_ACCEPTABLE,
                           "Data can't be null");
                    }

                    var validation = await ModuleByRoleValidate(request);
                    if (validation != E.ModuleByRoleValidation.Succesfull)
                    {
                        return ModuleByRoleResponse(validation);
                    }
                    M.ModuleByRole mbyr =
                        new M.ModuleByRole
                        (
                            request.RoleId,
                            request.ModuleId,
                            true
                        );

                    _rolesRepository.Insert(mbyr);
                    await _rolesRepository.SaveAsync();
                    return mbyr.ToResponse("Module By Rol Set");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service SetModuleByRole  Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> SetModulePermissions(HttpContext context, PermissionOnModuleRequest permOnModuleReq)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    if (permOnModuleReq.ModuleId < 0 || permOnModuleReq.PermissionId < 0)
                    {
                        return "Data can't be null".ToResponse(false, ResponseType.NOT_ACCEPTABLE,
                           "Data can't be null");
                    }

                    var validation = await PermissionOnModuleValidate(permOnModuleReq);
                    if (validation != E.PermissionOnModuleValidation.Successful)
                    {
                        return PermissionOnModuleResponse(validation);
                    }
                    M.PermissionOnModuleByRole mbyr =
                        new M.PermissionOnModuleByRole
                        (
                            permOnModuleReq.ModuleId,
                            permOnModuleReq.PermissionId,
                            permOnModuleReq.RoleId,
                            true
                        );

                    _rolesRepository.Insert(mbyr);
                    await _rolesRepository.SaveAsync();
                    return mbyr.ToResponse("Module Permissions By Rol Set");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service SetModuleByRole  Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetRoleById(HttpContext context, int roleId)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    var role = await _rolesRepository.GetRoleById(roleId);
                    if (role != null)
                    {
                        response = role.ToResponse("Role found");
                    }
                    else
                    {
                        response = role.ToResponse("Role not found");
                    }
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"Some Error happened on Account Service GetRoleById Ex: {ex}");
                throw ex;
            }

            return response;
        }

        public async Task<Response> GetDropDownRolesAsync(HttpContext context)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    var roles = await _rolesRepository.GetDropDownRoleAsync();
                    response = roles.ToResponse($"Number of roles {roles.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetDropDownRolesAsync  Ex: {ex}");
                throw ex;
            }
            return response;
        }

        public async Task<Response> GetModuleByRoleId(HttpContext context, int RoleId)
        {
            try
            {
                var userId = Utils.UtilsMethods.GetUserCacheFromContext(context.User).UserId;
                var userLogin = await _rolesRepository.GetByIdAsync<M.User>(userId);

                if (userLogin.UserId != 0)
                {
                    var roles = await _rolesRepository.GetModulesByRol(RoleId);
                    response = roles.ToResponse($"Number of modules on role {roles.Count}");
                }
                else
                {
                    response.Type = ResponseType.UNAUTHORIZED;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An unhandled exception occured in User Service GetModuleByRoleId  Ex: {ex}");
                throw ex;
            }
            return response;
        }



        private async Task<E.PermissionOnModuleValidation> PermissionOnModuleValidate(PermissionOnModuleRequest permOnModRequest, bool isNew = false)
        {
            var permOnMod = (await _rolesRepository.GetPermissionOnModule(permOnModRequest))
                .Where(W => W.ModuleId == permOnModRequest.ModuleId && W.PermissionId == permOnModRequest.PermissionId
                && W.RoleId == permOnModRequest.RoleId).FirstOrDefault();

            if ((isNew && permOnMod != null) || (permOnMod != null && permOnMod.ModuleId != permOnModRequest.ModuleId &&
                    permOnMod.PermissionId != permOnModRequest.PermissionId && permOnMod.RoleId != permOnModRequest.RoleId))
                return E.PermissionOnModuleValidation.PermissionOnModuleDuplicate;

            return E.PermissionOnModuleValidation.Successful;
        }

        private async Task<E.ModuleByRoleValidation> ModuleByRoleValidate(ModuleByRoleRequest mbrReq, bool isNew = false)
        {
            var modByRole = (await _modByRoleRepository.GetModuleByRoleForRequest(mbrReq))
                .Where(W => W.ModuleId == mbrReq.ModuleId && W.RoleId == mbrReq.RoleId).FirstOrDefault();

            if ((isNew && modByRole != null) || (modByRole != null && modByRole.ModuleId != mbrReq.ModuleId && modByRole.RoleId != mbrReq.RoleId))
                return E.ModuleByRoleValidation.ModuleByRoleDuplicate;

            return E.ModuleByRoleValidation.Succesfull;
        }

        private async Task<E.RoleValidation> RoleValidate(RoleRequest request, bool isNew = false)
        {
            var roles = (await _rolesRepository.GetRoleById(request.RoleId))
                  .Where(W => W.RoleDescrip.ToLower() == request.RoleDescrip.ToLower()).FirstOrDefault();

            if ((isNew && roles != null) || (roles != null && roles.RoleId != request.RoleId))
                return E.RoleValidation.RoleDuplicate;

            return E.RoleValidation.Succesfull;

        }
        private Response ModuleByRoleResponse(E.ModuleByRoleValidation validation)
        {
            if (validation == E.ModuleByRoleValidation.ModuleByRoleDuplicate)
            {
                return "Module and Role Duplicated".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Module and Role duplicated");
            }
            else
            {
                return "valid".ToResponse("Valid");
            }
        }

        private Response PermissionOnModuleResponse(E.PermissionOnModuleValidation validation)
        {
            if (validation == E.PermissionOnModuleValidation.PermissionOnModuleDuplicate)
            {
                return "Module and Permission Duplicated".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Module and Permission duplicated");
            }
            else
            {
                return "valid".ToResponse("Valid");
            }
        }

        private Response RoleResponse(E.RoleValidation validation)
        {
            if (validation == E.RoleValidation.RoleDuplicate)
            {
                return "Role Duplicated".ToResponse(false, ResponseType.NOT_ACCEPTABLE, "Role Duplicated");
            }
            else
            {
                return "valid".ToResponse("Valid");
            }
        }



    }
}
