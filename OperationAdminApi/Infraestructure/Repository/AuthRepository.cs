using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M=OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminRepository.Repository;
using OperationAdminDB.Data;
using OperationAdminApi.CommonObjects.Request;
using Microsoft.EntityFrameworkCore;
using OperationAdminApi.CommonObjects.DTOs;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class AuthRepository: RepositoryBase<M.User>
    {
        private readonly OperationAdminContext DBCon;
        public AuthRepository(OperationAdminContext DBContext)
            : base(DBContext)
        {
            this.DBCon = DBContext;
        }

        public async Task<M.User> GetloginAync(AuthorizationRequest authRequest)
        {
            var user= DBCon.Users.Where(X => X.Email == authRequest.Email && X.Status).FirstOrDefaultAsync();
            return await user;
        }

        public async Task<M.User> GetUserAsync(M.User userInfo)
        {
            return await DBCon.Users.Where(x => x.UserId == userInfo.UserId).FirstOrDefaultAsync();
        }

        public async Task<M.Token> GetTokenAsync(M.User profile)
        {
            return await DBCon.Tokens.Where(x => x.Email == profile.Email).FirstOrDefaultAsync();
        }

        public async Task<M.User> GetProfileFromTokenAsync(M.Token rtoken)
        {
            return await DBCon.Users.Where(x => x.Email == rtoken.Email).FirstOrDefaultAsync();
        }

        public async Task<M.Token> VerifyTokenAsync(string rToken)
        { 
            return await DBCon.Tokens.Where(m => m.TokenStr == rToken).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetRolesByUserAsync(int userId)
        {
            List<string> data = await (from ROLES in DBCon.Roles
                                       join USERS in DBCon.Users on ROLES.RoleId equals USERS.RoleId
                                       where USERS.Status == true && USERS.UserId == userId
                                       select ROLES.RoleDescrip).ToListAsync();
            return data;
        }

        public async Task<List<ModulePermissionsDTO>> GetPermissionOnModuleByUserIdAsync(int userId, bool Active = true)
        {
            List<ModulePermissionDTO> permisionOnModule = await (from USERS in DBCon.Users
                                                               join ROLES in DBCon.Roles on USERS.RoleId equals ROLES.RoleId
                                                               join MODBYROLE in DBCon.ModuleByRoles on USERS.RoleId equals MODBYROLE.RoleId
                                                               join PERM_MODbyROLE in DBCon.PermissionOnModuleByRoles on MODBYROLE.ModuleId equals PERM_MODbyROLE.ModuleId
                                                               where USERS.UserId == userId && MODBYROLE.Status == Active && PERM_MODbyROLE.Status == Active && ROLES.Status == true
                                                               group new { MODBYROLE.ModuleId, PERM_MODbyROLE.PermissionId } by new { MODBYROLE.ModuleId, PERM_MODbyROLE.PermissionId } into E
                                                               orderby E.Key.ModuleId, E.Key.PermissionId
                                                               select new ModulePermissionDTO
                                                               {
                                                                   ModuleId = E.Key.ModuleId,
                                                                   PermissionId = E.Key.PermissionId
                                                               }).ToListAsync();

            List<ModuleDTO> modules = await (from ROLES in DBCon.Roles
                                             join MODBYROLE in DBCon.ModuleByRoles on ROLES.RoleId equals MODBYROLE.RoleId
                                             join USERS in DBCon.Users on ROLES.RoleId equals USERS.RoleId
                                             where USERS.UserId == userId && MODBYROLE.Status == Active && ROLES.Status == Active
                                             group new { MODBYROLE.ModuleId } by new { MODBYROLE.ModuleId } into D
                                             select new ModuleDTO
                                             {
                                                 ModuleId = D.Key.ModuleId,
                                             }).ToListAsync();


            List<ModulePermissionsDTO> modulePermissions = (from MODULES in modules
                                                         select new ModulePermissionsDTO
                                                         {
                                                             ModuleId = MODULES.ModuleId,
                                                             PermissionsId = (from B in permisionOnModule
                                                                             where B.ModuleId == MODULES.ModuleId
                                                                             select B.PermissionId).ToList()
                                                         }).ToList();

            return modulePermissions;
        }
    }
}
