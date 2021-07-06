using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminDB.Data;
using Microsoft.EntityFrameworkCore;
using OperationAdminRepository.Repository;
using System.Collections.Generic;
using OperationAdminApi.CommonObjects.Request;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class RolesRepository:RepositoryBase<M.Role>
    {
        private readonly OperationAdminContext DBCon;
        public RolesRepository(OperationAdminContext DBContext)
            :base(DBContext)
        {
            this.DBCon = DBContext;
        }

        public async Task<D.RoleDTO> GetRoleById(int RolId)
        {
            D.RoleDTO roles = await (from ROLES in DBCon.Roles
                                    where ROLES.RoleId == RolId
                                    select new D.RoleDTO 
                                    { 
                                    RoleId=ROLES.RoleId,
                                    RoleDescrip=ROLES.RoleDescrip,
                                    Status=ROLES.Status,
                                    }).FirstOrDefaultAsync();
            return roles;
        }
        public async Task<List<D.PermissionOnModuleDTO>> GetPermissionOnModule(PermissionOnModuleRequest request)
        {
            List<D.PermissionOnModuleDTO> permOnModule = await (from PERMonModule in DBCon.PermissionOnModuleByRoles
                                                                join MODULE in DBCon.Modules on PERMonModule.ModuleId equals MODULE.ModuleId
                                                                join PERMISSION in DBCon.Permissions on PERMonModule.PermissionId equals PERMISSION.PermissionId
                                                                join ROLE in DBCon.Roles on PERMonModule.RoleId equals ROLE.RoleId
                                                                where PERMonModule.PermissionId == request.PermissionId && PERMonModule.ModuleId == request.ModuleId
                                                                && PERMonModule.RoleId==request.RoleId
                                                                select new D.PermissionOnModuleDTO
                                                                {
                                                                   PermissionOnModuleByRoleId=PERMonModule.PermissionOnModuleByRoleId,
                                                                   PermissionId= PERMonModule.PermissionId,
                                                                   PermissionDescript=PERMISSION.PermissionDescrip,
                                                                   ModuleId=PERMonModule.ModuleId,
                                                                   ModuleDescrip=MODULE.ModuleDescrip,
                                                                   RoleId= PERMonModule.RoleId,
                                                                   RoleDescript=ROLE.RoleDescrip,
                                                                   Status =PERMonModule.Status

                                                                }).ToListAsync();
            return permOnModule;
        }

        public async Task<List<D.ModuleByRoleDTO>> GetModulesByRol(int RolId)
        {
            List<D.ModuleByRoleDTO> moduleByRol = await (from MODBYROL in DBCon.ModuleByRoles
                                                         join MODULE in DBCon.Modules on MODBYROL.ModuleId equals MODULE.ModuleId
                                                         join ROLE in DBCon.Roles on MODBYROL.RoleId equals ROLE.RoleId
                                                         where MODBYROL.RoleId == RolId
                                                         select new D.ModuleByRoleDTO
                                                         {
                                                             ModuleId=MODBYROL.ModuleId,
                                                             ModuleDescrip=MODULE.ModuleDescrip,
                                                             RoleId=MODBYROL.RoleId,
                                                             RoleDescrip=ROLE.RoleDescrip,
                                                             Status=MODBYROL.Status
                                                         }).ToListAsync();
            return moduleByRol;
        }

        public async Task<List<D.RoleDTO>> GetDropDownRoleAsync()
        {
            List<D.RoleDTO> roles = await (from ROLES in DBCon.Roles
                                           orderby ROLES.RoleDescrip ascending
                                           select new D.RoleDTO
                                           { 
                                           RoleId=ROLES.RoleId,
                                           RoleDescrip=ROLES.RoleDescrip,
                                           }).ToListAsync();
            return roles;
        }

    }
}
