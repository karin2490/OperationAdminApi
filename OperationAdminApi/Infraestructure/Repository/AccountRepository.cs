
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
    public class AccountRepository : RepositoryBase<M.Account>
    {
        private readonly OperationAdminContext DBCon;
        public AccountRepository(OperationAdminContext DBContext)
            : base(DBContext)
        {
            this.DBCon = DBContext;
        }

        public async Task<D.AccountDTO> GetAccountByIdAsync(int id)
        {
            D.AccountDTO account = await (from ACCOUNTS in DBCon.Accounts
                                          where ACCOUNTS.AccountId == id
                                          select new D.AccountDTO
                                          {
                                              AccountId = ACCOUNTS.AccountId,
                                              AccountName = ACCOUNTS.AccountName,
                                              ClientName = ACCOUNTS.ClientName,
                                              OperationResp = ACCOUNTS.OperationResp,
                                              TeamId = ACCOUNTS.TeamId,
                                              Status = ACCOUNTS.Status
                                          }).FirstOrDefaultAsync();
            return account;
        }

        //public async Task<D.AccountDTO> GetAccountByUserId(int userId)
        //{

        //}

        public async Task<List<D.AccountForTeamDTO>> GetDropdownAsync()
        {
            List<D.AccountForTeamDTO> account = await (from ACCOUNT in DBCon.Accounts
                                                       orderby ACCOUNT.AccountName ascending
                                                       select new D.AccountForTeamDTO
                                                       {
                                                           AccountId = ACCOUNT.AccountId,
                                                           AccountName = ACCOUNT.AccountName,
                                                       }).ToListAsync();
            return account;
        }

        public async Task<List<D.UsersDTO>> GetUsersByAccountAsync(int id)
        {
            List<D.UsersDTO> users = await (from USERS in DBCon.Users
                                            join ACCOUNTS in DBCon.Accounts on USERS.AccountId equals ACCOUNTS.AccountId
                                            where ACCOUNTS.AccountId == id
                                            select new D.UsersDTO
                                            {
                                                UserId = USERS.UserId,
                                                FirstName = USERS.FirstName,
                                                LastName = USERS.LastName,
                                                Email = USERS.Email,
                                                AccountId = (int)USERS.AccountId,
                                                RoleId = USERS.RoleId,
                                                AdmissionDate = USERS.AdmissionDate,
                                                Status = USERS.Status,
                                            }).ToListAsync();
            return users;
        }
        public async Task<List<D.AccountForTeamDTO>> GetAccountsOfTeam(int id)
        {
            List<D.AccountForTeamDTO> accounts = await (from ACCOUNTS in DBCon.Accounts
                                                        join TEAMS in DBCon.Teams on ACCOUNTS.TeamId equals TEAMS.TeamId
                                                        where TEAMS.TeamId == id
                                                        select new D.AccountForTeamDTO
                                                        {
                                                            AccountId = ACCOUNTS.AccountId,
                                                            AccountName = ACCOUNTS.AccountName,
                                                        }).ToListAsync();

            return accounts;
        }


    }
}
