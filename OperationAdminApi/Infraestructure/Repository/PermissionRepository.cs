using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminDB.Data;
using Microsoft.EntityFrameworkCore;
using OperationAdminRepository.Repository;
using System.Collections.Generic;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class PermissionRepository:RepositoryBase<M.Permission>
    {
        private readonly OperationAdminContext DBCon;
        public PermissionRepository(OperationAdminContext DBContext)
            : base(DBContext)
        {
            this.DBCon = DBContext;
        }

        public async Task<List<D.PermissionDTO>> GetDropdownPermission()
        {
            List<D.PermissionDTO> permissions = await (from PERMISSION in DBCon.Permissions
                                                       orderby PERMISSION.PermissionDescrip ascending
                                                       select new D.PermissionDTO
                                                       {
                                                           PermissionId=PERMISSION.PermissionId,
                                                           PermissionDescrip=PERMISSION.PermissionDescrip,
                                                       }).ToListAsync();
            return permissions;
        }

        public async Task<D.PermissionDTO> GetPermissionById(int permId)
        {
            D.PermissionDTO permission=await(from PERMISSION in DBCon.Permissions
                                             where PERMISSION.PermissionId==permId
                                             select new D.PermissionDTO 
                                             { 
                                                PermissionId=PERMISSION.PermissionId,
                                                PermissionDescrip=PERMISSION.PermissionDescrip

                                             }).FirstOrDefaultAsync();
            return permission;
        }


       
    }
}
