using System.Linq;
using System.Threading.Tasks;
using M = OperationAdminDB.Models;
using D = OperationAdminApi.CommonObjects.DTOs;
using OperationAdminDB.Data;
using Microsoft.EntityFrameworkCore;
using OperationAdminRepository.Repository;
using System.Collections.Generic;
using System;

namespace OperationAdminApi.Infraestructure.Repository
{
    public class UsersRepository:RepositoryBase<M.User>
    {
        private readonly OperationAdminContext DBCon;

        public UsersRepository(OperationAdminContext DBContext)
            : base(DBContext)
        {
            this.DBCon = DBContext;
        }


        public async Task<List<D.UsersDTO>> GetUsersInfo()
        {
            List<D.UsersDTO> users = await (from USERS in DBCon.Users
                                            join ACCOUNTS in DBCon.Accounts on USERS.AccountId equals ACCOUNTS.AccountId
                                            join ROLES in DBCon.Roles on USERS.RoleId equals ROLES.RoleId
                                            join TEAMS in DBCon.Teams on ACCOUNTS.TeamId equals TEAMS.TeamId into u
                                            from TEAMS in u.DefaultIfEmpty()
                                            
                                            select new D.UsersDTO
                                            {
                                                UserId=USERS.UserId,
                                                FirstName=USERS.FirstName,
                                                LastName=USERS.LastName,
                                                Email=USERS.Email,
                                                RoleId=ROLES.RoleId,
                                                AccountId=ACCOUNTS.AccountId,
                                                TeamId=TEAMS.TeamId,
                                                AdmissionDate=USERS.AdmissionDate,
                                                Status=USERS.Status

                                            }).Distinct().ToListAsync();
            return users;
        }

        public async Task<M.User> FindUserById(int id)
        {
            M.User user = await DBCon.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return user;
        }

        public async Task<bool> ExistsActiveUserById(int userId)
        {
            return await DBCon.Users.AsNoTracking().AnyAsync(at => at.UserId == userId && at.Status);
        }
        public async Task<bool> IsValidEmailUserAsync(string email)
        {
            return await DBCon.Users.AsNoTracking().AnyAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public D.UserInfoDTO ResponseDataUser(M.User user)
        {
            return new D.UserInfoDTO
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Status = user.Status
            };
        }
        
    }
}
