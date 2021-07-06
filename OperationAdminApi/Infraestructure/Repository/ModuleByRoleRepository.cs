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
    public class ModuleByRoleRepository:RepositoryBase<M.ModuleByRole>
    {
        private readonly OperationAdminContext DBCon;
        public ModuleByRoleRepository(OperationAdminContext DBContext)
            : base(DBContext)
        {
            this.DBCon = DBContext;
        }

       
        public async Task<List<D.ModuleByRoleDTO>> GetModuleByRoleId(int RoleId)
        {
            List<D.ModuleByRoleDTO> moduleByRole = await (from MODBYROLE in DBCon.ModuleByRoles
                                                          join MODULE in DBCon.Modules on MODBYROLE.ModuleId equals MODULE.ModuleId
                                                          join ROLE in DBCon.Roles on MODBYROLE.RoleId equals ROLE.RoleId
                                                          where MODBYROLE.RoleId == RoleId
                                                          select new D.ModuleByRoleDTO
                                                          {
                                                              ModuleByRoleId = MODBYROLE.ModuleByRoleId,
                                                              ModuleId = MODBYROLE.ModuleId,
                                                              ModuleDescrip = MODULE.ModuleDescrip,
                                                              RoleId = MODBYROLE.RoleId,
                                                              RoleDescrip = ROLE.RoleDescrip,
                                                              Status = MODBYROLE.Status
                                                          }).ToListAsync();
            return moduleByRole;
        }

        public async Task<List<D.ModuleByRoleDTO>> GetModuleByRoleForRequest(ModuleByRoleRequest mbrReq)
        {
            List<D.ModuleByRoleDTO> moduleByRole = await (from MODBYROLE in DBCon.ModuleByRoles
                                                          join MODULE in DBCon.Modules on MODBYROLE.ModuleId equals MODULE.ModuleId
                                                          join ROLE in DBCon.Roles on MODBYROLE.RoleId equals ROLE.RoleId
                                                          where MODBYROLE.RoleId == mbrReq.RoleId && MODBYROLE.ModuleId==mbrReq.ModuleId
                                                          select new D.ModuleByRoleDTO
                                                          {
                                                              ModuleByRoleId = MODBYROLE.ModuleByRoleId,
                                                              ModuleId = MODBYROLE.ModuleId,
                                                              ModuleDescrip = MODULE.ModuleDescrip,
                                                              RoleId = MODBYROLE.RoleId,
                                                              RoleDescrip = ROLE.RoleDescrip,
                                                              Status = MODBYROLE.Status
                                                          }).ToListAsync();
            return moduleByRole;
        }
    }
}
