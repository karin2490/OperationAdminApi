using Microsoft.AspNetCore.Http;
using OperationAdminApi.CommonObjects.Request;
using OperationAdminApi.Infraestructure.Repository;
using OperationAdminRepository;
using OperationAdminRepository.Common;
using OperationAdminRepository.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using E = OperationAdminApi.CommonObjects.Enum;
using NLog.Fluent;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminApi.Services.Interfaces;

namespace OperationAdminApi.Services.Implementations
{
    public class PermissionService:IPermissionService
    {
        private readonly PermissionRepository _permissionRepository;
        Response response { get; set; }

        public PermissionService(PermissionRepository _permissionRepository)
        {
            this._permissionRepository = _permissionRepository;
            this.response = new Response();
        }

     
        public async Task<Response> GetDropDownPermissionAsync(HttpContext context)
        {
            try
            {
                var userCache = Utils.UtilsMethods.GetUserCacheFromContext(context.User);
                var userLogin = await _permissionRepository.GetByIdAsync<M.User>(userCache.UserId);

                if (userLogin.UserId != 0)
                {
                    var permissions =await _permissionRepository.GetDropdownPermission();
                    response = permissions.ToResponse($"Number of accounts {permissions.Count}");

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

    }
}
